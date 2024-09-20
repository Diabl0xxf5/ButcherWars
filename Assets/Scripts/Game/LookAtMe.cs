using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour
{

    Transform _camera;

    void FixedUpdate()
    {

        if (_camera)
        {
            transform.LookAt(_camera);
        }
        else if (GameManager.instance != null && GameManager.instance.playerCameraTransform != null)
        {
            _camera = GameManager.instance.playerCameraTransform;
        }

    }

}
