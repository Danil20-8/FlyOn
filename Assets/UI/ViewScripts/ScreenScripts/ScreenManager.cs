using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
public class ScreenManager : MonoBehaviour{

    [SerializeField]
    SlideData[] slides;
    [SerializeField]
    int curr;

    SlideBehaviour current;
    public static ScreenManager currentManager { get; private set; }

    Action transition;
    bool isTransiting = false;
    public readonly Dictionary<string, object> sharedData = new Dictionary<string, object>();

    protected void Awake()
    {
        currentManager = this;

        foreach (var s in slides)
        {
            if (s.isLoaded)
            {
                s.slide.SetSlider(this);
                s.slide.gameObject.SetActive(false);
            }
        }
        if (!slides[curr].isLoaded)
            Load(slides[curr]);
        current = slides[curr].slide;
        current.gameObject.SetActive(true);

        OnStart(current);
    }
    protected virtual void OnStart(SlideBehaviour beginSlide)
    {
    }
    public void MoveTo(string label, params object[] args)
    {
        foreach (var s in slides)
            if (s.slide.label == label)
            {
                if (!s.isLoaded)
                    Load(s);

                s.slide.gameObject.SetActive(true);
                s.slide.OnSlide();

                var t = Transition(current, s.slide);
                transition = () =>
                {
                    if (t())
                    {
                        current.OnOut();
                        current.gameObject.SetActive(false);
                        current = s.slide;
                        isTransiting = false;
                    }
                    else if(!isTransiting)
                        isTransiting = true;
                };
                transition();
                return;
            }
        throw new System.Exception(string.Format("Slide {0}'s not found", label));
    }
    void Load(SlideData data)
    {
        if (data.loadTransform != null)
            data.slide = (SlideBehaviour) Instantiate(data.slide, data.loadTransform.position, data.loadTransform.rotation);
        else
            data.slide = Instantiate(data.slide);
    }
    protected virtual Func<bool> Transition(SlideBehaviour from, SlideBehaviour to)
    {
        return () => true;
    }
    protected void EndTransition()
    {

        isTransiting = false;
    }

    GameConsole console;
    protected void Update()
    {
        if(isTransiting)
            transition();

        if(Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (console != null)
            {
                Destroy(console.gameObject);
                console = null;
                return;
            }
            console = Instantiate(Resources.Load<GameConsole>(@"Views\GameConsoleView"));
            console.WaitingForCommand(CommandHandler);
        }
    }
    string CommandHandler(string command, string[] args)
    {
        foreach(var m in current.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if(m.GetCustomAttributes(typeof(GameConsoleCommandAttribute), true).Length > 0)
            {
                if (m.Name == command)
                {
                    if(m.ReturnType == typeof(string))
                        return (string)m.Invoke(current, args);

                    m.Invoke(current, args);
                    return "OK";
                }
            }
        }
        return "Unknown command";
    }

    public static T ShowMessage<T>(T asset) where T : MonoBehaviour
    {
        var m = (T) Instantiate(asset, currentManager.current.messageTransform.position, currentManager.current.messageTransform.rotation);
        m.transform.SetParent(currentManager.current.messageTransform);
        return m;
    }

    public static T GetScreen<T>() where T : SlideBehaviour
    {
        if(currentManager != null)
        {
            foreach (var s in currentManager.slides)
            {
                if (s.slide is T)
                {
                    if (!s.isLoaded)
                        break;
                    return (T)s.slide;
                }
            }
        }
        throw new System.Exception("Slide " + typeof(T) + " is not found");
    }
}

[System.Serializable]
public class SlideData
{
    public bool isLoaded = true;
    public SlideBehaviour slide;
    public Transform loadTransform = null;
}

public class GameConsoleCommandAttribute : Attribute
{
}