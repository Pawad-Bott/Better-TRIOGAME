using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    public List<GameObject> Targets { get; set; }
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
        Camera = GetComponentInChildren<Camera>();
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
}
