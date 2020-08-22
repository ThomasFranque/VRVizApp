using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRWheel.VR.Input
{
    public class SpawnListener : MonoBehaviour
    {
        [SerializeField]
        private string _targetObjectName = default;

        // Update is called once per frame
        void Update()
        {
            GameObject found = GameObject.Find(_targetObjectName);
            if (found != null) OnFound(found);
        }

        private void OnFound(GameObject found)
        {
            // transform.position = found.transform.position;
            // transform.forward = found.transform.forward;
            // gameObject.AddComponent<FakeChild>().Init(found.transform, true);
            transform.SetParent(found.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            Destroy(this);
        }
    }
}