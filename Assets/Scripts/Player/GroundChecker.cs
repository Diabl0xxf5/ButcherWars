using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    HashSet<GameObject> _objInCollider = new HashSet<GameObject>();

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Ground"))
        {
            _objInCollider.Add(collider.gameObject);
        }       
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag.Equals("Ground"))
        {
            _objInCollider.Remove(collider.gameObject);
        }
    }

    public bool Grounded()
    {
        return _objInCollider.Count > 0;
    }

}
