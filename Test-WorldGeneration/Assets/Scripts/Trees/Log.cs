using UnityEngine;
using System.Collections.Generic;

public class Log : ForceGrabLogic
{
    [SerializeField] private float StartFalingForce;
    void Start()
    {
        //this code make is to when a log spawns in the log fals towards the player
        GameObject ClosestPlayer = GetClosestObject(transform.position, PlayerManager.Instance.Players);
        if (ClosestPlayer == null) return;
        Vector3 Force = ClosestPlayer.transform.forward * StartFalingForce;
        Vector3 TopOfLog = transform.position + transform.up * (transform.localScale.y * 0.5f);

        //rb is from ForceGrabLogic
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
}
