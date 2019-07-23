using System.Collections;
using System.Collections.Generic;
using System.Linq;
using atalantus.Buoyancy;
using UnityEngine;
using UnityEngine.Serialization;

namespace atalantus.Buoyancy
{
    public class BuoyancyVertices : BuoyancyBase
    {
        [SerializeField] private MeshCollider collisionMesh;

        private List<Vector3> _underWaterVertices = new List<Vector3>();

        protected override void ApplyBuoyancy()
        {
            // Get vertices of collision mesh
            var vs = collisionMesh.sharedMesh.vertices;

            // Filter underwater vertices and vertices with the same position
            _underWaterVertices = vs.Select(v => transform.TransformPoint(v))
                .Where(v => WaterManager.Instance.TouchesWater(v))
                .GroupBy(sv => new {sv.x, sv.y, sv.z})
                .Select(v => v.First()).ToList();

            // Calculate centroid of underwater vertices
            var x = Vector3.zero;

            foreach (var underWaterVertex in _underWaterVertices)
            {
                x += underWaterVertex;
            }

            _centerOfBuoyancy = x / _underWaterVertices.Count;
        }

        protected override void OnDrawGizmos()
        {
            var s = new Vector3(0.05f, 0.05f, 0.05f);
            Gizmos.color = Color.yellow;
            foreach (var v in _underWaterVertices)
            {
                Gizmos.DrawCube(v, s);
            }

            if (_centerOfBuoyancy != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere((Vector3) _centerOfBuoyancy, 0.1f);
            }
        }
    }
}