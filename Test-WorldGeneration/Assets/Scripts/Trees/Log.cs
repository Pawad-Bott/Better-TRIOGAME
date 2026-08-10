using UnityEngine;
using System.Collections.Generic;

public class Log : MonoBehaviour, Iinteractebole
{
    [SerializeField] private float StartFalingForce;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject ClosestPlayer = GetClosestObject(transform.position, PlayerManager.Instance.Players);

        if (ClosestPlayer == null) return;

        Vector3 Force = ClosestPlayer.transform.forward * StartFalingForce;

        Vector3 TopOfLog = transform.position + transform.up * (transform.localScale.y * 0.5f);

        rb.AddForceAtPosition(-Force, TopOfLog, ForceMode.Impulse);
    }

    GameObject GetClosestObject(Vector3 position, List<GameObject> objects)
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            if (obj == null) continue;

            float distance = Vector3.SqrMagnitude(obj.transform.position - position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj;
            }
        }
        return closest;
    }
    public void Interact()
    {
        // here is the logic for the player grabing the log!
    }
}
