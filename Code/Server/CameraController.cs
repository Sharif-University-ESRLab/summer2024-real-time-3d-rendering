using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10.0f; 
    public float lookSpeed = 2.0f; 

    private float yaw = 0.0f;       
    private float pitch = 0.0f;     

    void Update()
    {
        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch -= lookSpeed * Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.Self);
    }
}
