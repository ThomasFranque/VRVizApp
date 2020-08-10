using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    // Header for the sake of organization
    [Header("Components")]

    [SerializeField]
    private float sensitivity;

    [SerializeField]
    private Transform player;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        player.localRotation = Quaternion.Euler(0, yRotation, 0);
    }
}