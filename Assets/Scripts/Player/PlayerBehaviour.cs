using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    [SerializeField] PlayerMovement _playerMove;
    [SerializeField] Animator _animator;

    public void Play()
    {
        if (_playerMove)
        {
            _playerMove.enabled = true;
        }
    }

    public void Stop()
    {
        if (_playerMove)
        {
            _playerMove.enabled = false;
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
