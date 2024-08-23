using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    private Vector3 offset = new Vector3 (0f, 0.5f, -10f);
    private Vector3 zoomOffset = new Vector3(0f, 0.1f, -10f);
    private float smoothTime = 0.1f;
    private Vector3 Velocity = Vector3.zero;
    private Camera mainCamera;

    [SerializeField] private int cameraSize = 5;
    [SerializeField] private Transform sareko;
    [SerializeField] private Transform cameraTransform;

    void Update()
    {
        mainCamera = GetComponent<Camera>();
        Vector3 sarekoGroundPosition = new Vector3(sareko.position.x, 2.5f, -10f);
        Vector3 sarekoPosition = sareko.position + offset;
        Vector3 zommInPosition = sareko.position + zoomOffset;


        if (Movement.isZoomPressed)
        {
            transform.position = Vector3.SmoothDamp(transform.position, zommInPosition, ref Velocity, smoothTime);

            mainCamera.orthographicSize = 1;
        }
        else
        {
            mainCamera.orthographicSize = cameraSize;
        }
        if (sareko.position.y> 3.5 && !Movement.isZoomPressed)
        {
            transform.position = Vector3.SmoothDamp(transform.position, sarekoPosition, ref Velocity, smoothTime);
        }
        else if (sareko.position.y < -3 && !Movement.isZoomPressed)
        {
            transform.position = Vector3.SmoothDamp(transform.position, sarekoPosition, ref Velocity, smoothTime);
        }
        else if ( !Movement.isZoomPressed )
        {
            transform.position = Vector3.SmoothDamp(transform.position, sarekoGroundPosition, ref Velocity, smoothTime);
        }
    }
}
