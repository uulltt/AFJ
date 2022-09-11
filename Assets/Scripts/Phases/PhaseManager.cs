using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;

    public GameObject selectedHostile;

    public GameObject weapon;

    private bool goToPrepPhase;
    public bool GoToPrepPhase
    {
        get
        {
            return goToPrepPhase;
        }
        set
        {
            goToPrepPhase = value;
        }
    }

    public bool goToScenarioPhase;

    private bool goToPostGamePhase;
    public bool GoToPostGamePhase
    {
        get
        {
            return goToPostGamePhase;
        }

        set
        {
            goToPostGamePhase = value;
        }
    }

    private bool tryAgain;
    public bool TryAgain
    {
        get
        {
            return tryAgain;
        }
        set
        {
            tryAgain = value;
        }
    }

    private bool quit;
    public bool Quit
    {
        get
        {
            return quit;
        }
        set
        {
            quit = value;
        }
    }

    public Timer time;
    public Timer reactionTime;

    private enum Phases
    {
        BUY,
        PREP,
        SCENARIO,
        POST_GAME
    }

    private Phases phase;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    private void Start()
    {
        phase = Phases.BUY;
        goToPrepPhase = false;
        goToScenarioPhase = false;
        goToPostGamePhase = false;
        time = new Timer(UnityEngine.Random.Range(30f, 90f));
        reactionTime = new Timer(1.0f);
    }

    private void Update()
    {
        switch (phase)
        {
            case Phases.BUY:
                if (goToPrepPhase)
                {
                    goToPrepPhase = false;
                    //Debug.Log("In Buy Phase, moving to Prep Phase!");
                    phase++;
                    time.Reset();
                }

                //Debug.Log("In phase " + phase);

                // buy phase loop
                // no time limit,
                // building/menus enabled,
                // wait for next phase
                break;
            case Phases.PREP:
                Debug.Log("In phase " + phase);

                
                if (time.isComplete)
                {
                    AssignHostile();
                    goToScenarioPhase = true;
                }
                

                if (goToScenarioPhase)
                {
                    goToScenarioPhase = false;
                    Debug.Log("In Prep Phase, moving to Scenario Phase!");
                    phase++;
                }

                // prep phase loop
                // menus gone,
                // no building,
                // user just maneuvers troops, watches civis, whatever
                // scenario phase randomly triggers...
                break;
            case Phases.SCENARIO:
                if (!reactionTime.isComplete)
                {
                    break;
                }

                //Debug.Log("In phase " + phase);
                // scenario phase loop
                // random unit in base "turns"
                // game end conditions: turned is dead, everyone else is dead
                // keeps track of time elapsed, units killed
                if (goToPostGamePhase)
                {
                    goToPostGamePhase = false;
                    //Debug.Log("In Scenario Phase, moving to Post Game Phase!");
                    phase++;
                }

                break;
            case Phases.POST_GAME:
                Debug.Log("In Post Game Phase, awaiting Try Again or Quit!");
                Debug.Log("In phase " + phase);
                // scoreboard shows score (time, deaths)
                // display try again and quit buttons
                if (tryAgain)
                {
                    // start things over
                    Debug.Log("OK let's try again!");
                    tryAgain = false;
                    phase = Phases.BUY;
                }

                if (quit)
                {
                    // trigger close
                    Debug.Log("Bye!");
                    quit = false;
                    phase = Phases.BUY;
                    //Application.Quit();
                }
                break;
        }
    }

    public void AssignHostile()
    {
        selectedHostile = AbstractCharacter.listOfEveryone[UnityEngine.Random.Range(0, AbstractCharacter.listOfEveryone.Count)].gameObject;

        if (selectedHostile.GetComponent<AbstractCharacter>().weapon == null)
        {
            GameObject newWeapon = Instantiate(weapon, selectedHostile.transform.position, Quaternion.identity);
            newWeapon.transform.parent = selectedHostile.gameObject.transform;
        }

        Destroy(selectedHostile.GetComponent<AbstractCharacter>());

        selectedHostile.AddComponent<GunmanCharacter>();

    }
}
