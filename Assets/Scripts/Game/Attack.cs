using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    
    int damage;
    int pushForce;
    Collider _attackCollider;

    private void Awake()
    {
        _attackCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();
        PhotonView pv = other.GetComponentInParent<PhotonView>();

        if (health)
        {
            Vector3 forceVector = (other.transform.position - transform.position) * pushForce;
            health.SendAttackEvent(damage, forceVector);
        }
        
    }

    public void StartAttack(int dmg, int pforce)
    {
        damage = dmg; pushForce = pforce;
        StartCoroutine(activateAttackTrigger(0.25f));
        StartCoroutine(disableAttackTrigger(0.5f));
    }

    IEnumerator activateAttackTrigger(float delay)
    {
        yield return new WaitForSeconds(delay);
        _attackCollider.enabled = true;
    }

    IEnumerator disableAttackTrigger(float delay)
    {
        yield return new WaitForSeconds(delay);
        _attackCollider.enabled = false;
    }

}
