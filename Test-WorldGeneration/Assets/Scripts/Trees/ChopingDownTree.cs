using Unity.VisualScripting;
using UnityEngine;

public class ChopingDownTree : MonoBehaviour
{
    [SerializeField] private GameObject TreeStump;
    [SerializeField] private GameObject Log;
    [SerializeField] private Transform TreeStumpPosition;
    public void ChopdingDownTree()
    {
        var DeadTreeParent = new GameObject("DeadTree");

        Instantiate(TreeStump, TreeStumpPosition.position, Quaternion.Euler(0, 0, 0), DeadTreeParent.transform);
        Instantiate(Log, transform.position + transform.up, Quaternion.Euler(0, 0, 0), DeadTreeParent.transform);

        Destroy(gameObject);
    }
}
