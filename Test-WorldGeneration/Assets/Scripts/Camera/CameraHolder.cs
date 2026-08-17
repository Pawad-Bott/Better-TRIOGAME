using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public List<GameObject> Targets { get; private set; }
    [Header("Offset from the player")]
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    [Header("Camera Smooth Settings")]
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomLimiter;
    [SerializeField] private float camSmothnes;
    private Camera Camera;
    private void Start()
    {
        Camera = Camera.main;
        Targets = PlayerManager.Instance.Players;
    }
    void LateUpdate()
    {
        if (Targets.Count == 0 || Targets == null) return;

        Vector3 centerPoint = GetCenterPoint();
        transform.position = Vector3.SmoothDamp(transform.position, centerPoint + offset, ref velocity, camSmothnes);

        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistans() / zoomLimiter);
        Camera.orthographicSize = Mathf.Lerp(Camera.orthographicSize, newZoom, Time.deltaTime);
    }
    float GetGreatestDistans()
    {
        var bounds = new Bounds(Targets[0].transform.position, Vector3.zero);

        for (int i = 0; i < Targets.Count; i++)
        {
            bounds.Encapsulate(Targets[i].transform.position);
        }
        return bounds.size.x;
    }
    Vector3 GetCenterPoint()
    {
        if (Targets.Count == 1) return Targets[0].transform.position;

        var bounds = new Bounds(Targets[0].transform.position, Vector3.zero);

        for (int i = 0; i < Targets.Count; i++) bounds.Encapsulate(Targets[i].transform.position);

        return bounds.center;
    }

    public void ChakeCamera(float CameraChakeStrengt = 0.1f, float CameraChakeDuration = 0.1f)
    {
        StartCoroutine(Shake(CameraChakeDuration, CameraChakeStrengt));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }
}
