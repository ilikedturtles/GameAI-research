using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GridPos = MapGenSys.GridPos;
public class BiggestIsland : MonoBehaviour, MapGenSys.Algorithm
{
    private bool dirty = true;

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

    public void Apply(ref MapGenSys.Data<bool> map)
    {
        dirty = false;

        if (!alg_enabled) return;

        // index of island IDs based on grid pos
        // each contiguous space will have a unique ID

        // Key: position, Value: Island ID
        Dictionary<GridPos, int> isles = new();

        int numIsles = 0;

        for (int i = 0; i < map.w * map.h; ++i)
        {
            // skip walls
            if (map.data[i] == false) continue;

            // get index position
            GridPos gPos = new(i % map.w, i / map.w);

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
            Stack<GridPos> ffStack = new();
            ffStack.Push(gPos);

            while (ffStack.Count > 0)
            {
                var top = ffStack.Pop();

                for (int j = 0; j < 4; ++j)
                {
                    GridPos eval = top + MapGenSys.offsets[j];

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
            GridPos gPos = new(i % map.w, i / map.w);

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
