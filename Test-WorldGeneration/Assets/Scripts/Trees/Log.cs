using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Log : MonoBehaviour
{
    private Rigidbody rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        GameObject ClosestPlayer = GetClosestObject(transform.position, PlayerManager.Instance.Players);

        Vector3 Force = ClosestPlayer.transform.forward;

        rb.AddForceAtPosition(-Force, transform.up * 2, ForceMode.Force);
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
}
