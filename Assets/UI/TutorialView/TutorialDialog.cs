using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialDialog : MonoBehaviour {

    [SerializeField]
    Text text;

    Func<bool> isCompleted;
    Action completedAction;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void SetTutorial(string text, Func<bool> isCompleted, Action completedAction)
    {
        this.isCompleted = isCompleted;
        this.text.text = text;
        this.completedAction = completedAction;

        gameObject.SetActive(true);
    }
	
	void Update () {

        if (isCompleted())
        {
            gameObject.SetActive(false);
            completedAction();
        }
	}
}
