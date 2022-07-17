using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invert : MonoBehaviour, MapGenSys.Algorithm
{
    // Alg Interface
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
        return "Inversion";
    }

    public bool Dirty()
    {
        return dirty;
    }

    public void Apply(ref MapData<bool> data)
    {
        if (!alg_enabled) return;

        dirty = false;

        for (int i = 0; i < data.w * data.h; ++i)
        {
            data.data[i] = !data.data[i];
        }
    }
}
