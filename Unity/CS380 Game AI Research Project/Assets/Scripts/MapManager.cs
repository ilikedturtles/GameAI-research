using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapPos
{
    public float cost;
    public float given;

    public int x;
    public int y;
    public int parent_x;
    public int parent_y;

    public static MapPos[] Offsets = new[] {
        new MapPos(-1, 0), //left
        new MapPos( 1, 0), //right
        new MapPos( 0,-1), //down
        new MapPos( 0, 1)  //up
    };

    public MapPos(int a, int b)
    {
        x = a;
        y = b;
        cost = 0;
        parent_x = a;
        parent_y = b;
        given = 0;
    }

    public static MapPos operator +(MapPos other, MapPos other2)
    {
        return new MapPos(other.x + other2.x, other.y + other2.y);
    }
    
}

public class MapData<T>
{
    public int w, h;
    public T[] data;

    // CTOR
    public MapData(int width, int height)
    {
        w = width;
        h = height;
        data = new T[w * h];
    }

    // ACCESSORS
    public T GetPos(int x, int y)
    {
        if (!ValidPos(x, y))
        {
            // error
            Debug.LogError("Invalid Write Index");
        }

        return data[y * w + x];
    }

    public void SetPos(int x, int y, T value)
    {
        if (!ValidPos(x, y))
        {
            // error
            Debug.LogError("Invalid Read Index");
            return;
        }

        data[y * w + x] = value;
    }

    public ref T Pos(int x, int y)
    {
        return ref data[y * w + x];
    }

    public bool ValidPos(int x, int y)
    {
        if (x < 0 || y < 0 || x >= w || y >= h) return false;
        return true;
    }

    public T GetPos(MapPos gPos)
    {
        return GetPos(gPos.x, gPos.y);
    }

    public void SetPos(MapPos gPos, T value)
    {
        SetPos(gPos.x, gPos.y, value);
    }

    public ref T Pos(MapPos gPos)
    {
        return ref Pos(gPos.x, gPos.y);
    }

    public bool ValidPos(MapPos gPos)
    {
        return ValidPos(gPos.x, gPos.y);
    }

    public MapPos IndexMapPos(int index)
    {
        return new(index % w, index / w);
    }
};


public class MapManager : GenericSingletonClass<MapManager>
{
    [SerializeReference]
    public GameObject TilePrefab;

    public Gradient TileFloatGrad;
    public Gradient TileBoolGrad;

    private MapData<GameObject> tiles = null;

    private bool dirty = false;

    public bool Dirty()
    {
        if (dirty)
        {
            dirty = false;
            return true;
        }

        return false;
    }

    struct TileColor
    {
        public MapPos mPos;
        public Color _color;

        public TileColor(MapPos pos, Color color)
        {
            mPos = pos;
            _color = color;
        }
    };

    Stack<TileColor> colorTiles = new();

    // private helpers

    private void PlaceTiles()
    {
        Vector2 TileDims = new(TilePrefab.transform.localScale.x, TilePrefab.transform.localScale.z);

        // instantiate new tiles, apply position offset.
        for (int i = 0; i < tiles.h; ++i)
        {
            for (int j = 0; j < tiles.w; ++j)
            {
                // instantiate prefab
                GameObject obj = Instantiate(TilePrefab);
                obj.transform.position = new Vector3(TileDims.x * j, 0.0f, TileDims.y * i);

                // set MapTile data.
                MapTile mt = obj.GetComponent<MapTile>();
                mt.SetXY((uint)j, (uint)i);

                // add to tiles                    
                tiles.Pos(j, i) = obj;
            }
        }
    }

    private void DestoryTiles()
    {
        for (int i = 0; i < tiles.w * tiles.h; ++i)
        {
            MapPos mPos = tiles.IndexMapPos(i);
            Destroy(tiles.Pos(mPos));
            tiles.Pos(mPos) = null;
        }
    }

    // public functions


    public void SetSize(int width, int height)
    {
        if (tiles == null)
        {
            dirty = true;
            tiles = new(width, height);
            PlaceTiles();
            return;
        }

        if (tiles.w != width || tiles.h != height)
        {
            dirty = true;

            DestoryTiles();
            tiles = new(width, height);
            PlaceTiles();
        }
    }

    public void SetWidth(float width)
    {
        SetSize((int)width, tiles.h);
    }

    public void SetHeight(float height)
    {
        SetSize(tiles.w, (int)height);
    }

    public int GetWidth()
    {
        if (tiles != null) return tiles.w;
        return 0;
    }

    public int GetHeight()
    {
        if (tiles != null) return tiles.h;
        return 0;
    }


    public void WriteTileData(MapData<float> data)
    {
        if (data.h != tiles.h || data.w != tiles.w)
        {
            Debug.LogError("Mismatched MapData Sizes", this);
            return;
        }

        for (int i = 0; i < tiles.w * tiles.h; ++i)
        {
            MapPos mPos = tiles.IndexMapPos(i);

            GameObject tile = tiles.Pos(mPos);

            float value = data.Pos(mPos);

            Renderer renderer = tile.GetComponent<Renderer>();
            
            Color newColor = TileFloatGrad.Evaluate((value + 1.0f) / 2.0f);

            renderer.materials[0].SetColor("_Color", newColor);
        }
    }

    private void LateUpdate()
    {
        while (colorTiles.Count != 0)
        {
            TileColor tColor = colorTiles.Pop();

            GameObject tile = tiles.Pos(tColor.mPos);
            Renderer renderer = tile.GetComponent<Renderer>();
            renderer.materials[0].SetColor("_Color", tColor._color);
        }
    }

    public void WriteTileData(MapData<bool> data)
    {
        if (data.h != tiles.h || data.w != tiles.w)
        {
            Debug.LogError("Mismatched MapData Sizes", this);
            return;
        }

        for (int i = 0; i < tiles.w * tiles.h; ++i)
        {
            MapPos mPos = tiles.IndexMapPos(i);

            GameObject tile = tiles.Pos(mPos);

            bool value = data.Pos(mPos);

            Renderer renderer = tile.GetComponent<Renderer>();

            Color newColor = TileBoolGrad.Evaluate(value ? 1.0f : 0.0f);

            renderer.materials[0].SetColor("_Color", newColor);
        }
    }

    public void WriteColor(MapPos mPos, Color color)
    {
        colorTiles.Push(new TileColor(mPos, color));
    }

    public Vector3 GetTileWorldPos(MapPos mPos)
    {
        return tiles.Pos(mPos).transform.position;
    }
}