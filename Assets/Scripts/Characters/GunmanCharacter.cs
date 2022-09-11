using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunmanCharacter : AbstractCharacter
{
    AreaOfInterest currentDestination;

    float DeadZone = 3f;

    Timer wanderTimer = new Timer(3);
    Timer detectionTimer = new Timer(1);

    bool firstShot = false;

    private void Update()
    {
        if (currentDestination == null && AreaOfInterest.areasOfInterest.Count > 0 && wanderTimer.elapseAsPercent >= 1)
        {
            currentDestination = AreaOfInterest.areasOfInterest[Random.Range(0, AreaOfInterest.areasOfInterest.Count)];
            locomotor.MoveTowardsSpot(currentDestination.transform.position);
        }
        else if (currentDestination != null && Vector3.Distance(transform.position, currentDestination.transform.position) < DeadZone)
        {
            wanderTimer.Reset();
            wanderTimer.maxTime = Random.Range(0f, 1f);

            currentDestination = null;
        }
    }

    public override void ReactToCharacter(AbstractCharacter whoReactTo)
    {
        Attack(whoReactTo);
        if (!firstShot)
        {
            StartCoroutine(DetectionDelay());
        }
    }

    private IEnumerator DetectionDelay()
    {
        firstShot = true;
        detectionTimer.Reset();

        while(!detectionTimer.isComplete)
        {
            yield return null;
        }

        HostileIsKnown = true;
    }
}
