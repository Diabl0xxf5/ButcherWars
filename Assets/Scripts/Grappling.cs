using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    public Transform _camera;

    [Header("Settings")]
    public int maxGrappleDistance;
    public int quality;
    public float drawCD;

    [Header("Hook models")]
    public GameObject hook;
    public GameObject originalHook;

    private bool isGrappling;
    private Vector3 camera_forward;
    private LineRenderer lr;
    private float drawCDTimer;
    private int currentDistance;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!isGrappling && Input.GetMouseButtonDown(1)) {
            StartGrapple();
        }      
    }

    void LateUpdate()
    {
        if (isGrappling)
        {
            if (currentDistance == maxGrappleDistance) {
                StopGrapple();
                return;
            }
            else if (drawCDTimer > 0) {
                drawCDTimer -= Time.deltaTime;
            } else {

                Vector3 new_position = lr.GetPosition(quality - 1) + camera_forward;
                lr.SetPosition(quality - 1, new_position);

                hook.transform.position = new_position;
                currentDistance++;
                drawCDTimer = drawCD;

            }

            Vector3 delta_pos = lr.GetPosition(quality - 1) - transform.position;
            lr.SetPosition(0, transform.position);
            for (int i = 1; i < quality; i++)
            {
                double coef = Math.Sin(Math.PI * i / 2 / (quality - 1));
                lr.SetPosition(i, transform.position + delta_pos * (float)coef);
            }
        }
    }

    void StartGrapple()
    {
        hook.SetActive(true);
        hook.transform.parent = null;

        originalHook.SetActive(false);
        lr.enabled = true;
        lr.positionCount = quality;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(quality - 1, transform.position);
        currentDistance = 0;

        camera_forward = _camera.forward;
        camera_forward.y = 0;
        camera_forward.Normalize();

        isGrappling = true;
    }

    void StopGrapple()
    {   
        hook.SetActive(false);
        hook.transform.parent = transform;
        hook.transform.localPosition = Vector3.zero;
        hook.transform.localEulerAngles = Vector3.zero;
        
        originalHook.SetActive(true);
        lr.enabled = false;
        lr.positionCount = 0;
        drawCDTimer = 0;

        isGrappling = false;
    }

}
