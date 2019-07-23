using UnityEngine;
using System.Collections;

namespace atalantus.Buoyancy
{
    public static class DrawArrow
    {
        /// <summary>
        /// Draws a debug arrow
        /// </summary>
        /// <param name="from">Origin</param>
        /// <param name="to">Target</param>
        /// <param name="color">Color</param>
        /// <param name="arrowHeadLength">Arrow head length</param>
        /// <param name="arrowHeadAngle">Arrow head angle</param>
        public static void ForDebugLine(Vector3 from, Vector3 to, Color color, float arrowHeadLength = 0.25f,
            float arrowHeadAngle = 20.0f)
        {
            if (Vector3.Distance(from, to) < 0.1) return;

            Debug.DrawLine(from, to, color);
            DrawArrowEnd(false, from, to - from, color, arrowHeadLength, arrowHeadAngle);
        }

        private static void DrawArrowEnd(bool gizmos, Vector3 pos, Vector3 direction, Color color,
            float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(arrowHeadAngle, 0, 0) * Vector3.back;
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * Vector3.back;
            Vector3 up = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.back;
            Vector3 down = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.back;
            if (gizmos)
            {
                Gizmos.color = color;
                Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, up * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, down * arrowHeadLength);
            }
            else
            {
                Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
                Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
                Debug.DrawRay(pos + direction, up * arrowHeadLength, color);
                Debug.DrawRay(pos + direction, down * arrowHeadLength, color);
            }
        }
    }
}