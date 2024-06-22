using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField]
    public Transform hookTransform;
    private Grappling grappling;
    public bool activeHooking;

    private void Awake()
    {
        grappling = GetComponentInParent<Grappling>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activeHooking) return;

        if (other.tag.Equals("Bot"))
        {
            grappling.SetState(HookState.Compression);
            other.gameObject.transform.parent.parent = transform;
            other.gameObject.GetComponentInParent<Rigidbody>().isKinematic = true;
            activeHooking = false;
        } else if (other.tag.Equals("Pillar"))
        {
            grappling.SetState(HookState.Àttraction);
            activeHooking = false;
        }
    }
    
    public void DetachChildren()
    {
        if (transform.childCount > 1)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child == hookTransform) continue;

                child.parent = null;
                child.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
    
}
