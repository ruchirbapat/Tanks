using UnityEngine;

public class Entity : MonoBehaviour
{

    //Initial health for an Entity
    [Range(0, 100)]
    public float initialHealth;

    //Current health for an Entity
    public float currentHealth;

    //If an Entity is alive or not
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
