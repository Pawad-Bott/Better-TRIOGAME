using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IInteractebole
{
    [SerializeField] private float MaxHealth = 100f;
    [SerializeField] private UnityEvent Dead;
    [SerializeField] private UnityEvent TakenDamage;
    private float CurrentHealth;
    private void Start()
    {
        CurrentHealth = MaxHealth;
    }
    public void Interact(float Grabforce, Vector3 targetPosition, Vector3 InteractPoint)
    {
    }
    public void TakeDamage(int Amt)
    {
        CurrentHealth -= Amt;

        if (CurrentHealth <= 0)
        {
            Dead.Invoke();
        }
        else
        {
            TakenDamage.Invoke();
        }
    }
}
