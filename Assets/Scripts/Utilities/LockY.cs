using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockY : MonoBehaviour
{
    float _initialY;
    private void Awake()
    {
        _initialY = transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, _initialY, transform.position.z);
    }
}