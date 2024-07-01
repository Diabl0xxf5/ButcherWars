using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPatrol : MonoBehaviour
{
    [Header("References")]
    public List<Transform> _puth;

    [Header("Settings")]
    public float speedTimer;

    private int target_index = 1;
    private float speedTimerCD;
    private Vector3 prev_position;
    private Vector3 prev_rotation;
    private bool rotate;

    private void Start()
    {
        transform.position = _puth[0].position;
        prev_position = transform.position;
        prev_rotation = transform.eulerAngles;
    }

    private void Update()
    {
        if (speedTimerCD > speedTimer)
        {
            transform.position = _puth[target_index].position;
            target_index = (target_index == (_puth.Count - 1)) ? 0 : (target_index + 1);
            speedTimerCD = 0;
            prev_position = transform.position;
            prev_rotation = transform.eulerAngles;
            rotate = !rotate;
        }

        transform.LookAt(_puth[target_index]);

        speedTimerCD += Time.deltaTime;

        transform.position = Vector3.Lerp(prev_position, _puth[target_index].position, speedTimerCD / speedTimer);
        
        if (rotate)
            transform.eulerAngles = Vector3.Lerp(prev_rotation, new Vector3(prev_rotation.x, prev_rotation.y - 90f, prev_rotation.z), speedTimerCD / speedTimer);
    }

}
