using UnityEngine;

[RequireComponent(typeof(Health))]
public class TreeAnimatior : MonoBehaviour
{
    [SerializeField] private Mesh[] TreeChopStages;
    private MeshFilter meshFilter;
    private Health health;
    private float thismaxhealth;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        health = GetComponent<Health>();
        thismaxhealth = health.MaxHealth;
    }

    public void AnimateTree(float amt)
    {
        amt++;
        int index = Mathf.Clamp((int)(amt / thismaxhealth * TreeChopStages.Length), 0, TreeChopStages.Length - 1);
        Debug.Log(index);
        meshFilter.sharedMesh = TreeChopStages[index];
    }
}