using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerControler player;

    private List<IObjective> objectives = new List<IObjective>();
   
    public bool debug_showIsLevelCompleted;
    private float normalsiedLevelProgres;


    private Coroutine levelCompletedCor;


    private void Start()
    {
        FillObjectives();
        PrepareLevel();
    }


    private void Update()
    {
        debug_showIsLevelCompleted = IsLevelCompleted();
        CalculateLevelProgress();
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


    private void LevelCompleted()
    {
        if(levelCompletedCor == null)
        {
            levelCompletedCor = StartCoroutine(EndLevelProcces(3f));
        }

        player.BlockPlayerInput();
    }


    private IEnumerator EndLevelProcces(float delayTime)
    {
        float timer = 0f;


        while(timer < delayTime)
        {
            timer += Time.deltaTime;


            yield return null;
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