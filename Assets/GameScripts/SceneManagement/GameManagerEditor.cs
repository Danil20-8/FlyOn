#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;


public partial class GameManager : MonoBehaviour
{
#if UNITY_EDITOR
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameManager gm = (GameManager)target;

            var loadingScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(GetScenePath(gm.loadingScene));
            loadingScene = (SceneAsset) EditorGUILayout.ObjectField("Loading Scene", loadingScene, typeof(SceneAsset));


            List<SceneAsset> scenes = gm.scenes.Select(s => AssetDatabase.LoadAssetAtPath<SceneAsset>(GetScenePath(s))).Where(s => s != null).ToList();
            if (gm.current >= scenes.Count) gm.current = scenes.Count - 1;
            SceneAsset current = gm.current != -1 ? scenes[gm.current] : null;

            current = (SceneAsset) EditorGUILayout.ObjectField("Start Scene", current, typeof(SceneAsset));
            for(int i = 0; i < scenes.Count; i++)
            {
                scenes[i] = (SceneAsset) EditorGUILayout.ObjectField("Scene " + i.ToString(), scenes[i], typeof(SceneAsset));
            }

            var newScene = (SceneAsset) EditorGUILayout.ObjectField("Add Scene", null, typeof(SceneAsset));

            if (loadingScene != null)
            {
                serializedObject.FindProperty("loadingScene").stringValue = loadingScene.name;
            }

            if (newScene != null)
                scenes.Add(newScene); 

            if (scenes != null)
            {
                if (current != null)
                {
                    serializedObject.FindProperty("current").intValue = scenes.IndexOf(current);
                }
                var sArray = serializedObject.FindProperty("scenes");
                sArray.arraySize = scenes.Count;
                int i = 0;
                foreach (var s in scenes.Select(s => s.name))
                {
                    sArray.GetArrayElementAtIndex(i).stringValue = s;
                    i++;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        string GetScenePath(string sceneName)
        {
            return @"Assets\Levels\" + sceneName + ".unity";
        }
        string GetSceneName(string scenePath)
        {
            return scenePath.Split('/').Last().Split('.')[0];
            
        }
    }
#endif
}
