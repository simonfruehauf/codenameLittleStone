using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{ // Smooth towards the target

    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 target_Offset;
    PlayerController player;
    PauseMenu PauseMenuScript;

    private void Start()
    {
        PauseMenuScript = FindObjectOfType<PauseMenu>();
        player = FindObjectOfType<PlayerController>();
        target_Offset = transform.position - target.position;
    }
    void Update()
    {
        if (!PauseMenuScript.pause)
        {
            if (target && player.started)
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), ref velocity, smoothTime);
            }
        }


    }
}

