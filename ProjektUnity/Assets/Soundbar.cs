using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Soundbar : MonoBehaviour, IObjective
{
    private Wall wall;
    private List<Outline> outlines = new List<Outline>();

    private float currentOutline = 0f;
    private float maxOutline = 4f;
    private float targetOutline = 0f;

    public bool isPlaying;

    private void Start()
    {
        outlines = GetComponentsInChildren<Outline>().ToList();
        wall = GetComponentInChildren<Wall>();
        if (wall)
        {
            wall.RegisterToOnGotHitted(PlayMusic);
        }
        else
        {
            Debug.Log("No wall under soundbar?");
        }


    }


    public bool IsCompleted()
    {
        return isPlaying;
    }


    private void PlayMusic()
    {
        //audioSource.Play();
        isPlaying = true;
    }

    private void Update()
    {
        HandleVisual();
    }

    private void HandleVisual()
    {
        if (wall && wall.IsHiglighted())
        {
            Higlight();
        }
        else
        {
            Unhiglight();
        }
     

        currentOutline = isPlaying ? 0f : Mathf.MoveTowards(currentOutline, targetOutline, 15f * Time.deltaTime);
        outlines.ForEach(x => x.OutlineWidth = currentOutline);
    }


    private void Higlight()
    {
        targetOutline = maxOutline;
    }


    private void Unhiglight()
    {
        targetOutline = 0f;
    }
}
