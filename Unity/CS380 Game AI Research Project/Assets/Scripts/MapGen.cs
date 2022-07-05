using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    // dimensions of map generated (tiles)
    public uint width = 10, height = 10;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate() {
        MapManager.Instance.SetSize(width, height);
    }
}
