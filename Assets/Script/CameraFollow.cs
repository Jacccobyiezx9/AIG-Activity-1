using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float rotationSpeed;

    public float rotationStep = 90f;
    public float smoothSpeed = 5f;

    private float targetYRotation = 0f;
    private float defaultYRotation = 90f;

    void Update()
    {
        transform.position = player.position;
        //Mouse
        //if (Input.GetMouseButton(1))
        //{
        //    float horizontal = Input.GetAxis("Mouse X");
        //    transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
        //}

        //Hold
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            horizontal = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            targetYRotation = defaultYRotation;
        }

        if (horizontal != 0f)
        {
            transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
            targetYRotation = transform.eulerAngles.y;
        }

        ////Snap
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    targetYRotation -= rotationStep;
        //}
        //else if (Input.GetKeyDown(KeyCode.E))
        //{
        //    targetYRotation += rotationStep;
        //}

        //Return to default pos
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            targetYRotation = defaultYRotation;
        }

        float currentY = Mathf.LerpAngle(transform.eulerAngles.y, targetYRotation, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Euler(0f, currentY, 0f);

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            Camera.main.orthographicSize -= scroll * 5f;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 2f, 7f);
        }
    }
}

