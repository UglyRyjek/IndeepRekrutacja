using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataBase : MonoBehaviour
{
    public static DataBase DB;


    [Header("Dane leveli")]
    public List<LevelInfo> levels = new List<LevelInfo>();
    public SceneField mainScene;
    public SceneField controlScene;
    public SceneField loadedLevelScene;

    [Header("Referencje graficzne")]
    public Material accentMaterial_On;
    public Material accentMaterial_Off;


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

        LoadLevelsData();
    }



    private void LoadLevelsData()
    {
        foreach (LevelInfo level in levels)
        {
            string key = level.levelID.GetLevelID();
            if(PlayerPrefs.HasKey(key))
            {
                string levelData = PlayerPrefs.GetString(key);
                LevelRecord lr = JsonUtility.FromJson<LevelRecord>(levelData);
                if(lr != null)
                {
                    level.levelRecord = lr;
                }
            }
        }
    }


    // zapisuje dane o "przejsciu" levelu tylko jeżeli obecne przejście jest "lepsze" od poprzedniego
    public void SaveLevelsData(LevelInfo level, LevelRecord levelRecord)
    {
        if(levelRecord.completed == false)
        {
            return;
        }

        string key = level.levelID.GetLevelID();

        LevelRecord oldRecord = null;
        if (PlayerPrefs.HasKey(key))
        {
            string levelData = PlayerPrefs.GetString(key);
            LevelRecord lr = JsonUtility.FromJson<LevelRecord>(levelData);
            if (lr != null)
            {
                oldRecord = lr;
            }
        }

        if(oldRecord != null)
        {
            if(oldRecord.shootsUsed > levelRecord.shootsUsed)
            {
                string jsonData = JsonUtility.ToJson(levelRecord);
                PlayerPrefs.SetString(key, jsonData);
                PlayerPrefs.Save();
            }
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


[System.Serializable]
public class LevelInfo
{
    public string name;
    public LevelID levelID;
    public LevelRecord levelRecord;
    public Sprite levelIcon;
    public SceneField scene;
}

// temp solution dla zachowywania danych przez player prefs
[System.Serializable]
public struct LevelID
{
    [SerializeField] private int levelID;

    public string GetLevelID()
    {
        return levelID.ToString();
    }
}

[System.Serializable]
public class LevelRecord
{
    public bool completed = false;
    public int shootsUsed = 4;
    public string dateOfCompletion = "Never";
}