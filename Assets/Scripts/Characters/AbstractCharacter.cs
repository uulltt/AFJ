using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCharacter : MonoBehaviour
{
    public static List<AbstractCharacter> listOfEveryone = new List<AbstractCharacter>();
    public static bool HostileIsKnown = false;

    public float health = 100;
    public Weapon weapon;
    protected CharacterLocomotor locomotor;

    protected AbstractCharacter[] CharactersAwareOf;

    public bool isMale;

    public AudioClip maleHit, femaleHit, maleDead, femaleDead, armorHit;
    public AudioSource audio;

    private void Awake()
    {
        if(locomotor == null)
        {
            locomotor = GetComponent<CharacterLocomotor>();
        }

        if (weapon == null)
        {
            weapon = GetComponentInChildren<Weapon>();
        }

        if (weapon != null)
        {
            var gunHand = transform.Find("Gun Point");

            weapon.transform.parent = gunHand.transform;
            weapon.transform.localPosition = new Vector3();
        }

        if(audio == null)
        {
            audio = GetComponent<AudioSource>();
        }

        listOfEveryone.Add(this);
    }

    private void OnDestroy()
    {
        listOfEveryone.Remove(this);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (isMale)
        {
            audio.PlayOneShot(maleHit);
            if(armorHit != null)
            {
                audio.PlayOneShot(armorHit);
            }
        }
        else
        {
            audio.PlayOneShot(femaleHit);
            if (armorHit != null)
            {
                audio.PlayOneShot(armorHit);
            }
        }

        if (health <= 0)
        {
            if (isMale)
            {
                audio.PlayOneShot(maleDead);
                if (armorHit != null)
                {
                    audio.PlayOneShot(armorHit);
                }
            }
            else
            {
                audio.PlayOneShot(femaleDead);
                if (armorHit != null)
                {
                    audio.PlayOneShot(armorHit);
                }
            }
            health = 0;
            Die();
        }
        else if (isMale)
        {
            audio.PlayOneShot(maleHit);
            if (armorHit != null)
            {
                audio.PlayOneShot(armorHit);
            }
        }
        else
        {
            audio.PlayOneShot(femaleHit);
            if (armorHit != null)
            {
                audio.PlayOneShot(armorHit);
            }
        }

    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void SenseDanger()
    {

    }

    public virtual void SetCharactersAwareOf(AbstractCharacter[] characters)
    {
        CharactersAwareOf = characters;
    }

    public virtual void ReactToCharacter(AbstractCharacter whoReactTo)
    {

    }

    protected virtual void Attack(AbstractCharacter target)
    {
        if(weapon != null)
        {
            weapon.Start_SingleAttack(target);
        }
    }
}
