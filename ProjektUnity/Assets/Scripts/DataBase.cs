using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataBase : MonoBehaviour
{
    public static DataBase DB;

    [Header("Dane leveli")]
    public List<Level_Scriptable> levelsData = new List<Level_Scriptable>();
    public List<LevelInfo> levels = new List<LevelInfo>();
    public SceneField mainScene;
    public SceneField controlScene;
    public SceneField loadedLevelScene;

    private LevelInfo currentLevel;

    [Header("Referencje graficzne")]
    public Material accentMaterial_On;
    public Material accentMaterial_Off;

    [System.Serializable]
    public enum ShoutType
    {
        None = 0,
        WinTitle = 1,
        WinContent = 2,
        FailTitle = 3,
        FailContent = 4,
    }
    [Header("Shoutsy")]
    public List<string> winTitle = new List<string>();
    public List<string> winContent = new List<string>();
    public List<string> failTitle = new List<string>();
    public List<string> failContent = new List<string>();



    private void Awake()
    {
        if(DB == null)
        {
            DB = this;
            DontDestroyOnLoad(this.gameObject);
            LoadLevelsData();
        }
        else
        {
            Destroy(this.gameObject);
        }

        
    }


    private void OnLevelWasLoaded(int level)
    {
        if (DB == this)
        {
            UpdateDataFromPlayerPrefs();
        }
    }


    private string GetRandomElementFromListAndShuffle(List<string> list)
    {
        if(list.Count == 0)
        {
            return "";
        }
        if(list.Count == 1)
        {
            return list[0];
        }

        string s = list[Random.RandomRange(0, list.Count - 1)];
        list.Remove(s);
        list.Add(s);
        return s;
    }


    public string GetShout_Succes(ShoutType st)
    {
        switch (st)
        {
            case ShoutType.WinTitle:
                {
                    return GetRandomElementFromListAndShuffle(winTitle);
                }
            case ShoutType.WinContent:
                {
                    return GetRandomElementFromListAndShuffle(winContent);
                }
            case ShoutType.FailTitle:
                {
                    return GetRandomElementFromListAndShuffle(failTitle);
                }
            case ShoutType.FailContent:
                {
                    return GetRandomElementFromListAndShuffle(failContent);
                }
        }

        return "";
    }


    public List<LevelInfo> GetLevelsData()
    {
        if (levels.Count == 0)
        {
            levels.Clear();
            foreach (Level_Scriptable ls in levelsData)
            {
                levels.Add(new LevelInfo(ls.levelInfo));
            }
        }
        return levels;
    }


    private void LoadLevelsData()
    {
        GetLevelsData();
        UpdateDataFromPlayerPrefs();
    }

    private void UpdateDataFromPlayerPrefs()
    {
        foreach (LevelInfo level in levels)
        {
            string key = level.levelID.GetLevelID();
            if (PlayerPrefs.HasKey(key))
            {
                string levelData = PlayerPrefs.GetString(key);
                LevelRecord lr = JsonUtility.FromJson<LevelRecord>(levelData);
                if (lr != null)
                {
                    level.levelRecord = lr;
                }
            }
        }
    }

    // zapisuje dane o "przejsciu" levelu tylko jeżeli obecne przejście jest "lepsze" od poprzedniego
    public void SaveLevelsData(LevelInfo level, LevelRecord levelRecord)
    {
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

        if (oldRecord != null)
        {
            if (oldRecord.shootsUsed < levelRecord.shootsUsed)
            {
                string jsonData = JsonUtility.ToJson(levelRecord);
                PlayerPrefs.SetString(key, jsonData);
                PlayerPrefs.Save();
            }
        }
        else
        {
            string jsonData = JsonUtility.ToJson(levelRecord);
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

    }


    public void LoadLevelScene(LevelInfo level)
    {
        if(level.scene == mainScene || level.scene == controlScene)
        {
            Debug.Log("NOPE");
            return;
        }

        currentLevel = level;

        SceneManager.LoadScene(controlScene.SceneName, LoadSceneMode.Single);
        SceneManager.LoadScene(level.scene.SceneName, LoadSceneMode.Additive);
    }


    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainScene.SceneName, LoadSceneMode.Single);
    }


    public void LoadNextLevel()
    {
        if(currentLevel != null)
        {
            LevelInfo li = levels[levels.IndexOf(currentLevel) + 1];
            LoadLevelScene(li);
        }
    }


    public void ReloadLevel()
    {
        if(currentLevel != null)
        {
            LoadLevelScene(currentLevel);
        }
    }


    public bool IsCurrentLevelLastOne()
    {
        if (currentLevel != null)
        {
            if (currentLevel == levels.Last())
            {
                return true;
            }
        }
        return false;
    }


    public LevelInfo GetCurrentLevel()
    {
        return currentLevel;
    }
}


[System.Serializable]
public class LevelInfo
{
    public LevelInfo(LevelInfo copyFrom)
    {
        this.name = copyFrom.name;
        this.levelID = copyFrom.levelID;
        this.levelIcon = copyFrom.levelIcon;
        this.scene = copyFrom.scene;
        this.music = copyFrom.music;
        this.levelIntro = copyFrom.levelIntro;

        this.levelRecord = new LevelRecord();
    }

    public string name;
    public LevelID levelID;
    public LevelRecord levelRecord;
    public Sprite levelIcon;
    public SceneField scene;
    public AudioClip music;
    public LevelIntro levelIntro;
}

[System.Serializable]
public class LevelIntro
{
    public bool skip;
    public string title;
    [TextArea] public string content;
    public string songTitle;
    public string okButtonText;
    public Sprite image;
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

    public LevelRecord()
    {

    }

    public LevelRecord(bool completed, int shootsUsed, string dateOfCompletion)
    {
        this.completed = completed;
        this.shootsUsed = shootsUsed;
        this.dateOfCompletion = dateOfCompletion;
    }

    public bool completed = false;
    public int shootsUsed = 4;
    public string dateOfCompletion = "Never";
}