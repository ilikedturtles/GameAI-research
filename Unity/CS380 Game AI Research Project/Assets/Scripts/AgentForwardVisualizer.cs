using UnityEngine;

public class AgentForwardVisualizer : MonoBehaviour
{
    [SerializeField, Range(0.0f, 25.0f)]
    public float RayLength = 10.0f;
    [SerializeField, ColorUsage(false)]
    public Color RayColor = Color.red;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * RayLength, RayColor);
    }
}
