using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianCharacter : AbstractCharacter
{
    AreaOfInterest currentDestination;

    float DeadZone = 1f;

    Timer wanderTimer = new Timer(3);

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
            wanderTimer.maxTime = Random.Range(1f, 10f);

            currentDestination = null;
        }
    }


}
