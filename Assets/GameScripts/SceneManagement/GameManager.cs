using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
    [SerializeField]
    string loadingScene;
    [SerializeField]
    string[] scenes;

    [SerializeField]
    int current;

    public Camera currentCamera { get; private set; }
    Queue<GameObject> cameras = new Queue<GameObject>();

    Stack<int> pin = new Stack<int>();
    public void PinCurrent()
    {

        pin.Push(current);
    }

    public void SetCamera(GameObject camera)
    {
        cameras.Enqueue(camera);

        if (cameras.Count == 1)
        {
            camera.gameObject.SetActive(true);

            currentCamera = camera.GetComponentInChildren<Camera>();
        }
    }
    public void ReleaseCamera(GameObject camera)
    {
        if (cameras.Peek() == camera)
        {
            try {
                cameras.Dequeue().SetActive(false);
            }
            catch { }
            if (cameras.Count >= 1)
            {
                cameras.Peek().SetActive(true);

                currentCamera = camera.GetComponentInChildren<Camera>();
            }
        }
        else
            throw new Exception(camera + " is not current");
    }
    public static GameManager instance { get; private set; }

    void Start()
    {
        instance = this;
        SetScene(current);
    }

    public ISceneInitializer GetInitializer()
    {
        var scene = SceneManager.GetSceneByName(scenes[current]);

        SceneManager.SetActiveScene(scene);

        return scene
            .GetRootGameObjects()
            .Select(go => go.GetComponent<ISceneInitializer>())
            .FirstOrDefault(i => i != null);
    }
    public void GoToPin()
    {
        GoTo(pin.Pop());
    }
    public void GoTo(int scene)
    {
        StartCoroutine(CoroutineGoTo(scene));
    }
    IEnumerator CoroutineGoTo(int scene)
    {
        SceneManager.UnloadScene(scenes[current]);
        yield return null;

        SetScene(scene);
    }
    public void GoTo(string name)
    {
        for(int i = 0; i < scenes.Length; i++)
            if(scenes[i] == name)
            {
                GoTo(i);
                return;
            }
    }
    void SetScene(int scene)
    {
        StartCoroutine(CoroutineSetScene(scene));
    }
    IEnumerator CoroutineSetScene(int scene)
    {
        SceneManager.LoadScene(loadingScene, LoadSceneMode.Additive);
        yield return null;
        var ls = SceneManager.GetSceneByName(loadingScene)
            .GetRootGameObjects().Select(go => go.GetComponent<LoadingScreenBehaviour>()).First(b => b != null);

        var result = SceneManager.LoadSceneAsync(scenes[scene], LoadSceneMode.Additive);
        current = scene;

        StartCoroutine(Loading(result, ls));
        //ls.SetLoadingScene(result, Done);
    }

    IEnumerator Loading(AsyncOperation progress, LoadingScreenBehaviour ls)
    {
        ls.Begin();
        LoadingState state = ls.state;

        while(!progress.isDone)
        {
            state.Callback("LoadingScene", progress.progress);
            yield return null;
        }

        List<Action> doneListeners = new List<Action>();

        var initializer = GetInitializer();
        if(initializer != null)
        {

            state.Callback("InitializingScene", 0);
            yield return null;

            AsyncProcessor processor = new AsyncProcessor(this);
            yield return initializer.Initialize(processor, doneListeners.Add, state);

            yield return new WaitUntil(processor.allDone);
        }

        ls.End();
        Done();
        foreach (var dl in doneListeners)
            dl.Invoke();
    }
    void Done()
    {
        SceneManager.UnloadScene(loadingScene);
    }
}

public delegate void AddListener(Action listener);

public interface ISceneInitializer
{
    IEnumerator Initialize(AsyncProcessor processor, AddListener addDoneListener, LoadingState state);
}


