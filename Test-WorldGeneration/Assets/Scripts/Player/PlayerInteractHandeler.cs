using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractHandeler : MonoBehaviour
{
    [SerializeField] private float Grabrange = 4f;
    private Iinteractebole Iinteractebole;

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * Grabrange);

        if (!Keyboard.current.eKey.wasPressedThisFrame) { return; }

        CastRay();
    }

    private void CastRay()
    {
        Ray ray = new Ray
        {
            origin = transform.position,
            direction = transform.forward
        };

        RaycastHit hitInfo;
        if (!Physics.Raycast(ray, out hitInfo, Grabrange)) { return; }

        Iinteractebole = hitInfo.collider.GetComponent<Iinteractebole>();

        if (Iinteractebole == null) return;

        Iinteractebole.Interact();
    }
}
