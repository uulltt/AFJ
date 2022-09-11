using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianCharacter : AbstractCharacter
{
    AreaOfInterest currentDestination;

    float DeadZone = 3f;

    Timer wanderTimer = new Timer(3);

    AbstractCharacter runningFrom;

    private void Update()
    {
        if(runningFrom == null || !HostileIsKnown)
        {
            Meander();
        }
        else
        {
            Vector3 awayDir = transform.position - runningFrom.transform.position;

            locomotor.MoveTowardsSpot(transform.position + awayDir.normalized * 3f);
        }
    }

    private void Meander()
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
        runningFrom = whoReactTo;
    }


}
