using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Waterforce : MonoBehaviour
{
    [SerializeField] private float WaterPuchForce;
    private Rigidbody rb;
    private Vector3 WaterDirection;
    private bool ShuldApplyForce = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Waterbottom")) return;

        WaterDirection = collision.transform.forward;
        ShuldApplyForce = true;

    }
    public void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Waterbottom")) return;

        ShuldApplyForce = false;
    }
    private void FixedUpdate()
    {
        if (!ShuldApplyForce) return;

        Vector3 Force = WaterDirection * WaterPuchForce;
        rb.AddForce(Force, ForceMode.Force);
    }
}
