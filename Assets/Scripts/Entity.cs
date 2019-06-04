// Dependencies
using UnityEngine;

// Parent class for Player and Enemy
public class Entity : MonoBehaviour
{
    // Initial health
    const float initialHealth = 100;

    //Current health for an Entity
    public float currentHealth = 100;

    //If an Entity is alive or not
    [System.NonSerialized]
    public bool dead;

    //Event for when the Entity dies (Player and Enemy are subscribed)
    public event System.Action OnDeath;

    public ParticleSystem deathEffect;

    protected virtual void Start()
    {
        // Initially set the current health to max
        currentHealth = initialHealth;
    }

    // Deals damage to an Entity (lowers remaining health and/or calls Die())
    public virtual void Damage(float damageAmount, Vector3 hitDirection)
    {
        // Reduce health
        currentHealth -= damageAmount;
       
        // Clamp health between 0 and 100 because there cannot be a negative health
        currentHealth = Mathf.Clamp(currentHealth, 0, initialHealth);

        // If the health is less than 0, then call Die()
        if (currentHealth <= 0 && !dead)
            Die(hitDirection);// dead = false;
    }

    // Wrapper function for Damage()
    public virtual void TakeHit(float damageAmount, Vector3 point, Vector3 direction)
    {
        Damage(damageAmount, direction);
    }

    // Destroys an Entity and creates a death particle effect VFX
    public virtual void Die(Vector3 hitDirection)
    {
        // Call the subscribed event, if it exists
        if (OnDeath != null) { OnDeath(); };

        // Instantiates a death effect (explosion effect) and destroys it after a fixed 2 seconds
        Destroy(Instantiate(deathEffect, transform.position, Quaternion.FromToRotation(Vector3.forward, hitDirection)), 2);

        // Destroy the Entity and let the garbage collector free resources
        Destroy(gameObject);
    }
}
