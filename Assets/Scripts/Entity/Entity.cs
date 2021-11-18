using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    public int Lvl = 0;
    public string Name = "";
    public string Title = "";

    public EntityIdentity Identity;

    public float MaxHealth = 100f;
    public bool IsCurrentHealthMaxHealth = true;
    [DisableIf("IsCurrentHealthMaxHealth")]
    public float CurrentHealth;
    public EntityHealthbar Healthbar;

    protected Rigidbody2D _rigidbody;

    public virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        CurrentHealth = MaxHealth;
    }

    public virtual void Start()
    {
        if (Identity != null)
        {
            Identity.UpdateIdentity(Lvl, Name, Title);
        }
    }

    public virtual void Update()
    {
        if (CurrentHealth <= 0)
        {
            Die();
        }

        if (Healthbar != null)
        {
            Healthbar.UpdateHealth(CurrentHealth, MaxHealth);
        }
    }
    public virtual void TakeDamage(float damage, Vector2 knockback)
    {
        CurrentHealth -= damage;

        if (knockback != Vector2.zero)
        {
            _rigidbody.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void RandomizeAttributes(int minLevel, int maxLevel) { }
}
