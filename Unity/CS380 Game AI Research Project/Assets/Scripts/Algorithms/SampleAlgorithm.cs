using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAlgorithm : MapGenSys.Algorithm
{
    //public string name = "Sample";
    
    override public string Name { get { return "Sample";  } }

    override public void Apply(ref MapGenSys.Data<bool> data)
    {
        for (int i = 0; i < data.w * data.h; ++i)
        {
            data.data[i] = !data.data[i];
        }
    }
}
