using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPassOnlyOnce : Wall
{
    public int allowThisManyPassingThrough = 1;
    private int counter = 0;

    private void Start()
    {
        counter = 0;
    }

    private void Update()
    {
        counter = 0;
    }

    public override TrajectoryPoint GetTrajectoryPoint(RaycastHit hit, Vector3 currentDirection)
    {
        if (counter < allowThisManyPassingThrough)
        {
            HiglightByAim();
            counter++;
            return new TrajectoryPoint(hit.point, currentDirection, this, true);

        }
        else
        {
            HiglightByAim();
            return new TrajectoryPoint(hit.point, currentDirection, this, false);
        }




    }

}
