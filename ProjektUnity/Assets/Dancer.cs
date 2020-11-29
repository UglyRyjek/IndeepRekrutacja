using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class Dancer : MonoBehaviour, IObjective
{
    private List<Outline> outlines = new List<Outline>();
    [SerializeField] private Animator _animator;
    private Wall wall;

    private float currentOutline = 0f;
    private float maxOutline = 4f;
    private float targetOutline = 0f;

    private bool isDancing;

    public Animator Animator
    {
        get
        {
            if(_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }


    private void Start()
    {

        Unhiglight();
        outlines = GetComponentsInChildren<Outline>().ToList();
        wall = GetComponentInChildren<Wall>();
        if(wall)
        {
            wall.RegisterToOnGotHitted(Dance);
        }
        else
        {
            Debug.Log("No wall under Dancer?");
        }
    }


    private void Update()
    {
        HandleVisual();
    }


    public bool IsCompleted()
    {
        return isDancing;
    }



    public void Dance()
    {
        isDancing = true;
        Animator.SetTrigger("Dance");
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

        currentOutline = isDancing ? 0f : Mathf.MoveTowards(currentOutline, targetOutline, 15f * Time.deltaTime);
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
