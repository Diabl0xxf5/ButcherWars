using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("References")]
    public PlayerControl _playerControl;
    public Animator _animator;

    public Transform _spawnPoint;
    public Teams _team;

    private void Start()
    {
        GameManager.instance.FillPlayerBehaviour(this);
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
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
        _animator.SetTrigger("Punch");
        Stop();
        StartCoroutine(PlayContinious());
    }

    public void Die()
    {
        _animator.SetTrigger("Die");
        Stop();
    }

    IEnumerator PlayContinious()
    {
        yield return new WaitForSeconds(0.5f);
        Play();
    }

}
