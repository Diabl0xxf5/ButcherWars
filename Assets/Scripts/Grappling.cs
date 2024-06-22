using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HookState
{
    Expansion,
    Compression,
    Аttraction,
    None
}


public class Grappling : MonoBehaviour
{
    [Header("References")]
    public Transform _camera;
    public Transform player;

    [Header("Settings")]
    public float maxGrappleDistance;
    public float minGrappleDistance;
    public int quality;
    public float speed;

    [Header("Hook models")]
    public GameObject hookGO;
    public GameObject originalHook;
    public Hook hook;

    private Vector3 grapple_forward;
    private Vector3 start_grapple_position;
    private Vector3 end_grapple_position;
    private LineRenderer lr;
    
    private float currentDistance;
    private float[] SinCoefs;
    private float[] LineCoefs;
    private bool drawline;
    public HookState hookState;
    

    void OnValidate()
    {
        FillSinCoefs();
    }

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        SetState(HookState.None);
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
        if (hookState == HookState.None && Input.GetMouseButtonDown(1))
        {
            SetState(HookState.Expansion);
        }
    }

    void LateUpdate()
    {   
       GrapplingStateMachine();       
    }

    void GrapplingStateMachine()
    {
        if (hookState == HookState.None) {
            return;
        }

        if (currentDistance >= maxGrappleDistance)
        {
            SetState(HookState.Compression);
        }
        else if ((hookState == HookState.Compression || hookState == HookState.Аttraction) && currentDistance <= minGrappleDistance)
        {
            SetState(HookState.None);
            return;
        }

        if (hookState == HookState.Expansion)
        {
            end_grapple_position += grapple_forward * Time.deltaTime * speed;
            lr.SetPosition(quality - 1, end_grapple_position);

            hookGO.transform.position = end_grapple_position;
        }
        else if (hookState == HookState.Compression)
        {
            end_grapple_position += (transform.position - end_grapple_position).normalized * Time.deltaTime * speed;
            lr.SetPosition(quality - 1, end_grapple_position);

            hookGO.transform.position = end_grapple_position;
        } else if (hookState == HookState.Аttraction)
        {
            player.position += (end_grapple_position - player.position).normalized * Time.deltaTime * speed;
        }

        currentDistance = (end_grapple_position - transform.position).magnitude;

        if (drawline)
            DrawGrapple_line();
        else
            DrawGrapple_diagonal();

    }

    public void SetState(HookState h_state)
    {
        hookState = h_state;

        if (hookState == HookState.Expansion)
        {
            StartGrapple();
        }
        else if (hookState == HookState.None)
        {
            StopGrapple();
        }
        else if (hookState == HookState.Аttraction)
        {
            player.GetComponent<Rigidbody>().isKinematic = true;
        }

    }

    public void GrappleForwardBounce()
    {

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
        hook.activeHooking = true;

        start_grapple_position = transform.position;
        end_grapple_position = start_grapple_position;

        hookGO.SetActive(true);
        hookGO.transform.parent = null;
        hookGO.transform.LookAt(start_grapple_position + _camera.forward);

        originalHook.SetActive(false);
        lr.enabled = true;
        lr.positionCount = quality;
        lr.SetPosition(0, start_grapple_position);

        currentDistance = 0;

        grapple_forward = _camera.forward;
        //camera_forward.y = 0;
        grapple_forward.Normalize();

        float cos_camera = Mathf.Abs(grapple_forward.x / grapple_forward.magnitude);
        drawline = (cos_camera < 0.48f || cos_camera > 0.87f); // от 30 до 60 градусов включается отрисовка по диагонали, иначе линейная
    }

    void StopGrapple()
    {
        hook.DetachChildren();

        hookGO.SetActive(false);
        hookGO.transform.parent = transform;
        hookGO.transform.localPosition = Vector3.zero;
        hookGO.transform.localEulerAngles = Vector3.zero;

        originalHook.SetActive(true);
        lr.enabled = false;
        lr.positionCount = 0;

        player.GetComponent<Rigidbody>().isKinematic = false;
    }

}
