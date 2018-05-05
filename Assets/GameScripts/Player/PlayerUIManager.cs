using UnityEngine;
using System.Collections;

public class PlayerUIManager : MonoBehaviour {

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    RadarBehaviour radar;
    [SerializeField]
    AimBehaviour aim;

    [SerializeField]
    BattleMenuBehaviour menu;

    void Awake()
    {
        enabled = false;
    }

    public void Update()
    {
        radar.Update();
    }

    public RadarBehaviour GetRadar()
    {
        return radar;
    }
    public AimBehaviour GetAim()
    {
        return aim;
    }
    public void SetAimPosition(Vector2 position)
    {
        var rect = canvas.GetComponent<RectTransform>();
        aim.GetComponent<RectTransform>().localPosition = new Vector2(-rect.rect.width / 2 + rect.rect.width * position.x, -rect.rect.height / 2 + rect.rect.height * position.y);
    }

    public void ShowHide()
    {
        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
    }
}
