using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation, Color color, float duration)
    {
        if (!Application.isEditor)
        {
            return;
        }
        
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

    public static void DrawSphere(Vector3 center, float radius, Color color, float duration)
    {
        if (!Application.isEditor)
        {
            return;
        }
        
        var pos = new Vector3[][] { new Vector3[32], new Vector3[32], new Vector3[32] };
        for (var i = 0; i < 32; i++)
        {
            var degree = i * (360f / 32);
            var angle = degree * Mathf.Deg2Rad;

            var value1 = radius * Mathf.Cos(angle);
            var value2 = radius * Mathf.Sin(angle);

            pos[0][i] = new Vector3(center.x + value1, center.y, center.z + value2);
            pos[1][i] = new Vector3(center.x, center.y + value1, center.z + value2);
            pos[2][i] = new Vector3(center.x + value1, center.y + value2, center.z);

            if (i > 0)
            {
                Debug.DrawLine(pos[0][i], pos[0][i - 1], color, duration);
                Debug.DrawLine(pos[1][i], pos[1][i - 1], color, duration);
                Debug.DrawLine(pos[2][i], pos[2][i - 1], color, duration);

                if (i >= 31)
                {
                    Debug.DrawLine(pos[0][i], pos[0][0], color, duration);
                    Debug.DrawLine(pos[1][i], pos[1][0], color, duration);
                    Debug.DrawLine(pos[2][i], pos[2][0], color, duration);
                }
            }
        }
    }
}
