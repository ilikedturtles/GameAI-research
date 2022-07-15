using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggestIsland : MapGenSys.Algorithm
{
    //public string name = "Sample";

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

    private GridPos[] offsets = new[] {
        new GridPos(-1, 0),
        new GridPos( 1, 0),
        new GridPos( 0,-1),
        new GridPos( 0, 1)
    };

    override public string Name { get { return "Biggest Island"; } }

    override public void Apply(ref MapGenSys.Data<bool> map)
    {
        
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

            int val = 0;
            // if not in map, continue
            if (isles.TryGetValue(gPos, out val) == false) {
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
                    GridPos eval = top + offsets[j];

                    // check pos is in range
                    if (!map.ValidPos(eval.x, eval.y)) continue;

                    // check that pos is not wall
                    if (!map.GetPos(eval.x, eval.y)) continue;

                    if (isles[eval] != 0) continue;

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

        int biggestIsleId = 0;
        foreach (var item in count)
        {
            if (item > biggestIsleId)
            {
                biggestIsleId = item;
            }
        }

        for (int i = 0; i < map.w * map.h; ++i)
        {
            GridPos gPos = new(i % map.w, i / map.w);

            int id;

            if (isles.TryGetValue(gPos, out id))
            {
                if (id != biggestIsleId)
                {
                   map.data[i] = false;
                }
            }
        }
    }
}
