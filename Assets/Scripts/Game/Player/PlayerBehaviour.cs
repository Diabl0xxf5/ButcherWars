using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("References")]
    public PlayerControl _playerControl;
    public NetworkedAnimation _nwAnimator;
    public InGameMenu _inGameMenu;
    public PhotonView _pview;

    public Transform _spawnPoint;
    public Teams _team;
    public bool died;

    private void Start()
    {
        if (_pview.IsMine) {
            GameManager.instance.FillPlayerBehaviour(this);
            Respawn();
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.ResetSlot(this);
    }

    public void Play()
    {
        if (_playerControl)
        {
            _playerControl.enabled = true;
        }
    }

    public void Stop()
    {
        if (_playerControl)
        {
            _playerControl.enabled = false;
        }     
    }

    public void TakeDamage()
    {
        _nwAnimator.SetTrigger("Punch");
        Stop();
        StartCoroutine(PlayContinious());
    }

    public void Die()
    {
        _nwAnimator.SetTrigger("Die");
        Stop();
        died = true;
        if (_pview.IsMine) _inGameMenu.Show();
    }

    public void Respawn()
    {
        transform.parent = null;
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        GetComponent<Health>().FullHeal();
        Play();

        died = false;
    }



    IEnumerator PlayContinious()
    {
        yield return new WaitForSeconds(0.5f);
        Play();
    }

}
