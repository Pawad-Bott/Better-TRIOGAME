using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerInteractHandeler : MonoBehaviour
{
    [Header("Interact settings")]
    [SerializeField] private float Grabrange = 4f;
    [SerializeField] private float GrabForce = 2f;
    public bool IsGrabing { get; private set; }
    public bool HasInteractable => IInteractebole != null;
    public Vector3 LocalGrabPoint { get; private set; } //LocalGrabPoint är den punkt på objectet som spelaren grabar och den är i localspace till objectet
    public Vector3 WorldGrabPoint { get; private set; }
    public Vector3 HandPoint { get; private set; }
    public Transform GrabObjectTranform { get; private set; }
    private IInteractebole IInteractebole;
    private IDamagebole damagebole;
    private static Animator PlayerAnimatior;
    private static readonly int ChopTreeHash = Animator.StringToHash("ChoppTree");
    public bool IsChoppingAnimationPlaying { get; private set; }
    [SerializeField] private ParticleSystem[] ChopTreeParticals;
    private void Awake()
    {
        PlayerAnimatior = GetComponent<Animator>();
    }

    private void Start()
    {
        IsGrabing = false;
        IsChoppingAnimationPlaying = false;
    }
    private void Update()
    {
        CalculateRay();

        if (!IsGrabing)
        {
            Debug.DrawRay(transform.position + transform.up * transform.localScale.y, transform.forward * Grabrange, Color.red);
            return;
        }
        if (GrabObjectTranform == null)
        {
            IsGrabing = false;
            IInteractebole = null;
            return;
        }

        WorldGrabPoint = GrabObjectTranform.TransformPoint(LocalGrabPoint);

        HandPoint = transform.position + transform.forward * Grabrange + transform.up * transform.localScale.y;

        Debug.DrawLine(transform.position + transform.up * transform.localScale.y, HandPoint, Color.white);
        Debug.DrawLine(HandPoint, WorldGrabPoint, Color.white);

        IInteractebole?.Interact(GrabForce, HandPoint, WorldGrabPoint);
    }

    public void Interact(InputAction.CallbackContext callbackContext)
    {
        bool HasSomthingToInteract = CalculateRay();

        if (callbackContext.started && HasSomthingToInteract) IsGrabing = true;
        if (callbackContext.canceled) IsGrabing = false;

        if (!callbackContext.performed) return;
        if (IsChoppingAnimationPlaying) return;

        if (damagebole == null) return;

        damagebole.TakeDamage(1);

        foreach (ParticleSystem ChoptreePartical in ChopTreeParticals)
        {
            Instantiate(ChoptreePartical, transform.position, quaternion.identity);
        }
    }

    /// <summary>
    /// Draws a ray from the player and chescs for somrging to interact with
    /// </summary>
    /// <returns>true if the player has somthing in range to grab</returns>
    private bool CalculateRay()
    {
        if (IsGrabing)
        {
            return IInteractebole != null;
        }
        Ray ray = new Ray
        {
            origin = transform.position + transform.up * transform.localScale.y,
            direction = transform.forward
        };

        if (!Physics.Raycast(ray, out RaycastHit hitInfo, Grabrange))
        {
            IInteractebole = null;
            damagebole = null;
            return false;
        }
        // Everyting under here is where the ray hitts something

        IInteractebole = hitInfo.collider.GetComponent<IInteractebole>();
        damagebole = hitInfo.collider.GetComponent<IDamagebole>();

        if (IInteractebole == null && damagebole == null) return false;

        GrabObjectTranform = hitInfo.transform;
        LocalGrabPoint = GrabObjectTranform.transform.InverseTransformPoint(hitInfo.point);

        return true;
    }

    public void FinishChopping()
    {
        IsChoppingAnimationPlaying = false;
        PlayerAnimatior.SetBool(ChopTreeHash, false);
    }
    public void StartChopping()
    {
        IsChoppingAnimationPlaying = true;
        PlayerAnimatior.SetBool(ChopTreeHash, true);
    }
}
