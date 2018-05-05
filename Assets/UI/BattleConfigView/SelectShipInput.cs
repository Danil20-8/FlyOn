using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Assets.Global;

public class SelectShipInput: MonoBehaviour, IAutoInput
{
    [SerializeField]
    Dropdown main;

    [SerializeField]
    Dropdown second;

    public event Action<PlayerShip> onValueChanged;

    public PlayerShip value { get { return _value; } set { _value = value; Refresh(); } }
    PlayerShip _value;

    List<int> shipClasses;
    List<string> shipsNames;


    void Awake()
    {
        shipClasses = Enumerable.Range(1, 3).ToList();
        shipsNames = GameResources.GetShipsNames().ToList();

        main.ClearOptions();
        main.AddOptions(new List<string>() { Localization.GetGlobalString("Random"), Localization.GetGlobalString("OfClass"), Localization.GetGlobalString("OfName") });

        main.onValueChanged.AddListener(MainChanged);
        second.onValueChanged.AddListener(SecondChanged);
    }

    void Refresh()
    {
        switch (_value.type)
        {
            case PlayerShip.ShipType.Random:
                second.gameObject.SetActive(false);
                break;
            case PlayerShip.ShipType.RandomOfClass:
                second.gameObject.SetActive(true);
                second.ClearOptions();
                second.AddOptions(shipClasses.Select(i => i.ToString()).ToList());
                second.value = shipClasses.IndexOf(_value.shipClass);
                break;
            case PlayerShip.ShipType.Concrete:
                second.gameObject.SetActive(true);
                second.ClearOptions();
                second.AddOptions(shipsNames);
                second.value = shipsNames.IndexOf(_value.shipName);
                break;
        }

        if(onValueChanged != null)
            onValueChanged.Invoke(_value);
    }

    void MainChanged(int index)
    {
        _value.type = (PlayerShip.ShipType) index;
        Refresh();
    }

    void SecondChanged(int index)
    {
        switch(_value.type)
        {
            case PlayerShip.ShipType.RandomOfClass:
                _value.shipClass = shipClasses[index];
                break;
            case PlayerShip.ShipType.Concrete:
                _value.shipName = shipsNames[index];
                break;
        }
        if (onValueChanged != null)
            onValueChanged.Invoke(_value);
    }

    public bool IsAble(Type type)
    {
        return type == typeof(PlayerShip);
    }

    public void Init(ValueOption value)
    {
        this.value = (PlayerShip)value.value;

        onValueChanged += p => value.setAction(p);
    }
}

