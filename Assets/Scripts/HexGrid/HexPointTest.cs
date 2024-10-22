using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexPointTest : MonoBehaviour
{
    [SerializeField] Vector3Int offsetCoord;
    [SerializeField] bool isTop;

    // Update is called once per frame
    void Update()
    {
        Vector3Int cubicCoord = HexUtils.OffsetToCubic(offsetCoord);
        HexPoint point = new HexPoint(cubicCoord, isTop);
        Debug.Log(point.getPosition());
    }
}
