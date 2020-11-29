using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WallBounce : Wall
{
    private void Start()
    {
        
    }

    public override TrajectoryPoint GetTrajectoryPoint(RaycastHit hit, Vector3 currentDirection)
    {
        HiglightByAim();
        Vector3 incomingVec = currentDirection;
        Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
        currentDirection = reflectVec;
        return new TrajectoryPoint(hit.point, currentDirection, this, true);
    }

    
}
