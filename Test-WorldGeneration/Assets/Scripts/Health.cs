using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, Iinteractebole
{
    [SerializeField] private float MaxHealth = 100f;
    [SerializeField] private UnityEvent TakenDamage;
    private float CurrentHealth;
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
