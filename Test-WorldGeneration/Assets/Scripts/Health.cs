using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamagebole
{
    [Header("HealthSettings")]
    [SerializeField] private float MaxHealth = 100f;
    [Header("HealtRegen")]
    [SerializeField] private float HealthRegen = 0.1f;
    [SerializeField] private float HealtRegenInterval = 0.5f;
    [Header("Events")]
    [SerializeField] private UnityEvent Dead;
    [SerializeField] private UnityEvent TakenDamage;
    private float CurrentHealth;
    private void Start()
    {
        CurrentHealth = MaxHealth;
        InvokeRepeating(nameof(HealtRegen), 0, HealtRegenInterval);
    }
    private void HealtRegen()
    {
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += HealthRegen;
        }
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }
    public void TakeDamage(int Amt)
    {
        CurrentHealth -= Amt;

        if (CurrentHealth > 0)
        {
            TakenDamage.Invoke();
        }
        else
        {
            TakenDamage.Invoke();
            Dead.Invoke();
        }
    }
}
