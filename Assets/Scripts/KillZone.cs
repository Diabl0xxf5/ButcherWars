using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Health hp = other.GetComponentInParent<Health>();
        if (hp) {
            hp.Kill();
        } else
        {
            Destroy(other.gameObject);
        }
    }
}
