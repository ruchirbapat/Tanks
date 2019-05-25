using UnityEngine;

public class Entity : MonoBehaviour
{
    const float initialHealth = 100;

    //Current health for an Entity
    public float currentHealth = 100;

    //If an Entity is alive or not
    [System.NonSerialized]
    public bool dead;

    //Event for when the Entity dies (Player and Enemy are subscribed)
    public event System.Action OnDeath;

    //Set the current health equal to the initial health
    protected virtual void Start()
    {
        currentHealth = initialHealth;
    }

    //Damages an Entity
    public virtual void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, initialHealth);
        if (currentHealth <= 0 && !dead)
            Die();// dead = false;
    }

    public virtual void TakeHit(float damageAmount, Vector3 point, Vector3 direction)
    {
        Damage(damageAmount);
    }

    //Kills an Entity
    public virtual void Die()
    {
        dead = true;
        if (OnDeath != null) { OnDeath(); };
        Destroy(gameObject);
    }
}
