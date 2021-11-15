using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileEntity : Entity
{
    public float Speed = 10.0f;
    public float Damage = 10.0f;
    public float LifeTime = 5.0f;
    public float Knockback = 1.0f;

    private float _lifeTimer = 0.0f;

    private Collider2D _collider;

    public override void Awake()
    {
        base.Awake();

        _collider = GetComponent<Collider2D>();
    }

    public override void Update()
    {
        base.Update();

        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= LifeTime)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Entity>(out var entity))
        {
            var knockbackDirection = (other.transform.position - transform.position).normalized;

            entity.TakeDamage(Damage, knockbackDirection * Knockback);
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        _rigidbody.velocity = direction * Speed;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void IgnoreCollider(Collider2D collider)
    {
        Physics2D.IgnoreCollision(collider, _collider);
    }
}
