using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.GameModels;
using System;
using MyLib.Algoriphms;
public class ShipKeyInput{

    readonly ComponentInput actions = new ComponentInput();
    List<KeyComponent> components = new List<KeyComponent>();

    PlayerUIBehaviour ui;
    public ShipKeyInput(ShipSystem ship)
    {
        ui = ScreenManager.GetScreen<PlayerUIBehaviour>();

        var shipSystem = ship;
        if (shipSystem.keys.Any())
        {
            foreach (var k in shipSystem.keys)
                components.Add(new KeyComponent(k.Key, k.Value));

            actions.component = components.First().component;

            ui.SetUI(shipSystem.keys.Select(p => new LinksGroup() { key = p.Key, link = p.Value, name = p.Value.item.lName }).ToArray());

            UpdateView();
        }
    }

	public void Update () {
        foreach (var c in components.Where(c => Input.GetKeyDown(c.key)))
        {
            ui.UpdateGroup(actions.component, g => g.UnMarkAsSelected());
            actions.component = c.component;
            ui.UpdateGroup(actions.component, g => g.MarkAsSelected());
            UpdateView();
            break;
        }
        if (UInput.key1())
            Act(c => c.Enable());
        if (UInput.key2())
            Act(c => c.Disable());
    }
    void UpdateView()
    {
        //ui.GetComponentInfo().SetData(actions.component.GetInfo());
    }
    void Act(Action<ComponentInput> componentAction)
    {
        componentAction(actions);

        ui.UpdateGroup(actions.component);

        UpdateView();
    }
}

public class KeyComponent
{
    public readonly string key;
    public readonly SystemLink component;
    public KeyComponent(string key, SystemLink component)
    {
        this.key = key;
        this.component = component;
    }
}

public class ComponentInput
{
    public SystemLink component { get; set; }
    public void Enable()
    {
        component.enabled = true;
    }
    public void Disable()
    {
        component.enabled = false;
    }
}