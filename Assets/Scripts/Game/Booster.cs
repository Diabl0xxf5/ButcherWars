using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField]
    Animation _boostAnim;

    [SerializeField]
    private float _jumpForce;

    private bool _ready = true;

    private void OnTriggerEnter(Collider other)
    {
        if (_ready && other.tag.Equals("Player"))
        {
            _ready = false;
            _boostAnim.Play();
            
            Rigidbody other_rb = other.GetComponentInParent<Rigidbody>();
            other_rb.velocity = new Vector3(other_rb.velocity.x, 0, other_rb.velocity.z);
            other_rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

            Animator other_anim = other.GetComponentInChildren<Animator>();
            other_anim.SetBool("Salto", true);
            other_anim.SetTrigger("Salting");

            StartCoroutine(_readyReset(other_anim));
        }
    }

    IEnumerator _readyReset(Animator anim)
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Salto", false);
        _ready = true;
    }

}
