using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class PlayerInteractHandeler : MonoBehaviour
{
    [Header("Interact settings")]
    [SerializeField] private float Grabrange = 4f;
    [SerializeField] private float GrabForce = 2f;
    [SerializeField] private float SphereCastSize = 1f;
    public bool IsGrabing { get; private set; }
    public Vector3 LocalGrabPoint { get; private set; } //LocalGrabPoint är den punkt på objectet som spelaren grabar och den är i localspace till objectet
    public Vector3 WorldGrabPoint { get; private set; }
    public Vector3 HandPoint { get; private set; }
    public Transform GrabObjectTranform { get; private set; }
    public bool HasInteractable => IInteractebole != null;
    private IInteractebole IInteractebole;
    private IDamagebole damagebole;
    private static Animator PlayerAnimatior;
    private static readonly int ChopTreeHash = Animator.StringToHash("ChoppTree");
    public bool IsChoppingAnimationPlaying { get; private set; }
    [Header("Particals")]
    [SerializeField] private ParticleSystem ChopTreeParticals;
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
        //CalculateRay();

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

        // it's neded to Convert The localGrabpoint to worldSpace so the point folows the object we wanna grab
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
        SpawnChoppTreeParticals();
    }
    private void SpawnChoppTreeParticals()
    {
        Quaternion rotation = transform.rotation * Quaternion.Euler(-90f, 0f, 0f);
        Vector3 position = transform.position + transform.up * transform.localScale.y + transform.forward * 0.5f;

        Instantiate(ChopTreeParticals, position, rotation);
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
        if (!Physics.SphereCast(ray, SphereCastSize, out RaycastHit hitInfo, Grabrange))
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

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + transform.up * transform.localScale.y;
        Vector3 direction = transform.forward;
        Vector3 end = origin + direction * Grabrange;

        Gizmos.color = HasInteractable ? Color.green : Color.red;

        Gizmos.DrawWireSphere(origin, SphereCastSize);
        Gizmos.DrawWireSphere(end, SphereCastSize);

        Gizmos.DrawLine(
            origin + transform.right * SphereCastSize,
            end + transform.right * SphereCastSize
        );

        Gizmos.DrawLine(
            origin - transform.right * SphereCastSize,
            end - transform.right * SphereCastSize
        );

        Gizmos.DrawLine(
            origin + transform.up * SphereCastSize,
            end + transform.up * SphereCastSize
        );

        Gizmos.DrawLine(
            origin - transform.up * SphereCastSize,
            end - transform.up * SphereCastSize
        );
    }
}
