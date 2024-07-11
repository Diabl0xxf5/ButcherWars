using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("References")]
    public PlayerControl _playerControl;
    public NetworkedAnimation _nwAnimator;
    public InGameMenu _inGameMenu;
    public PhotonView _pview;
    
    public GameObject _inGameHood;
    public GameObject _camera;
    public GameObject _HPBar;
    public GameObject _InGameMenu;

    

    public bool died;
    public PhotonTeam _photonTeam;

    private void Start()
    {
        if (_pview.IsMine) {
            _camera.SetActive(true);
            _InGameMenu.SetActive(true);
            _inGameHood.SetActive(true);
            _photonTeam = PhotonManager._photonTeam;
            GameManager.instance.playerCameraTransform = _camera.transform;
            Respawn();
        } else
        {
            _HPBar.SetActive(true);
        }
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

        Transform spawn_point = GameManager.instance.GetSpawnPoint();

        transform.position = spawn_point.position;
        transform.eulerAngles = spawn_point.eulerAngles;
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
