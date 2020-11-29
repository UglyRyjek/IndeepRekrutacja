using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDancer : Wall
{
    private Dancer dancer;

    private void Start()
    {
        dancer = GetComponentInParent<Dancer>();
    }


    public override TrajectoryPoint GetTrajectoryPoint(RaycastHit hit, Vector3 currentDirection)
    {
        HiglightByAim();
        return new TrajectoryPoint(hit.point, currentDirection, this, true);
    }

    
}

