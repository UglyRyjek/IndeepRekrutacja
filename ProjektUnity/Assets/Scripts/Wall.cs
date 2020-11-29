using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wall : MonoBehaviour
{
    protected bool isHiglighted;
    protected Action onGotHitted;


    public void GotHitted()
    {
        onGotHitted?.Invoke();
    }


    public bool IsHiglighted()
    {
        return isHiglighted;
    }


    public void RegisterToOnGotHitted(Action OnGotHitted)
    {
        this.onGotHitted += OnGotHitted;
    }


    protected void HiglightByAim()
    {
        Higlight();
        CancelInvoke(nameof(Unhiglight));
        Invoke(nameof(Unhiglight), 0.05f);
    }


    protected void Higlight()
    {
        isHiglighted = true;
    }


    protected void Unhiglight()
    {
        isHiglighted = false;
    }


    public virtual TrajectoryPoint GetTrajectoryPoint(RaycastHit hit, Vector3 currentDirection)
    {
        HiglightByAim();
        return new TrajectoryPoint(Vector3.zero, Vector3.zero, null, false);
    }


   
}
