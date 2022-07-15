using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : GenericSingletonClass<MapManager>
{
    [SerializeReference]
    public GameObject TilePrefab;

    public List<List<GameObject>> tiles = null;

    private void DestroyTiles()
    {
        // free previous grid
        for (int i = 0; i < tiles.Count; ++i)
        {
            for (int j = 0; j < tiles[i].Count; ++j)
            {
                Destroy(tiles[i][j]);
            }
            tiles[i].Clear();
        }

        tiles.Clear();
    }

    private void PlaceTiles(uint width, uint height)
    {
        Vector2 TileDims = new Vector2(TilePrefab.transform.localScale.x, TilePrefab.transform.localScale.z);

        // instantiate new tiles, apply position offset.
        for (int i = 0; i < height; ++i)
        {
            // establish new row
            tiles.Add(new List<GameObject>());
            for (int j = 0; j < width; ++j)
            {
                // instantiate prefab
                GameObject obj = Instantiate(TilePrefab);
                obj.transform.position = new Vector3(TileDims.y * i, 0.0f, TileDims.x * j);

                // set MapTile data.
                MapTile mt = obj.GetComponent<MapTile>();
                mt.SetXY((uint)j, (uint)i);

                // VALUE RANDOMIZER FOR TEMPORARY USE
                //mt.SetValue(Random.Range(-1.0f, 1.0f));

                // add to tiles                    
                tiles[i].Add(obj);
            }
        }
    }

    private void Alloc(uint width, uint height)
    {
        if (tiles == null)
        {
            tiles = new List<List<GameObject>>();
        }
        else
        {
            DestroyTiles();
        }

        PlaceTiles(width, height);
    }

    public void SetSize(uint width, uint height)
    {
        if (tiles == null)
        {
            Alloc(width, height);
            return;
        }

        // reallocate to fit new size.
        if (height != tiles.Count && width != tiles[0].Count)
        {
            Alloc(width, height);
        }
    }

    public void WriteTileData(MapGenSys.Data<float> data)
    {
        if (data.h > tiles.Count ||
            data.w > tiles[0].Count)
        {
            SetSize((uint)data.w, (uint)data.h);
        }

        for (int i = 0; i < data.h; ++i)
        {
            for (int j = 0; j < data.w; ++j)
            {
                MapTile mt = tiles[i][j].GetComponent<MapTile>();
                mt.SetValue(data.GetPos(j,i));
            }
        }
    }

    public void WriteTileData(MapGenSys.Data<bool> data)
    {
        if (data.h > tiles.Count ||
            data.w > tiles[0].Count)
        {
            SetSize((uint)data.w, (uint)data.h);
        }

        for (int i = 0; i < data.h; ++i)
        {
            for (int j = 0; j < data.w; ++j)
            {
                MapTile mt = tiles[i][j].GetComponent<MapTile>();
                mt.SetValue(data.GetPos(j, i) ? 1.0f : 0.0f);
            }
        }
    }
}
