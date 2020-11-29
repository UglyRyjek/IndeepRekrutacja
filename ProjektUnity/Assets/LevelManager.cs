using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
   
    private List<IObjective> objectives = new List<IObjective>();
   
    public bool debug_showIsLevelCompleted;


    private void Start()
    {
        FillObjectives();
        PrepareLevel();
    }


    private void Update()
    {
        debug_showIsLevelCompleted = IsLevelCompleted();
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

    
    public bool IsLevelCompleted()
    {
        return objectives.TrueForAll(x => x.IsCompleted());
    }


}


public interface IObjective
{
    bool IsCompleted();
}