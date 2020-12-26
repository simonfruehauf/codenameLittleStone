using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform target;
    public float speed = 0.03f;
        
    void Start()
    {
        if (target == null)
        {
            target = GameObject.Find("Main Camera").transform;
        }    
    }

    void Update()
    {
        transform.position = new Vector2(target.transform.position.x * -speed, 0);
    }
}
