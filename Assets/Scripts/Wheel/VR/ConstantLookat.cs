using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRWheel.VR
{
    public class ConstantLookat : MonoBehaviour
    {
        public Transform _target = default;


        // Update is called once per frame
        void LateUpdate()
        {
            transform.LookAt(_target);
            transform.forward = - transform.forward;
        }
    }
}