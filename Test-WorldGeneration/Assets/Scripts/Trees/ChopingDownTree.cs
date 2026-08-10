using System.Collections.Generic;
using UnityEngine;

public class ChopingDownTree : MonoBehaviour
{
    [SerializeField] private GameObject TreeStump;
    [SerializeField] private GameObject Log;
    [SerializeField] private Transform TreeStumpPosition;
    public void ChopdingDownTree()
    {
        Instantiate(TreeStump, TreeStumpPosition.position, Quaternion.Euler(0, 0, 0));
        Instantiate(Log, transform.position + transform.up, Quaternion.Euler(0, 0, 0));

        Destroy(gameObject);
    }
}
