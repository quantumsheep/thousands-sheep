using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEntity : Entity
{
    public float SpawnsPerSecond = 0.5f;
    public float SpawnRange = 10f;

    public GameObject EntityPrefab;

    private float _lastSpawnTime = 0f;

    public override void Update()
    {
        base.Update();

        if (SpawnsPerSecond > 0.0f && ((Time.time - _lastSpawnTime) > (1f / SpawnsPerSecond)))
        {
            _lastSpawnTime = Time.time;
            Spawn();
        }
    }

    private void Spawn()
    {
        var spawnPosition = transform.position + Random.insideUnitSphere * SpawnRange;
        var spawn = Instantiate(EntityPrefab, spawnPosition, Quaternion.identity);
    }
}
