using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggestIsland : MonoBehaviour, MapGenSys.Algorithm
{
    private bool dirty = true;

    [SerializeField]
    private bool _alg_enabled = true;
    public bool alg_enabled
    {
        get { return _alg_enabled; }
        set { _alg_enabled = value; dirty = true; }
    }

    public string Name()
    {
        return "Biggest Island";
    }

    public bool Dirty()
    {
        return dirty;
    }

    public void Apply(ref MapData<bool> map)
    {
        dirty = false;

        if (!alg_enabled) return;

        // index of island IDs based on grid pos
        // each contiguous space will have a unique ID

        // Key: position, Value: Island ID
        Dictionary<MapPos, int> isles = new();

        int numIsles = 0;

        for (int i = 0; i < map.w * map.h; ++i)
        {
            // skip walls
            if (map.data[i] == false) continue;

            // get index position
            MapPos gPos = new(i % map.w, i / map.w);

            // if not in map, continue
            if (isles.TryGetValue(gPos, out int val) == false) {
                isles.Add(gPos, ++numIsles);
            }
            // if already in island, continue
            else if (val != 0)
            {
                continue;
            }

            // mark new island

            // floodfill whole island
            Stack<MapPos> ffStack = new();
            ffStack.Push(gPos);

            while (ffStack.Count > 0)
            {
                var top = ffStack.Pop();

                for (int j = 0; j < 4; ++j)
                {
                    MapPos eval = top + MapPos.Offsets[j];

                    // check pos is in range
                    if (!map.ValidPos(eval.x, eval.y)) continue;

                    // check that pos is not wall
                    if (!map.GetPos(eval.x, eval.y)) continue;

                    if (isles.ContainsKey(eval) && (isles[eval] != 0))
                    {
                        continue;
                    }

                    isles[eval] = numIsles;

                    ffStack.Push(eval);
                }
            }
        }

        int[] count = new int[numIsles];

        // find biggest island
        foreach (var item in isles)
        {
            count[item.Value - 1]++;
        }

        int maxIndex = 0;
        int currIndex = 0;
        int biggestIsleId = 0;
        foreach (var item in count)
        {
            ++currIndex;
            if (item > biggestIsleId)
            {
                biggestIsleId = item;
                maxIndex = currIndex;
            }

        }

        for (int i = 0; i < map.w * map.h; ++i)
        {
            MapPos gPos = new(i % map.w, i / map.w);

            int id;

            if (isles.TryGetValue(gPos, out id))
            {
                if (id != maxIndex)
                {
                   map.data[i] = false;
                }
            } else
            {
                map.data[i] = false;
            }
        }
    }

}
