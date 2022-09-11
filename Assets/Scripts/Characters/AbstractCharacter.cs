using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCharacter : MonoBehaviour
{
    public static List<AbstractCharacter> listOfEveryone = new List<AbstractCharacter>();

    public float health = 100;
    public Weapon weapon;
    protected CharacterLocomotor locomotor;

    protected AbstractCharacter[] CharactersAwareOf;


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

        listOfEveryone.Add(this);
    }

    private void OnDestroy()
    {
        listOfEveryone.Remove(this);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
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
