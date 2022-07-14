using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenSys : MonoBehaviour
{
    public class Data<T>
    {
        public int w, h;
        public T[] data;
        //public ArrayList data;
        //public T[] data;

        public Data(int width, int height)
        {
            w = width;
            h = height;
            //data = new List<T>(w * h);
            data = new T[w * h];
        }

        public T GetPos(int x, int y)
        {
            if (y * w + x > w * h)
            {
                // error
                Debug.LogError("Invalid Write Index");
            }

            return data[y * w + x];
        }

        public void SetPos(int x, int y, T value) {
            if (y * w + x > w * h)
            {
                // error
                Debug.LogError("Invalid Read Index");
                return;
            }


            data[y * w + x] = value;
        }
    };

    public abstract class Algorithm
    {
        public abstract string Name { get; }
        public abstract void Apply(ref Data<bool> data);
        // TODO: implement method to get/set paramenters.
    };

    public abstract class Filter
    {
        public abstract string Name { get; }
        public abstract void Apply(ref Data<float> data);
        // TODO: implement method to get/set paramenters.
    };

    public int width, height;
    public int seed;

    [SerializeField]
    public List<Algorithm> algs = new List<Algorithm>();

    [SerializeField]
    public List<Filter> filters = new List<Filter>();

    private void Start()
    {
        algs.Add(new SampleAlgorithm());

        filters.Add(new SampleFilter());
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
                if (initData.GetPos(x_iter, y_iter) >= 0.0f) // THIS IS WHERE FLOAT TURNS TO BOOL
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

        MapManager.Instance.WriteTileData(initData);
    }
}
