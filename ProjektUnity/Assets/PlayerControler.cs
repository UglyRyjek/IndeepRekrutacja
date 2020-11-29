using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PlayerControler : MonoBehaviour
{
    public PlayerState playerState;

    [SerializeField] private InputBase input;

    [SerializeField] private Transform playerPivot;

    [SerializeField] private Transform gunPoint;

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private float maxRaycastDistance = 10f;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float bounceAngle = -90f;
    [SerializeField] private int maxReflections = 50;
    [SerializeField] private Transform bullet;
    [SerializeField] private float bulletSpeed;
    private float dragTimer = 0f;
    private Vector3 nulledVector = new Vector3(0f, -10f, 0f);

    public int shootsThisLevel = 0;
    public bool isWorking;
    private Coroutine shootCor = null;

    private List<TrajectoryPoint> trajectory = new List<TrajectoryPoint>();



    private void Start()
    {
        playerState = PlayerState.None;
    }


    private void Update()
    {
        if(playerState == PlayerState.InputBlocked)
        {
            HideGunAiming();
            return;
        }

        if (shootCor == null)
        {
            input.UpdateMe(playerPivot, Shoot);

            AimTheGun();
            playerState = PlayerState.Aiming;
        }
        else
        {
            HideGunAiming();
            playerState = PlayerState.ShootInProgress;
        }
    }


    public void BlockPlayerInput()
    {
        playerState = PlayerState.InputBlocked;
    }

    public void UnblockPlayerInput()
    {
        playerState = PlayerState.None;
    }

    private void Shoot()
    {
        List<TrajectoryPoint> shotTrajectory = new List<TrajectoryPoint>();
        trajectory.ForEach(x => shotTrajectory.Add(x));

        shootsThisLevel++;
        if (shotTrajectory.Count >= 2)
        {
            if (shootCor == null)
            {
                shootCor = StartCoroutine(ShootProcess(shotTrajectory, bullet));
            }
        }
        else
        {
            Debug.Log("Nie pownno się nigdy zdarzyć");
        }

    }


    private IEnumerator ShootProcess(List<TrajectoryPoint> bulletTrajectory, Transform bulletPrefab)
    {
        bulletPrefab.gameObject.SetActive(false);
        Transform bullet = GameObject.Instantiate(bulletPrefab.gameObject, bulletPrefab.parent).gameObject.transform;
        bullet.gameObject.SetActive(true);


        List<TrajectoryPoint> bt = bulletTrajectory;
        TrajectoryPoint from = bt[0];
        TrajectoryPoint to = bt[1];
        TrajectoryPoint end = bt.Last();
        float safetyTime = 50f;

        bullet.position = from.position;

        while (bullet.position != end.position && safetyTime >= 0f)
        {
            safetyTime -= Time.deltaTime;

            bullet.position = Vector3.MoveTowards(bullet.position, to.position, bulletSpeed * Time.deltaTime);
            if (bullet.position == to.position)
            {
                if (to.wall != null)
                {
                    to.wall?.GotHitted();
                }

                if (to.position == end.position)
                {
                    // skip and go to end
                }
                else
                {
                    int fromIndex = bt.IndexOf(from);
                    from = bt[fromIndex + 1];
                    to = bt[fromIndex + 2];
                }
            }

            yield return null;
        }


        Destroy(bullet.gameObject);
        shootCor = null;
        yield return null;

    }


    private void HideGunAiming()
    {
        lineRenderer.positionCount = 0;
    }


    private void AimTheGun()
    {
        trajectory = CalculateTrajectory(gunPoint.position, gunPoint.forward);
        lineRenderer.positionCount = trajectory.Count;

        List<Vector3> trajectoryPath = new List<Vector3>();
        trajectory.ForEach(x => trajectoryPath.Add(x.position));
        lineRenderer.SetPositions(trajectoryPath.ToArray());
    }


    private List<TrajectoryPoint> CalculateTrajectory(Vector3 startPoint, Vector3 startForward)
    {
        int reflectionCount = 0;
        List<TrajectoryPoint> trajectory = new List<TrajectoryPoint>();

        Vector3 aimPoint = startPoint;
        Vector3 aimDirection = startForward;

        TrajectoryPoint firstPoint = new TrajectoryPoint(aimPoint, aimDirection, null, true);
        trajectory.Add(firstPoint);
        TrajectoryPoint trackedPoint = firstPoint;

        Wall lastWall = null;

        while (trackedPoint.continueTracking && reflectionCount <= maxReflections)
        {
            reflectionCount++;
            bool hitSomeWall = false;
            Vector3 pointOfhit = trackedPoint.position + (trackedPoint.direction.normalized * maxRaycastDistance);
            RaycastHit[] hits = Physics.RaycastAll(trackedPoint.position, trackedPoint.direction, maxRaycastDistance, raycastLayerMask, QueryTriggerInteraction.Ignore);

            foreach (RaycastHit hit in hits)
            {
                Wall w = hit.transform.GetComponent<Wall>();
                if (w != null)
                {
                    if (w == lastWall)
                    {
                        continue;
                    }
                    lastWall = w;

                    pointOfhit = hit.point;
                    aimPoint = hit.point;

                    TrajectoryPoint nextPoint = w.GetTrajectoryPoint(hit, aimDirection);
                    trajectory.Add(nextPoint);
                    trackedPoint = nextPoint;
                    hitSomeWall = true;

                    break;
                }
            }

            if (hitSomeWall == false)
            {
                TrajectoryPoint lastPoint = new TrajectoryPoint(pointOfhit, trackedPoint.direction, null, false);
                trajectory.Add(lastPoint);
                trackedPoint = lastPoint;
            }
        }



        return trajectory;
    }
}


[System.Serializable]
public enum PlayerState
{
    None = 0,
    Aiming = 1,
    ShootInProgress = 2,
    InputBlocked = 3,
}


[System.Serializable]
public struct TrajectoryPoint
{
    public TrajectoryPoint(Vector3 position, Vector3 direction, Wall wall, bool continueTracking)
    {
        this.position = position;
        this.direction = direction;
        this.wall = wall;
        this.continueTracking = continueTracking;
    }

    public Vector3 position;
    public Vector3 direction;
    public Wall wall;
    public bool continueTracking;
}