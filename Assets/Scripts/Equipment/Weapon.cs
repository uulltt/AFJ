using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int currentAmmo, maxAmmo;
    public float reloadTime;
    public float damage, shotInterval, range, accuracy, weight;
    public float currentRecoil, maxRecoil, incrementRecoil;

    public float damageArea;

    public LayerMask damageLayer;

    float random;

    bool isAttack;

    AbstractCharacter target;

    RaycastHit hit;

    public void End_SingleAttack()
    {
        StopAllCoroutines();
        isAttack = false;
        currentRecoil = 0;
    }

    public void Start_SingleAttack(AbstractCharacter targetSelected)
    {
        StopAllCoroutines();
        isAttack = true;
        target = targetSelected;
        StartCoroutine(SingleAttack());
    }

    IEnumerator SingleAttack()
    {
        random = Random.Range(0, 100) + currentRecoil;
        if (random < accuracy)
        {
            target.TakeDamage(damage);
        }
        currentRecoil = Mathf.Clamp(currentRecoil + incrementRecoil, 0, maxRecoil);

        currentAmmo -= 1;

        yield return new WaitForSeconds(shotInterval);

        if (currentAmmo > 0)
        {
            StartCoroutine(SingleAttack());
        }
        else
        {
            End_SingleAttack();
        }
    }

    public void End_AreaAttack()
    {
        StopAllCoroutines();
        isAttack = false;
        currentRecoil = 0;
    }

    public void Start_AreaAttack(AbstractCharacter targetSelected)
    {
        StopAllCoroutines();
        isAttack = true;
        target = targetSelected;
        StartCoroutine(AreaAttack());
    }

    IEnumerator AreaAttack()
    {
        random = Random.Range(0, 100) + currentRecoil;
        if (random < accuracy)
        {
            Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, damageArea, damageLayer);
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.gameObject.GetComponent<AbstractCharacter>().TakeDamage(damage);
            }
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(target.transform.position.x + Random.Range(.1f, 1f), target.transform.position.y, target.transform.position.z + Random.Range(.1f, 1f)), damageArea, damageLayer);
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.gameObject.GetComponent<AbstractCharacter>().TakeDamage(damage);
            }
        }

        currentRecoil = Mathf.Clamp(currentRecoil + incrementRecoil, 0, maxRecoil);

        currentAmmo -= 1;

        yield return new WaitForSeconds(shotInterval);

        if (currentAmmo > 0)
        {
            StartCoroutine(SingleAttack());
        }
        else
        {
            End_SingleAttack();
        }
    }
}
