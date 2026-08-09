using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, Iinteractebole
{
    [SerializeField] private UnityEvent TakenDamage;
    [SerializeField] private float CurrentHealth;
    [SerializeField] private float MaxHealth = 100f;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void Interact()
    {
        CurrentHealth--;

        if (CurrentHealth <= 0)
        {
            TakenDamage.Invoke();
        }
    }
}
