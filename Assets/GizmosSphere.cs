using UnityEngine;

public class GizmosSphere : MonoBehaviour
{
    [SerializeField] private Color color = Color.blue  ;
    [SerializeField] private float _size = 0.5f;
    private void OnDrawGizmos() {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, _size);
    }
}
