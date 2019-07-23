using System;
using UnityEngine;

namespace atalantus.Buoyancy
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public abstract class BuoyancyBase : MonoBehaviour
    {
        protected Rigidbody _rb;
        protected Collider _col;
        protected Vector3 _centerOfGravity;
        protected Vector3? _centerOfBuoyancy = null;
        protected Vector3 _buoyantForce;

        [Header("Settings")] [SerializeField] private bool customCenterOfMass;

        [Tooltip("The center of mass relative to the transform`s origin")] [SerializeField]
        private Vector3 centerOfMassOffset;

        [Space] [Header("Debug Options")] [SerializeField]
        private bool debugView = true;

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

            _centerOfGravity = _rb.worldCenterOfMass;
        }

        private void Update()
        {
            // Get data
            var vel = _rb.velocity;
            var buoyancy = Vector3.zero;

            // Buoyancy
            if (true)
            {
                Debug.Log("Apply Buoyancy");
                ApplyBuoyancy();
            }
            else
            {
                _centerOfBuoyancy = null;
            }

            // Show debug output
            if (debugView) ShowDebug(_centerOfGravity, vel, buoyancy);
        }

        protected abstract void ApplyBuoyancy();

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

        protected virtual void OnDrawGizmos()
        {
            if (_rb == null || !debugView) return;

            var centerOfGravity = _rb.worldCenterOfMass;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(centerOfGravity, 0.1f);
        }
    }
}