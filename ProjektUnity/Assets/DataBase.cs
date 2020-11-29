using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataBase : MonoBehaviour
{
    public static DataBase DB;

    public Material accentMaterial_On;
    public Material accentMaterial_Off;

    public SceneField mainScene;
    public SceneField controlScene;
    public SceneField loadedLevelScene;

    private void Awake()
    {
        if(DB == null)
        {
            DB = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void LoadLevelScene(SceneField scene)
    {
        if(scene == mainScene || scene == controlScene)
        {
            Debug.Log("NOPE");
            return;
        }

        SceneManager.LoadScene(controlScene.SceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(scene.SceneName, LoadSceneMode.Additive);
    }

    public void ReloadLevel()
    {
        if(loadedLevelScene != null)
        {
            LoadLevelScene(loadedLevelScene);
        }
    }
}
