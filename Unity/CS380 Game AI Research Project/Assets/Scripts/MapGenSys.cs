using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenSys : MonoBehaviour
{
    public struct GridPos
    {
        public int x;
        public int y;

        public GridPos(int a, int b)
        {
            x = a;
            y = b;
        }

        public static GridPos operator +(GridPos other, GridPos other2) => new GridPos(other.x + other2.x, other.y + other2.y);
    }

    static public GridPos[] offsets = new[] {
        new GridPos(-1, 0), //left
        new GridPos( 1, 0), //right
        new GridPos( 0,-1), //down
        new GridPos( 0, 1)  //up
    };

    //public void FindEnds(ref Data<bool> map, out GridPos start, out GridPos end)
    //{

    //}

    [Range(-1.0f, 1.0f)]
    public float threshold = 0.0f;

    public void SetThreshold(float val)
    {
        bool different = false;

        if (val != threshold) different = true;

        threshold = val;

        if (different) Generate();
    }

    public class Data<T>
    {
        public int w, h;
        public T[] data;

        public Data(int width, int height)
        {
            w = width;
            h = height;
            //data = new List<T>(w * h);
            data = new T[w * h];
        }

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
            if (!ValidPos(x,y))
            {
                // error
                Debug.LogError("Invalid Read Index");
                return;
            }

            data[y * w + x] = value;
        }

        public bool ValidPos(int x, int y)
        {
            if (x < 0 || y < 0 || x >= w || y >= h) return false;
            return true;
        }

        public ref T Pos(int x, int y)
        {
            return ref data[y * w + x];
        }

        public T GetPos(GridPos gPos)
        {
            return GetPos(gPos.x, gPos.y);
        }

        public void SetPos(GridPos gPos, T value)
        {
            SetPos(gPos.x, gPos.y, value);
        }

        public ref T Pos(GridPos gPos)
        {
            return ref Pos(gPos.x, gPos.y);
        }

        public bool ValidPos(GridPos gPos)
        {
            return ValidPos(gPos.x, gPos.y);
        }
    };

    public interface Algorithm
    {
        string Name();
        bool Dirty();
        void Apply(ref Data<bool> data);
    }

    public interface Filter
    {
        string Name();
        bool Dirty();
        void Apply(ref Data<float> data);
    }

    //public abstract class Algorithm
    //{
    //    public bool dirty = true;
    //    public abstract string Name { get; }
    //    public abstract void Apply(ref Data<bool> data);
    //};

    //public abstract class Filter
    //{
    //    public bool dirty = true;
    //    public abstract string Name { get; }
    //    public abstract void Apply(ref Data<float> data);
    //};

    public int width, height;
    public int seed;

    public List<Algorithm> algs = new List<Algorithm>();
    public List<Filter> filters = new List<Filter>();

    [SerializeReference]
    public List<Component> Algorithms = new();

    [SerializeReference]
    public List<Component> Filters = new();

    private void Start()
    {
        foreach (var item in Algorithms)
        {
            algs.Add(item as Algorithm);
        }
        foreach (var item in Filters)
        {
            filters.Add(item as Filter);
        }
    }

    private void Update()
    {
        bool needRecalc = false;

        foreach (var iter in filters)
        {
            if (iter.Dirty() == true)
            {
                needRecalc = true;
                break;
            }
        }

        foreach (var iter in algs)
        {
            if (iter.Dirty() == true)
            {
                needRecalc = true;
                break;
            }
        }

        if (needRecalc)
        {
            Generate();
        }
    }

    public void Generate() {
        var seed = FindObjectOfType<Seed>();
        seed.ResetRandom();

        // floating point data structure
        Data<float> initData = new Data<float>(width, height);

        // assign values
        // TODO: figure out seed setting.
        for (int i = 0; i < initData.data.Length; ++i)
        {
            initData.data[i] = Random.Range(-1.0f, 1.0f);
        }

        // apply filters
        for (int i = 0; i < filters.Count; ++i)
        {
            filters[i].Apply(ref initData);
        }

        Data<bool> mapData = new Data<bool>(width, height);
        
        for (int y_iter = 0; y_iter < height; ++y_iter)
        {
            for (int x_iter = 0; x_iter < width; ++x_iter)
            {
                if (initData.GetPos(x_iter, y_iter) >= threshold) // THIS IS WHERE FLOAT TURNS TO BOOL
                {
                    mapData.SetPos(x_iter, y_iter, true);
                }
                else
                {
                    mapData.SetPos(x_iter, y_iter, false);
                }
            }
        }

        // apply algorithms
        for (int i = 0; i < algs.Count; ++i)
        {
            algs[i].Apply(ref mapData);
        }

        MapManager.Instance.WriteTileData(mapData);
    }
}
