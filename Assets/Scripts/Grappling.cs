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
    private Vector3 start_grapple_position;
    private Vector3 end_grapple_position;
    private LineRenderer lr;
    private float drawCDTimer;
    private int currentDistance;
    private float[] SinCoefs;
    private float[] LineCoefs;
    private bool drawline;

    void OnValidate()
    {
        FillSinCoefs();
    }

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        FillSinCoefs();
    }

    void FillSinCoefs()
    {
        SinCoefs = new float[quality];
        LineCoefs = new float[quality];

        for (int i = 0; i < quality; i++)
        {
            SinCoefs[i] = Mathf.Sin(Mathf.PI * i / 2 / (quality - 1));
            LineCoefs[i] = (float)i / quality;
        }
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
            GrapplingExecute();
        }
    }

    void GrapplingExecute()
    {
        if (currentDistance == maxGrappleDistance)
        {
            StopGrapple();
            return;
        }
        else if (drawCDTimer > 0)
        {
            drawCDTimer -= Time.deltaTime;
        }
        else
        {
            end_grapple_position += camera_forward;
            lr.SetPosition(quality - 1, end_grapple_position);

            hook.transform.position = end_grapple_position;
            currentDistance++;
            drawCDTimer = drawCD;
        }

        if (drawline)
            DrawGrapple_line();
        else
            DrawGrapple_diagonal();

    }

    void DrawGrapple_line()
    {
        Vector3 delta_hook = end_grapple_position - transform.position;

        bool AlignmentX = Mathf.Abs(delta_hook.x) < Mathf.Abs(delta_hook.z);

        for (int i = 0; i < quality; i++)
        {
            if (AlignmentX)
                lr.SetPosition(i, transform.position + new Vector3(delta_hook.x * SinCoefs[i], 
                                                                    delta_hook.y * LineCoefs[i], 
                                                                    delta_hook.z * LineCoefs[i]));
            else
                lr.SetPosition(i, transform.position + new Vector3(delta_hook.x * LineCoefs[i], 
                                                                    delta_hook.y * LineCoefs[i], 
                                                                    delta_hook.z * SinCoefs[i]));
        }
    }

    void DrawGrapple_diagonal()
    {
        Vector3 delta_player = start_grapple_position - transform.position;
        Vector3 delta_hook = end_grapple_position - start_grapple_position;
        Vector3 delta_pos = end_grapple_position - transform.position;

        bool AlignmentX = Mathf.Abs(delta_player.x) < Mathf.Abs(delta_player.z);

        for (int i = 0; i < quality; i++)
        {
            if (AlignmentX)
                lr.SetPosition(i, transform.position + new Vector3(delta_hook.x * LineCoefs[i] + delta_player.x * SinCoefs[i],              //x
                                                                   delta_pos.y * LineCoefs[i],                                             //y
                                                                   delta_pos.z * LineCoefs[i]));                                           //z
            else
                lr.SetPosition(i, transform.position + new Vector3(delta_pos.x * LineCoefs[i],                                             //x
                                                                   delta_pos.y * LineCoefs[i],                                             //y
                                                                   delta_hook.z * LineCoefs[i] + delta_player.z * SinCoefs[i]));            //z
        }
    }

    void StartGrapple()
    {
        start_grapple_position = transform.position;
        end_grapple_position = start_grapple_position;

        hook.SetActive(true);
        hook.transform.parent = null;

        originalHook.SetActive(false);
        lr.enabled = true;
        lr.positionCount = quality;
        lr.SetPosition(0, start_grapple_position);
        
        currentDistance = 0;

        camera_forward = _camera.forward;
        camera_forward.y = 0;
        camera_forward.Normalize();

        float cos_camera = Mathf.Abs(camera_forward.x / camera_forward.magnitude);
        drawline = (cos_camera < 0.48f || cos_camera > 0.87f); // от 30 до 60 градусов включается отрисовка по диагонали, иначе линейная

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
