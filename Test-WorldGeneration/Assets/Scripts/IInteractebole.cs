using UnityEngine;
public interface IInteractebole
{
    void Interact(float Grabforce, Vector3 targetPosition, Vector3 InteractPoint);

    void TakeDamage(int DamageAmt);
}