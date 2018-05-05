using UnityEngine;
using System.Collections;
using Assets.Global;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class LoadingBehaviour : MonoBehaviour {

    [SerializeField]
    Text info;

    static AsyncOperation result;
    static string sceneName;
    static Scene loadingScene;
    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(@"LoadingScene");
        loadingScene = SceneManager.GetActiveScene();

        LoadingBehaviour.sceneName = sceneName;
        result = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
	void Update () {

        if (result.progress == 1f)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
            foreach (var l in scene.GetRootGameObjects().Select(g => g.GetComponent<ILoading>()).Where(l => l != null))
                l.OnLoad();

            enabled = false;
            SceneManager.UnloadScene(@"LoadingScene");
        }
    }
}

public class LoadSceneStage : Stage
{
    AsyncOperation result;
    string sceneName;


    public LoadSceneStage(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public override void Init()
    {
        SceneManager.LoadScene(@"LoadingScene", LoadSceneMode.Additive);
        var loadingScene = SceneManager.GetSceneByName(@"LoadingScene");
        ChangeScene(loadingScene);


        result = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    void ChangeScene(Scene to)
    {
        Scene last = GameState.updater.gameObject.scene;
        SceneManager.MoveGameObjectToScene(GameState.updater.gameObject, to);
        SceneManager.UnloadScene(last);
    }
    public override void Update()
    {
        if(result.progress == 1f)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);
            foreach (var l in scene.GetRootGameObjects().Select(g => g.GetComponent<ILoading>()).Where(l => l != null))
                l.OnLoad();

            ChangeScene(scene);
            GameState.updater.RemoveStage(this);
        }
    }
}