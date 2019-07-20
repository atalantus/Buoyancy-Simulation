using System;
using UnityEngine;

namespace atalantus.Buoyancy
{
    public class Buoyancy : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private bool customCenterOfMass;
        [Tooltip("The center of mass relative to the transform`s origin")] [SerializeField] private Vector3 centerOfMassOffset;
        [Space]
        
        [Header("Debug Options")]
        [SerializeField] private bool debugView;
        [SerializeField] private float debugForceScalar;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (customCenterOfMass)
                _rb.centerOfMass = centerOfMassOffset;
        }

        private void FixedUpdate()
        {
            var centerOfGravity = _rb.worldCenterOfMass;
            
            // Buoyancy
            
            if (!debugView) return;
            
            DrawArrow.ForDebugLine(centerOfGravity,
                new Vector3(centerOfGravity.x, centerOfGravity.y + Physics.gravity.y * debugForceScalar,
                    centerOfGravity.z), Color.red);
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