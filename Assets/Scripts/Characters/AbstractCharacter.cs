using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCharacter : MonoBehaviour
{
    protected float health;
    protected List<AbstractEquipment> equipment = new List<AbstractEquipment>();
    protected CharacterLocomotor locomotor;

    protected AbstractCharacter[] CharactersAwareOf;


    private void Awake()
    {
        if(locomotor == null)
        {
            locomotor = GetComponent<CharacterLocomotor>();
        }
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

    public virtual void Die()
    {

    }

    public virtual void SenseDanger()
    {

    }

    public virtual void SetCharactersAwareOf(AbstractCharacter[] characters)
    {
        CharactersAwareOf = characters;
    }

    protected virtual void CheckDistanceToCharacter(AbstractCharacter character, out float distance)
    {
        distance = Vector3.Distance(transform.position, character.transform.position);
    }

    protected virtual void ReactToVisible()
    {
        if (CharactersAwareOf == null)
            return;

        float closestDistance = 10000;
        AbstractCharacter whoReactingTo = null;

        for(int i = 0; i < CharactersAwareOf.Length; i++)
        {
            CheckDistanceToCharacter(CharactersAwareOf[i], out float dist);
            if(closestDistance > dist)
            {
                whoReactingTo = CharactersAwareOf[i];
                closestDistance = dist;
            }
        }

        if(whoReactingTo != null)
        {
            ReactToCharacter(whoReactingTo);
        }
    }

    protected virtual void ReactToCharacter(AbstractCharacter whoReactTo)
    {

    }

    protected virtual void Attack(AbstractCharacter target)
    {

    }




}
