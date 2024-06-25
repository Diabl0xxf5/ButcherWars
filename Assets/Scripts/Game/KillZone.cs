using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Hook")) return;


        Health hp = collision.collider.GetComponentInParent<Health>();
        if (hp)
        {
            hp.Kill();
        }
        else
        {
            Destroy(collision.collider.gameObject);
        }
    }

}
