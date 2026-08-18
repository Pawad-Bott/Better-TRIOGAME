using UnityEngine;

public class ChopingDownTree : MonoBehaviour
{
    [SerializeField] private GameObject TreeStump;
    [SerializeField] private GameObject Log;
    public void ChopdingDownTree()
    {
        var DeadTreeParent = new GameObject("DeadTree");

        Vector3 LogPosition = transform.position;
        LogPosition.y = 0;

        Instantiate(TreeStump, LogPosition, Quaternion.Euler(0, 0, 0), DeadTreeParent.transform);
        Instantiate(Log, transform.position + transform.up, Quaternion.Euler(0, 0, 0), DeadTreeParent.transform);

        Destroy(gameObject);
    }
}
