using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using NaughtyAttributes;
using Unity.Mathematics;

public enum WorldLayerType
{
    Ground,
    Path,
    Vegetation,
    Rocks,
}

public class WorldGeneration : MonoBehaviour
{
    public int ChunkSize = 16;
    public float Seed = 2343423f;

    public GameObject TilemapPrefab;

    public List<Tile> GroundTiles;
    public List<Tile> PathTiles;
    public List<Tile> VegetationTiles;
    public List<Tile> RocksTiles;

    public int PathOctaves = 8;
    public float PathScale = 0.05f;
    public float PathAmplitude = 5.0f;

    private Dictionary<WorldLayerType, WorldLayer> _layers = new Dictionary<WorldLayerType, WorldLayer> {
        { WorldLayerType.Ground, new WorldLayer(0) },
        { WorldLayerType.Path, new WorldLayer(1) },
        { WorldLayerType.Vegetation, new WorldLayer(2) },
        { WorldLayerType.Rocks, new WorldLayer(2) },
    };

    void Start()
    {
        foreach (var layer in _layers)
        {
            var layerData = layer.Value;

            var tilemapGameObject = Instantiate(TilemapPrefab, transform);
            tilemapGameObject.name = layer.Key.ToString();

            var tilemapRenderer = tilemapGameObject.GetComponent<TilemapRenderer>();
            tilemapRenderer.sortingOrder = layerData.SortingOrder;

            var tilemap = tilemapGameObject.GetComponent<Tilemap>();
            layerData.SetTilemap(tilemap);
        }

        GenerateWorld();
    }

    void Update()
    {

    }

    [Button]
    private void GenerateWorld()
    {
        foreach (var layer in _layers)
        {
            layer.Value.Tilemap.ClearAllTiles();
        }

        GenerateChunk(0, 0);
        GenerateChunk(-1, 0);
        GenerateChunk(0, -1);
        GenerateChunk(-1, -1);
    }

    private void GenerateChunk(int chunkX, int chunkY)
    {
        int startX = chunkX * ChunkSize;
        int startY = chunkY * ChunkSize;

        for (int x = 0; x < ChunkSize; x++)
        {
            for (int y = 0; y < ChunkSize; y++)
            {
                int worldX = startX + x;
                int worldY = startY + y;

                var tile = GroundTiles[UnityEngine.Random.Range(0, GroundTiles.Count)];
                _layers[WorldLayerType.Ground].SetTile(worldX, worldY, tile);

                if (RidgedPerlin(worldX, worldY, PathScale, PathOctaves, PathAmplitude) > 0.5f)
                {
                    tile = PathTiles[UnityEngine.Random.Range(0, PathTiles.Count)];
                    _layers[WorldLayerType.Path].SetTile(worldX, worldY, tile);
                }
                else
                {
                    if (UnityEngine.Random.Range(0, 100) < 10)
                    {
                        tile = VegetationTiles[UnityEngine.Random.Range(0, VegetationTiles.Count)];
                        _layers[WorldLayerType.Vegetation].SetTile(worldX, worldY, tile);
                    }
                    else if (UnityEngine.Random.Range(0, 300) < 10)
                    {
                        tile = RocksTiles[UnityEngine.Random.Range(0, RocksTiles.Count)];
                        _layers[WorldLayerType.Rocks].SetTile(worldX, worldY, tile);
                    }
                }
            }
        }
    }

    private float RidgedPerlin(float x, float y, float scale, int octaves, float amplitude)
    {
        float f = amplitude * Mathf.PerlinNoise((x + Seed) * scale, (y + Seed) * scale);
        return math.abs((f - 0.5f) * 2f);
    }
}
