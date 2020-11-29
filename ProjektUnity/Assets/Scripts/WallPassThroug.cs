using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class WallPassThroug : Wall
{

    private void Start()
    {
        
    }

    public override TrajectoryPoint GetTrajectoryPoint(RaycastHit hit, Vector3 currentDirection)
    {
        HiglightByAim();
        return new TrajectoryPoint(hit.point, currentDirection, this, true);
    }


}
