using UnityEngine;
using System.Collections;

public class ToolSelectorBehaviour : MonoBehaviour {

    [SerializeField]
    GameObject inventory;

    [SerializeField]
    GameObject keySelector;

    [SerializeField]
    GameObject current;

    void Start()
    {
        inventory.SetActive(false);
        keySelector.SetActive(false);

        current.SetActive(true);
    }

    public void ShowInventory()
    {
        Show(inventory);
    }

    public void ShowKeySelector()
    {
        Show(keySelector);
    }

    void Show(GameObject obj)
    {
        current.SetActive(false);
        current = obj;
        current.SetActive(true);
    }
}
