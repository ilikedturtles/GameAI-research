using UnityEngine;

public class MapTile : MonoBehaviour
{
    public uint X, Y;

    [Range(-1.0f, 1.0f)]
    public float Value = 1.0f;
    bool valueDirty = true;

    [Header("Visualizer")]
    public Gradient valueGrad;


    void LateUpdate()
    {
        // set color for visualization
        if (valueDirty == true)
        {
            gameObject.GetComponent<Renderer>().materials[0].SetColor("_Color", valueGrad.Evaluate((Value - (-1.0f)) / 2.0f));
        } 
    }

    public void SetXY(uint x, uint y)
    {
        X = x;
        Y = y;
    }

    public void SetValue(float value)
    {
        Value = value;
    }
}