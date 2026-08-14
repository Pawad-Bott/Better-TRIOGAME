using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ForceGrabLogic : MonoBehaviour, IInteractebole
{
    public Rigidbody rb { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Interact(float Grabforce, Vector3 targetPosition, Vector3 InteractPoint)
    {
        Vector3 direction = targetPosition - InteractPoint;
        Vector3 force = direction * Grabforce;

        rb.AddForceAtPosition(force, InteractPoint, ForceMode.Force);
    }
}
