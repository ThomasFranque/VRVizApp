using UnityEngine;
using Archives_Deprecated;

public class Selecter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Archive"))
        {
            //other.GetComponent<Archive>().Select();
        }
    }
}
