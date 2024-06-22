using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField]
    public int damage;

    [SerializeField]
    int pushForce;

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health)
        {
            health.Damage(damage);
        }

        Rigidbody other_rb = other.GetComponentInParent<Rigidbody>();
        if (other_rb)
        {
            other_rb.AddForce((other.transform.position - transform.position) * pushForce, ForceMode.Impulse);
        }
        
    }

}
