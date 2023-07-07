using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation, Color color, float duration)
    {
        var halfSize = size * 0.5f;


        var tfl = center + rotation * new Vector3(-halfSize.x, halfSize.y, halfSize.z);
        var tfr = center + rotation * new Vector3(halfSize.x, halfSize.y, halfSize.z);
        var tbl = center + rotation * new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
        var tbr = center + rotation * new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        var bfl = center + rotation * new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
        var bfr = center + rotation * new Vector3(halfSize.x, -halfSize.y, halfSize.z);
        var bbl = center + rotation * new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
        var bbr = center + rotation * new Vector3(halfSize.x, -halfSize.y, -halfSize.z);

        Debug.DrawLine(tfl, tfr, color, duration);
        Debug.DrawLine(tfr, tbr, color, duration);
        Debug.DrawLine(tbr, tbl, color, duration);
        Debug.DrawLine(tbl, tfl, color, duration);
        
        Debug.DrawLine(bfl, bfr, color, duration);
        Debug.DrawLine(bfr, bbr, color, duration);
        Debug.DrawLine(bbr, bbl, color, duration);
        Debug.DrawLine(bbl, bfl, color, duration);
        
        Debug.DrawLine(tfl, bfl, color, duration);
        Debug.DrawLine(tfr, bfr, color, duration);
        Debug.DrawLine(tbl, bbl, color, duration);
        Debug.DrawLine(tbr, bbr, color, duration);
    }
}
