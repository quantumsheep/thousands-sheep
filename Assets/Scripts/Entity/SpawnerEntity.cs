using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpawnerEntity : Entity
{
    public float SpawnsPerSecond = 0.5f;
    public float SpawnRange = 10f;

    public GameObject EntityPrefab;

    [MinMaxSlider(0, 100)]
    public Vector2Int Level = new Vector2Int(1, 10);

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

        var entity = spawn.GetComponent<Entity>();
        entity.RandomizeAttributes(Level.x, Level.y);
    }
}
