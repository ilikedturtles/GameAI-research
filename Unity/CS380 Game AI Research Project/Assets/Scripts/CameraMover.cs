using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField]
    private float yAdjust;

    private float yAdjustMod;

    public void SetYAdjustMod(float yAdjust)
    {
        yAdjustMod = yAdjust;
    }

    private void LateUpdate()
    {
        Vector3 newCameraPos;

        MapPos farCorner = new(MapManager.Instance.GetWidth() - 1, MapManager.Instance.GetHeight() - 1);
        MapPos nearCorner = new(0, 0);

        Vector3 farCornerV3 = MapManager.Instance.GetTileWorldPos(farCorner);
        Vector3 nearCornerV3 = MapManager.Instance.GetTileWorldPos(nearCorner);

        newCameraPos = nearCornerV3 + (farCornerV3 - nearCornerV3) / 2.0f;
        newCameraPos.y = yAdjust + yAdjustMod;

        transform.position = newCameraPos;
    }
}
