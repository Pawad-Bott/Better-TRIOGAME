using UnityEngine;

public class AddCameraChake : MonoBehaviour
{
    private Camera camera;
    private CameraHolder cameraHolder;
    private void Awake()
    {
        camera = gameObject.GetComponent<Camera>();
        cameraHolder = camera.GetComponentInParent<CameraHolder>();
        enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled || collision.collider == null) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            cameraHolder.ChakeCamera();
            enabled = false;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            //stun the player here
        }

    }
}
