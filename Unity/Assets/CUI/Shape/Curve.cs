using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUI.Shape
{
    /// <summary>
    /// 曲线
    /// </summary>
    public class Curve
    {
        /// <summary>
        /// 绘制简单LineRenderer
        /// </summary>
        /// <param name="fromPos"></param>
        /// <param name="fromDir"></param>
        /// <param name="toPos"></param>
        /// <param name="toDir"></param>
        /// <param name="line"></param>
        /// <param name="loopCount"></param>
        /// <param name="width"></param>
        public static void DrawSimpleCurve(Vector3 fromPos, Vector3 fromDir, Vector3 toPos, Vector3 toDir, LineRenderer line, int loopCount, float width)
        {
            float power = Vector3.Distance(fromPos, toPos) / 2;
            fromDir = fromPos + fromDir * power;
            toDir = toPos + toDir * power;

            Vector3[] points = new Vector3[loopCount + 1];
            points[0] = fromPos;
            points[loopCount] = toPos;
            for (int i = 1; i < loopCount; i++)
            {
                float t = (float)i / (float)loopCount;
                points[i] = fromPos * Mathf.Pow(1 - t, 3) + 3 * fromDir * Mathf.Pow(1 - t, 2) * t + 3 * toDir * (1 - t) * Mathf.Pow(t, 2) + toPos * Mathf.Pow(t, 3);
            }

            line.startWidth = width;
            line.endWidth = width;
            line.positionCount = loopCount + 1;
            line.SetPositions(points);
        }

        /// <summary>
        /// 绘制简单Mesh
        /// </summary>
        /// <param name="fromPos"></param>
        /// <param name="fromDir"></param>
        /// <param name="toPos"></param>
        /// <param name="toDir"></param>
        /// <param name="shareMesh"></param>
        /// <param name="mesh"></param>
        public static void DrawSimpleCurve(Vector3 fromPos, Vector3 fromDir, Vector3 toPos, Vector3 toDir, Mesh shareMesh, Mesh mesh)
        {
            float power = Vector3.Distance(fromPos, toPos) / 2;
            fromDir = fromPos + fromDir * power;
            toDir = toPos + toDir * power;


        }

    }
}