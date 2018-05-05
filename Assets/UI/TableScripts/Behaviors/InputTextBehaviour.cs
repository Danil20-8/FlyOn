using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
public class InputTextBehaviour : Selectable {

    [SerializeField]
    Text text;

    bool isSelected;


	void Update()
    {
        if (!isSelected)
            return;

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            StartCoroutine(RemoveCoroutine());
        }
        else
        {
            var str = Input.inputString;
            if (str.Length != 0)
            {
                if (str[0] != 8)
                    text.text += str;
            }
        }

    }


    IEnumerator RemoveCoroutine()
    {
        if(Input.GetKey(KeyCode.Backspace) && text.text.Length > 0)
            text.text = text.text.Substring(0, text.text.Length - 1);

        yield return new WaitForSecondsRealtime(.3f);

        while (Input.GetKey(KeyCode.Backspace) && text.text.Length > 0)
        {
            text.text = text.text.Substring(0, text.text.Length - 1);
            yield return new WaitForSecondsRealtime(.027f);
        }
    }

    public override void OnMove(AxisEventData eventData)
    {
    }

    public override void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }
}
