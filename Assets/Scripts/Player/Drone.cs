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

    public bool IsShooting = false;
    public Vector3 TargetDirection;

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

    public void SetIsShooting(bool isShooting, Vector3 targetDirection)
    {
        IsShooting = isShooting;
        TargetDirection = targetDirection;
    }

    private void Shoot(Vector3 direction)
    {
        // var rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Instantiate(ExplosionPrefab, transform.position, Quaternion.AngleAxis(rotationAngle - 90f, Vector3.forward));

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
