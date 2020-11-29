using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelHUD levelHud;
    [SerializeField] private PlayerControler player;
    private List<IObjective> objectives = new List<IObjective>();
    private float normalsiedLevelProgres;
    private Coroutine levelCompletedCor;
   
    

    private void Start()
    {
        FillObjectives();
        PrepareLevel();
        DisplaySomeBiedaIntro();
    }


    private void Update()
    {
        CalculateLevelProgress();

        if (player.shootsThisLevel >= 4)
        {
            LevelFailed();
        }

        if (IsLevelCompleted())
        {
            LevelCompleted();
        }
    }


    private void DisplaySomeBiedaIntro()
    {
        LevelInfo level = DataBase.DB.GetCurrentLevel();

        if (level.levelIntro.skip == true)
        {

        }
        else
        {
            player.BlockPlayerInput();
            levelHud.DisplayLevelIntro(level, () => player.UnblockPlayerInput());
        }
      
    }


    private void FillObjectives() // rozwiązanie na szybko z braku czasu. Wydajniejsze byłoby nawet przypisanie referencji ręcznie w edytorze
    {
        Transform[] t = FindObjectsOfType<Transform>();
        foreach (Transform item in t)
        {
            IObjective io = item.GetComponent<IObjective>();
            if (io != null)
            {
                objectives.Add(io);
            }
        }
    }


    public int GetShootsCount()
    {
        return player.shootsThisLevel;
    }


    private void LevelCompleted()
    {
        if(levelCompletedCor == null)
        {
            levelCompletedCor = StartCoroutine(EndLevelProcces(3f, true));
        }

        player.BlockPlayerInput();
    }


    private void LevelFailed()
    {
        if (levelCompletedCor == null)
        {
            levelCompletedCor = StartCoroutine(EndLevelProcces(3f, false));
        }

        player.BlockPlayerInput();
    }


    private IEnumerator EndLevelProcces(float delayTime, bool succes)
    {
        float timer = 0f;


        while(timer < delayTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if(succes)
        {
            string title = DataBase.DB.GetShout_Succes(DataBase.ShoutType.WinTitle);
            string content = DataBase.DB.GetShout_Succes(DataBase.ShoutType.WinContent);

            LevelInfo currentLEvel = DataBase.DB.GetCurrentLevel();
            if(currentLEvel != null)
            {
                LevelRecord lr = new LevelRecord(true, GetShootsCount(), System.DateTime.UtcNow.ToShortDateString());
                DataBase.DB.SaveLevelsData(currentLEvel, lr);
            }

            levelHud.simplePrompter.DisplayPrompter(title, content, () => DataBase.DB.LoadNextLevel(), "Next level", () => DataBase.DB.LoadMainMenu(), "Back to menu");
        }
        else
        {
            string title = DataBase.DB.GetShout_Succes(DataBase.ShoutType.FailTitle);
            string content = DataBase.DB.GetShout_Succes(DataBase.ShoutType.FailContent);

            levelHud.simplePrompter.DisplayPrompter(title, content, () => DataBase.DB.ReloadLevel(), "Try again", () => DataBase.DB.LoadMainMenu(), "Back to menu");
        }

        yield return null;
    }


    public float GetLevelProgressionNormalized()
    {
        return normalsiedLevelProgres;
    }


    private void PrepareLevel()
    {
        PlayerControler pc = FindObjectOfType<PlayerControler>();
        List<SceneMarker> markers = FindObjectsOfType<SceneMarker>().ToList();

        SceneMarker playerPlace = markers.FirstOrDefault(x=> x.type == MarkerType.PLAYER_START);
        if(pc != null && playerPlace != null)
        {
            pc.transform.position = playerPlace.transform.position;
            pc.transform.rotation = playerPlace.transform.rotation;
        }

        Camera cam = FindObjectOfType<Camera>();
        SceneMarker cameraPlace = markers.FirstOrDefault(x=> x.type == MarkerType.CAMERA);
        if (cam != null && cameraPlace != null)
        {
            cam.transform.position = cameraPlace.transform.position;
            cam.transform.rotation = cameraPlace.transform.rotation;
        }
    }


    private void CalculateLevelProgress()
    {
        if(IsLevelCompleted())
        {
            normalsiedLevelProgres = 1f;
        }

        float part = 1f / objectives.Count;

        normalsiedLevelProgres = 0f;

        foreach (IObjective objective in objectives)
        {
            if(objective.IsCompleted())
            {
                normalsiedLevelProgres += part;
            }
        }

        normalsiedLevelProgres = Mathf.Clamp(normalsiedLevelProgres, 0f, 1f);
    }


    public bool IsLevelCompleted()
    {
        return objectives.TrueForAll(x => x.IsCompleted());
    }
}


public interface IObjective
{
    bool IsCompleted();
}