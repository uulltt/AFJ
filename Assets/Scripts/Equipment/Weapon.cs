using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponState
    {
        Idle,
        Shooting,
        Reloading
    }

    public int currentAmmo, maxAmmo;
    public float reloadTime;
    public float maxDamage, minDamage, shotInterval, range, accuracy, weight;
    public float currentRecoil, maxRecoil, incrementRecoil, reaimTime;

    public float damageArea;

    public LayerMask damageLayer;

    float random;

    WeaponState currentState;

    AbstractCharacter target;

    RaycastHit hit;

    Timer shotTimer;
    Timer reloadTimer;
    Timer reAimTimer;

    public AudioClip audioClip;
    public AudioSource audio;
    
    private void Awake()
    {
        shotTimer = new Timer(shotInterval);
        reloadTimer = new Timer(reloadTime);
        reAimTimer = new Timer(reaimTime);
    }

    public void End_SingleAttack()
    {
        StopAllCoroutines();
        currentState = WeaponState.Idle;
        currentRecoil = 0;
    }

    public void Start_SingleAttack(AbstractCharacter targetSelected)
    {
        if (currentState != WeaponState.Idle)
            return;

        StopAllCoroutines();
        currentState = WeaponState.Shooting;
        if(target != targetSelected)
        {
            reAimTimer.Reset();
        }
        target = targetSelected;
        StartCoroutine(SingleAttack());
    }

    IEnumerator SingleAttack()
    {
        while(shotTimer.elapseAsPercent < 1)
        {
            yield return null;
        }

        while(reAimTimer.elapseAsPercent < 1)
        {
            yield return null;
        }

        if (target == null)
        {
            End_SingleAttack();
        }
        else
        {
            random = Random.Range(0, 100) + currentRecoil;
            if (random < accuracy)
            {
                target.TakeDamage(Random.Range(minDamage, maxDamage));
            }
            currentRecoil = Mathf.Clamp(currentRecoil + incrementRecoil, 0, maxRecoil);

            audio.PlayOneShot(audioClip);

            currentAmmo -= 1;

            shotTimer.Reset();

            Debug.Log("Shot Fired");

            if (currentAmmo <= 0 || target == null)
            {
                End_SingleAttack();

                if (currentAmmo <= 0)
                    StartReload();
            }
            else
            {
                StartCoroutine(SingleAttack());
            }
        }
    }

    public void End_AreaAttack()
    {
        StopAllCoroutines();
        currentState = WeaponState.Idle;
        currentRecoil = 0;
    }

    public void Start_AreaAttack(AbstractCharacter targetSelected)
    {
        if (currentState != WeaponState.Idle)
            return;

        StopAllCoroutines();
        currentState = WeaponState.Shooting;
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
                hitCollider.gameObject.GetComponent<AbstractCharacter>().TakeDamage(Random.Range(minDamage, maxDamage));
            }
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(new Vector3(target.transform.position.x + Random.Range(.1f, 1f), target.transform.position.y, target.transform.position.z + Random.Range(.1f, 1f)), damageArea, damageLayer);
            foreach (var hitCollider in hitColliders)
            {
                hitCollider.gameObject.GetComponent<AbstractCharacter>().TakeDamage(Random.Range(minDamage, maxDamage));
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

    public void StartReload()
    {
        if (currentState == WeaponState.Idle)
        {
            Debug.Log("Reloading");
            reloadTimer.Reset();
            currentState = WeaponState.Reloading;
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        while (reloadTimer.elapseAsPercent < 1)
            yield return null;

        currentAmmo = maxAmmo;

        EndReload();
    }

    public void EndReload()
    {
        Debug.Log("Reloaded");
        currentState = WeaponState.Idle;
    }
}
