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
    public float BaseAttackDamages = 0.0f;
    public float BaseAttackRange = 0.0f;

    public GameObject ExplosionPrefab;

    public GameObject ProjectilePrefab;
    public Collider2D PlayerCollider;

    public bool IsShooting = false;
    public Vector2 TargetDirection;

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
        if (IsShooting && AttacksPerSecond >= 0.0f && ((Time.time - _lastAttackTime) > (1.0f / AttacksPerSecond)))
        {
            _lastAttackTime = Time.time;

            Shoot(TargetDirection);
        }
    }

    public void SetIsShooting(bool isShooting, Vector2 targetDirection)
    {
        IsShooting = isShooting;
        TargetDirection = targetDirection;
    }

    private void Shoot(Vector2 direction)
    {
        // var rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Instantiate(ExplosionPrefab, transform.position, Quaternion.AngleAxis(rotationAngle - 90f, Vector3.forward));

        var projectileGameObject = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        var projectile = projectileGameObject.GetComponent<ProjectileEntity>();

        projectile.Damage += BaseAttackDamages;
        projectile.LifeTime += BaseAttackRange;
        projectile.SetDirection(direction + new Vector2(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f)));
        projectile.IgnoreCollider(PlayerCollider);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.1f);
        Gizmos.DrawSphere(transform.position, DetectionRange);
    }
}
