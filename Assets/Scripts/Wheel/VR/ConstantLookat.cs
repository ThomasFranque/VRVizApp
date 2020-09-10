using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRWheel.VR
{
    public class ConstantLookat : MonoBehaviour
    {
        public Transform _target = default;

        private bool _look;
        private void Awake() 
        {
            _look= false;
        }

        private void Update() 
        {
            _look = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
        }


        // Update is called once per frame
        void LateUpdate()
        {
            if (!_look) return;
            transform.LookAt(_target);
            transform.forward = - transform.forward;
        }
    }
}