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

    public ParticleSystem deathEffect;
    private float deathEffectDestroyTime;

    //Set the current health equal to the initial health
    protected virtual void Start()
    {
        //deathEffect.GetComponent<ParticleSystemRenderer>().material.color = gameObject.GetComponent<MeshRenderer>().materials[0].color;
        deathEffectDestroyTime = deathEffect.main.startLifetime.Evaluate(0);
        currentHealth = initialHealth;
    }

    //Damages an Entity
    public virtual void Damage(float damageAmount, Vector3 hitDirection)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, initialHealth);
        if (currentHealth <= 0 && !dead)
            Die(hitDirection);// dead = false;
    }

    public virtual void TakeHit(float damageAmount, Vector3 point, Vector3 direction)
    {
        Damage(damageAmount, direction);
    }

    //Kills an Entity
    public virtual void Die(Vector3 hitDirection)
    {
        if (OnDeath != null) { OnDeath(); };
        Destroy(Instantiate(deathEffect, transform.position, Quaternion.FromToRotation(Vector3.forward, hitDirection)), deathEffectDestroyTime);
        Destroy(gameObject);
    }
}
