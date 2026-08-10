using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteractHandeler : MonoBehaviour
{
    [SerializeField] private float Grabrange = 4f;
    private Iinteractebole Iinteractebole;

    private void Update()
    {
        Debug.DrawRay(transform.position + transform.up * (transform.localScale.y * 0.5f), transform.forward * Grabrange, Color.red);
    }

    public void Interact(InputAction.CallbackContext callbackContext)
    {
        Ray ray = new Ray
        {
            origin = transform.position + transform.up * (transform.localScale.y * 0.5f),
            direction = transform.forward
        };


        if (!Physics.Raycast(ray, out RaycastHit hitInfo, Grabrange)) return;

        Iinteractebole = hitInfo.collider.GetComponent<Iinteractebole>();

        if (Iinteractebole == null) return;

        Iinteractebole.Interact();
    }
}
