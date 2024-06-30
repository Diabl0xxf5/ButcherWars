using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviourPunCallbacks
{

    [Header("References")]
    public HPBar _InGameHood;
    public HPBar _InWorld;
    public PlayerBehaviour _pb;
    public Animation _anim;
    public GameObject _damageImage;
    public GameObject _healEffect;
    public GameObject _hitEffect;


    [Header("Settings")]
    public int _hp = 100;


    private int _max;
    PhotonView pv;
    Rigidbody rb;

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    void Awake()
    {
        _max = _hp;
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    public void Kill()
    {
        Damage(_hp, Vector3.zero);
    }

    private void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == PhotonManager.AttackEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int targetPhotonView = (int)data[0];
            if (targetPhotonView == this.photonView.ViewID)
            {
                Damage((int)data[1], (Vector3)data[2]);
            }
        } else if (eventCode == PhotonManager.HealEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int targetPhotonView = (int)data[0];
            if (targetPhotonView == this.photonView.ViewID)
            {
                Heal((int)data[1]);
            }
        }
    }

    public void SendAttackEvent(int value, Vector3 forceVector)
    {
        Damage(value, forceVector);
        PhotonManager.SendAttackEvent(pv.ViewID, value, forceVector);
    }

    public void Damage(int value, Vector3 forceVector)
    {
        if (_hp == 0) return;

        rb.AddForce(forceVector, ForceMode.Impulse);

        _hp -= Mathf.Min(_hp, value);
        _InGameHood.UpdateView(_hp, _max);
        _InWorld.UpdateView(_hp, _max);

        if (_anim)
        {
            _anim.Play();
        }

        if (_damageImage)
        {
            _damageImage.SetActive(true);
            StartCoroutine(offDamageImage());
        }

        if (_hp == 0)
        {
            _pb.Die();
        } else
        {
            _pb.TakeDamage();
        }

        Instantiate(_hitEffect, transform);

    }

    IEnumerator offDamageImage()
    {
        yield return new WaitForSeconds(0.15f);
        _damageImage.SetActive(false);
    }

    public void FullHeal()
    {
        Heal(_max);
    }

    public void Heal(int value)
    {
        _hp = Mathf.Min(_hp + value, _max);
        _InGameHood.UpdateView(_hp, _max);
        _InWorld.UpdateView(_hp, _max);
        
        Instantiate(_healEffect, transform);

        if (pv.IsMine) PhotonManager.SendHealEvent(pv.ViewID, value);
    }

}
