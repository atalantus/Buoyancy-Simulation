using System;
using UnityEngine;

namespace atalantus.Buoyancy
{
    public class Buoyancy : MonoBehaviour
    {
        private Rigidbody _rb;
        private Collider _col;
        
        [Header("Settings")] 
        [SerializeField] private bool customCenterOfMass;
        [Tooltip("The center of mass relative to the transform`s origin")] [SerializeField] private Vector3 centerOfMassOffset;
        [Space]
        
        [Header("Debug Options")]
        [SerializeField] private bool debugView = true;
        [SerializeField] private float debugForceScalar = 0.1f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<Collider>();
        }

        private void Start()
        {
            if (customCenterOfMass)
                _rb.centerOfMass = centerOfMassOffset;
        }

        private void FixedUpdate()
        {
            // Get data
            var centerOfGravity = _rb.worldCenterOfMass;
            var centerOfBuoyancy = centerOfGravity;
            var vel = _rb.velocity;
            var buoyancy = Vector3.zero;
            
            // Buoyancy
            if (IsTouchingWater())
            {
                buoyancy = new Vector3(0, 20, 0);
                // Applies buoyant force (not affected by the mass of the object)
                _rb.AddForceAtPosition(buoyancy, centerOfBuoyancy, ForceMode.Acceleration);
            }
            
            // Show debug output
            if (debugView) ShowDebug(centerOfGravity, vel, buoyancy);
        }

        private bool IsTouchingWater()
        {
            var bottom = _col.bounds.min;

            return WaterManager.Instance.TouchesWater(bottom);
        }

        private void ShowDebug(Vector3 centerOfGravity, Vector3 vel, Vector3 buoyancy)
        {
            DrawArrow.ForDebugLine(centerOfGravity,
                new Vector3(centerOfGravity.x, centerOfGravity.y + Physics.gravity.y * debugForceScalar,
                    centerOfGravity.z), Color.red);
            
            DrawArrow.ForDebugLine(centerOfGravity, centerOfGravity + vel * debugForceScalar, Color.blue);
            
            DrawArrow.ForDebugLine(centerOfGravity, centerOfGravity + buoyancy * debugForceScalar, Color.green);
        }

        private void OnDrawGizmos()
        {
            if (_rb == null || !debugView) return;

            var centerOfGravity = _rb.worldCenterOfMass;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(centerOfGravity, 0.1f);
        }
    }
}