using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Attack : MonoBehaviour
{
    
    int damage;
    int pushForce;
    Collider _attackCollider;
    PhotonView _pv;
    Teams _team;

    private void Awake()
    {
        _attackCollider = GetComponent<BoxCollider>();
        _pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _team = GetComponentInParent<PlayerBehaviour>()._team;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_pv.IsMine) return;

        Health health = other.GetComponentInParent<Health>();

        if (health)
        {
            if (health.killed) return;

            PlayerBehaviour pb = other.GetComponentInParent<PlayerBehaviour>();

            if (pb._team == _team) return;

            Vector3 forceVector = (other.transform.position - transform.position) * pushForce;
            health.SendAttackEvent(damage, forceVector);
            
            if (health.killed)
            {
                YandexPlugin.instance.AddKill();
                GameManager.instance.PlayerKill(_team);
                PhotonManager.SendKillRPC((int)_team);
            }
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
        Sounds.instance.PlaySound(SoundType.Attack);
    }

    IEnumerator disableAttackTrigger(float delay)
    {
        yield return new WaitForSeconds(delay);
        _attackCollider.enabled = false;
    }

}
