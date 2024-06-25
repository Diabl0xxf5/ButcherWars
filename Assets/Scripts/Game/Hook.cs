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
            GameObject botGO = other.gameObject.transform.parent.gameObject;

            botGO.transform.parent = transform;
            botGO.transform.localPosition = new Vector3(botGO.transform.localPosition.x, -0.2f ,botGO.transform.localPosition.z);
            botGO.GetComponent<Rigidbody>().isKinematic = true;
            activeHooking = false;
        } else if (other.tag.Equals("Pillar"))
        {
            grappling.SetState(HookState.Àttraction);
            activeHooking = false;
        } else if (other.tag.Equals("Prop"))
        {
            grappling.SetState(HookState.Compression);
            activeHooking = false;
        } else if (other.tag.Equals("Ground"))
        {
            grappling.SetState(HookState.Compression);
            activeHooking = false;
            //grappling.GrappleForwardBounce();
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
