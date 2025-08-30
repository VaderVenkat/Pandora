using UnityEngine;

public class Camera_Behaviour : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Vector3 CamOffSet = new Vector3(2f, 1.3f, -5f);

    [SerializeField] private float MouseSensitivity = 2f;

    private float XRotation = 0f;

    private float YRotation = 0f;

    private bool isLocked = false;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        XRotation = angles.x;
        YRotation = angles.y;
       // OffSet = transform.position - player.transform.position;
    }

    private void Update()
    {

        if (Time.timeScale == 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return; // stop here
        }

        // Mouse input
        if (Input.GetMouseButtonDown(0)) // Left click → lock
        {
            isLocked = true;
        }
        if (Input.GetMouseButtonDown(1)) // Right click → unlock
        {
            isLocked = false;
        }

        // Y button (toggle)
        if (Input.GetButtonDown("cam_mode")) // Y button
        {
            isLocked = !isLocked;
        }

        // --- Always enforce cursor state ---
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;   // force hidden
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;    // force visible
        }
    }

    private void LateUpdate()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X") ;
            float mouseY = Input.GetAxis("Mouse Y");

            // Joystick input (Right Stick → 4th & 5th axis in Input Manager)
            float joyX = Input.GetAxis("CameraX");
            float joyY = Input.GetAxis("CameraY");

            // Combine inputs
            float lookX = (mouseX + joyX) * MouseSensitivity ;
            float lookY = (mouseY + joyY) * MouseSensitivity ;

            YRotation += lookX;
            XRotation -= lookY;

            XRotation = Mathf.Clamp(XRotation, 0f, 0f);

            Quaternion rotation = Quaternion.Euler(XRotation, YRotation, 0);
            transform.position = player.position + rotation * CamOffSet;
            transform.LookAt(player.position + Vector3.up * 1.5f);
        }
        
    }
}
