using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;

    public static bool hostileDead;

    public GameObject selectedHostile;

    public GameObject weapon;

    //Hostile's Old Data
    public float health;
    public bool isMale;
    public AudioClip maleHit, femaleHit, maleDead, femaleDead, armorHit;

    public AudioSource audio;

    public int SoldierCount;

    public SpawningSystem spawningSystem;
    public GameObject resultsScreen;

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
                    
                    spawningSystem.SpawnCivilians();
                    audio.enabled = true;
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
                    goToScenarioPhase = true;
                    SoldierCount = FindObjectsOfType<GuardCharacter>().Length;
                }
                

                if (goToScenarioPhase)
                {
                    AssignHostile();
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

                if (hostileDead)
                {
                    goToPostGamePhase = true;
                }
                //Debug.Log("In phase " + phase);
                // scenario phase loop
                // random unit in base "turns"
                // game end conditions: turned is dead, everyone else is dead
                // keeps track of time elapsed, units killed
                if (goToPostGamePhase)
                {
                    resultsScreen.SetActive(true);
                    resultsScreen.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = time.elapseTime + " seconds";
                    resultsScreen.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = (spawningSystem.civilianCount - FindObjectsOfType<CivilianCharacter>().Length).ToString();
                    resultsScreen.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(0).GetComponent<TMPro.TMP_Text>().text = (SoldierCount - FindObjectsOfType<GuardCharacter>().Length).ToString();
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

        if (selectedHostile.GetComponentInChildren<FieldOfView>() != null)
        {
            Destroy(selectedHostile.GetComponentInChildren<FieldOfView>().gameObject);
        }

        health = selectedHostile.GetComponent<AbstractCharacter>().health;

        isMale = selectedHostile.GetComponent<AbstractCharacter>().isMale;

        //maleHit = selectedHostile.GetComponent<AbstractCharacter>().maleHit;
        //femaleHit = selectedHostile.GetComponent<AbstractCharacter>().femaleHit;
        //maleDead = selectedHostile.GetComponent<AbstractCharacter>().maleDead;
        //femaleDead = selectedHostile.GetComponent<AbstractCharacter>().femaleDead;
        //armorHit = selectedHostile.GetComponent<AbstractCharacter>().armorHit;
        Transform gunHand = selectedHostile.GetComponent<AbstractCharacter>().GunHand;
        Destroy(selectedHostile.GetComponent<AbstractCharacter>());

        selectedHostile.AddComponent<GunmanCharacter>();

        selectedHostile.GetComponent<GunmanCharacter>().health = health;
        selectedHostile.GetComponent<GunmanCharacter>().isMale = isMale;
        selectedHostile.GetComponent<GunmanCharacter>().maleHit = maleHit;
        selectedHostile.GetComponent<GunmanCharacter>().femaleHit = femaleHit;
        selectedHostile.GetComponent<GunmanCharacter>().maleDead = maleDead;
        selectedHostile.GetComponent<GunmanCharacter>().femaleDead = femaleDead;
        selectedHostile.GetComponent<GunmanCharacter>().armorHit = armorHit;
        selectedHostile.GetComponent<TargetingSystem>().myCharacter = selectedHostile.GetComponent<GunmanCharacter>();
        selectedHostile.GetComponent<TargetingSystem>().initialCheckRadius = 50f;
        selectedHostile.GetComponent<GunmanCharacter>().GunHand = gunHand;
        selectedHostile.GetComponent<GunmanCharacter>().audio = selectedHostile.GetComponent<AudioSource>();
        selectedHostile.GetComponent<GunmanCharacter>().Awake();
        selectedHostile.GetComponent<TargetingSystem>().sphereLayer = 1 << 14 | 1 << 10;
        selectedHostile.layer = LayerMask.NameToLayer("Targets");// 1 << 11;
    }
}
