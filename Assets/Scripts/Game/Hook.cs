using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField]
    public Transform hookTransform;
    private Grappling grappling;
    public bool activeHooking;
    private PlayerBehaviour _pb;

    private void Awake()
    {
        grappling = GetComponentInParent<Grappling>();
        _pb = GetComponentInParent<PlayerBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activeHooking) return;
        
        if (other.tag.Equals("Bot") || other.tag.Equals("Player"))
        {
            GameObject grappleGO = other.gameObject.transform.parent.gameObject;
            if (grappleGO.GetComponent<PlayerBehaviour>() == _pb) return;

            grappling.SetState(HookState.Compression);

            grappleGO.transform.parent = transform;
            grappleGO.transform.localPosition = new Vector3(grappleGO.transform.localPosition.x, -0.2f , grappleGO.transform.localPosition.z);
            grappleGO.GetComponent<Rigidbody>().isKinematic = true;
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
