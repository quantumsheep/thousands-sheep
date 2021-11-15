using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldLayer
{
    public int SortingOrder;

    public Tilemap Tilemap;

    public WorldLayer(int sortingOrder)
    {
        SortingOrder = sortingOrder;
    }

    public void SetTilemap(Tilemap tilemap)
    {
        Tilemap = tilemap;
    }

    public void SetTile(int x, int y, TileBase tile)
    {
        Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }
}
