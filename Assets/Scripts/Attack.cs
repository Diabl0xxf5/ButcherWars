using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField]
    int damage;

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health)
        {
            health.Damage(damage);
        }
    }

}
