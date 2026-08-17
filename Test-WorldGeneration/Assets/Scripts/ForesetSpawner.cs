using System.Collections.Generic;
using UnityEngine;
public class TreeSpawner : MonoBehaviour
{
    [SerializeField] private bool draGismos = false;
    [Header("Spawn Settings")]
    public float SpawnNothingChance = 30f;
    [Space]
    public SpawnType[] SpawnTypes;
    [Space]
    public BoxCollider SpawnArea;
    public LayerMask Ground;
    private List<Vector2> points = new();
    private List<SpawnedObject> spawnedObjects = new();
    void Start()
    {
        // Small radius = many possible spawn locations.
        points = PoissonDiskSampling.GeneratePoints(0.5f, new Vector2(SpawnArea.size.x, SpawnArea.size.z)); // here are all the spawnpoints chosen

        TrySpawnAssets();
    }
    void TrySpawnAssets()
    {
        // Sortera från störst radie till minst.
        List<SpawnType> sortedTypes = new List<SpawnType>(SpawnTypes);

        sortedTypes.Sort((a, b) => b.SpawnRadius.CompareTo(a.SpawnRadius));

        foreach (Vector2 point in points)
        {
            // Total chans för alla SpawnTypes
            float totalSpawnChance = 0f;

            foreach (SpawnType type in sortedTypes)
            {
                totalSpawnChance += type.SpawnChance;
            }

            // Nothing + alla SpawnTypes
            float totalChance = SpawnNothingChance + totalSpawnChance;

            float randomValue = Random.Range(0f, totalChance);

            // Om slumpen hamnar inom Nothing-chansen
            if (randomValue < SpawnNothingChance)
                continue;

            // Ta bort Nothing från random-värdet
            randomValue -= SpawnNothingChance;

            // Hitta vilken SpawnType som valdes
            foreach (SpawnType type in sortedTypes)
            {
                if (randomValue < type.SpawnChance)
                {
                    TrySpawn(point, type);
                    break;
                }

                randomValue -= type.SpawnChance;
            }
        }
    }
    void TrySpawn(Vector2 point, SpawnType type)
    {
        Vector3 localPos = new Vector3(point.x - SpawnArea.size.x / 2f, 0, point.y - SpawnArea.size.z / 2f);

        Vector3 worldPos = SpawnArea.transform.TransformPoint(localPos);

        Vector3 rayStart = worldPos + Vector3.up * 50f;

        if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f, Ground)) return;

        // Check against every object that has already spawned.
        foreach (SpawnedObject obj in spawnedObjects)
        {
            float distance = Vector3.Distance(hit.point, obj.Position);

            if (distance < type.SpawnRadius + obj.Radius) return;
        }

        GameObject prefab = type.Prefabs[Random.Range(0, type.Prefabs.Length)];

        //here we calculate the bottom of the mesh and spawn the object at that point so it doesn't go thrue the ground!
        GameObject spawned = Instantiate(prefab, hit.point, prefab.transform.rotation, transform);
        Bounds bounds = GetBounds(spawned);
        float bottomOffset = hit.point.y - bounds.min.y;
        spawned.transform.position += Vector3.up * bottomOffset;


        spawnedObjects.Add(new SpawnedObject() { Position = hit.point, Radius = type.SpawnRadius });
    }
    Bounds GetBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return new Bounds(obj.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds;
    }
    private void OnDrawGizmos()
    {
        if (!draGismos) return;

        if (points == null || SpawnArea == null) return;

        Gizmos.color = Color.red;

        foreach (Vector2 point in points)
        {
            Vector3 localPos = new Vector3(point.x - SpawnArea.size.x / 2f, 0, point.y - SpawnArea.size.z / 2f);

            Vector3 worldPos = SpawnArea.transform.TransformPoint(localPos);

            Gizmos.DrawSphere(worldPos, 0.1f);
        }
    }
}
[System.Serializable]
public class SpawnType
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public GameObject[] Prefabs { get; private set; }
    [field: SerializeField] public float SpawnRadius { get; private set; }
    [Range(0, 100)]
    [field: SerializeField] public float SpawnChance { get; private set; }
}
public class SpawnedObject
{
    public Vector3 Position;
    public float Radius;
}
public static class PoissonDiskSampling
{
    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int rejectionSamples = 30)
    {
        float cellSize = radius / Mathf.Sqrt(2);

        int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];

        List<Vector2> points = new();
        List<Vector2> spawnPoints = new();

        spawnPoints.Add(sampleRegionSize / 2);

        while (spawnPoints.Count > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCentre = spawnPoints[spawnIndex];

            bool accepted = false;

            for (int i = 0; i < rejectionSamples; i++)
            {
                float angle = Random.value * Mathf.PI * 2;

                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

                Vector2 candidate = spawnCentre + dir * Random.Range(radius, radius * 2);

                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);

                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;

                    accepted = true;
                    break;
                }
            }
            if (!accepted) spawnPoints.RemoveAt(spawnIndex);
        }
        return points;
    }

    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
    {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
        {
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);

            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;

                    if (pointIndex != -1)
                    {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;

                        if (sqrDst < radius * radius)
                            return false;
                    }
                }
            }
            return true;
        }
        return false;
    }
}