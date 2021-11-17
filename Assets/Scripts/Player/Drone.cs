using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public float DetectionRange = 4.0f;
    [Layer]
    public List<int> DetectionLayerMaskIndexes;

    public float AttacksPerSecond = 1.0f;

    public GameObject ExplosionPrefab;

    public GameObject ProjectilePrefab;
    public Collider2D PlayerCollider;

    private int _detectionLayerMask = 0;
    private float _lastAttackTime = 0.0f;

    void Start()
    {
        foreach (var index in DetectionLayerMaskIndexes)
        {
            _detectionLayerMask |= 1 << index;
        }
    }

    void Update()
    {
        if (AttacksPerSecond >= 0.0f && ((Time.time - _lastAttackTime) > (1.0f / AttacksPerSecond)))
        {
            _lastAttackTime = Time.time;

            var colliders = Physics2D.OverlapCircleAll(transform.position, DetectionRange, _detectionLayerMask);

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

            if (closestCollider != null)
            {
                Shoot(closestCollider.transform.position);
            }
        }
    }

    private void Shoot(Vector3 targetPosition)
    {
        var direction = (targetPosition - transform.position).normalized;

        var rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Instantiate(ExplosionPrefab, transform.position, Quaternion.AngleAxis(rotationAngle - 90f, Vector3.forward));

        var projectileGameObject = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        var projectile = projectileGameObject.GetComponent<ProjectileEntity>();

        projectile.SetDirection(direction);
        projectile.IgnoreCollider(PlayerCollider);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.1f);
        Gizmos.DrawSphere(transform.position, DetectionRange);
    }
}
