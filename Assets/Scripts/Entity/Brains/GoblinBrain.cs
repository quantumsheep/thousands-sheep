using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GoblinBrain : HostileEntity
{
    public float Speed = 10.0f;

    public float DetectionRange = 30f;
    public float AttackRange = 1f;
    [Layer]
    public List<int> TargetLayerMaskIndexes;

    public float AttackDamage = 10f;
    public float AttacksPerSecond = 0.5f;
    public float Knockback = 1.0f;

    private int _targetLayerMask = 0;
    public Entity _target = null;

    private float _lastAttackTime = 0.0f;

    public override void Awake()
    {
        base.Awake();

        foreach (var index in TargetLayerMaskIndexes)
        {
            _targetLayerMask |= 1 << index;
        }
    }

    public override void Update()
    {
        base.Update();

        if (_target == null)
        {
            _target = FindTarget();
        }
        else if (Vector3.Distance(transform.position, _target.transform.position) > DetectionRange)
        {
            _target = null;
        }

        if (_target)
        {
            var direction = (_target.transform.position - transform.position).normalized;
            if (Vector3.Distance(transform.position, _target.transform.position) > AttackRange)
            {
                var velocity = direction * Speed;
                _rigidbody.velocity = velocity;
            }
            else if (AttacksPerSecond >= 0.0f && ((Time.time - _lastAttackTime) > (1.0f / AttacksPerSecond)))
            {
                _lastAttackTime = Time.time;
                _target.TakeDamage(AttackDamage, direction * Knockback);
            }
        }
    }

    private Entity FindTarget()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, DetectionRange, _targetLayerMask);

        Collider2D closestCollider = null;
        float closestDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            var distance = Vector2.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                closestCollider = collider;
                closestDistance = distance;
            }
        }

        return closestCollider?.GetComponent<Entity>();
    }
}
