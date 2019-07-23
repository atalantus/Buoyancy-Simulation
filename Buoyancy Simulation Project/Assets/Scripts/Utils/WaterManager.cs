using UnityEngine;
using UnityEngine.Serialization;

namespace atalantus.Buoyancy
{
    /// <inheritdoc />
    /// <summary>
    ///     Controls the water.
    /// </summary>
    public class WaterManager : MonoBehaviour
    {
        [SerializeField] private Transform water;
        [SerializeField] private Material waterMaterial;
        public static WaterManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);
        }

        /// <summary>
        ///     Get the current water height at a position.
        /// </summary>
        /// <param name="worldXPos">x coordinate in world space</param>
        /// <returns>y coordinate of water in world space</returns>
        public float GetWaterHeight(float worldXPos)
        {
            return water.position.y +
                   Mathf.Sin(Time.timeSinceLevelLoad * waterMaterial.GetFloat("_WaveSpeed") +
                             worldXPos * waterMaterial.GetFloat("_WaveDistance")) *
                   waterMaterial.GetFloat("_WaveHeight");
        }

        /// <summary>
        ///     Checks if a position is under the water.
        /// </summary>
        /// <param name="pos">Position to check.</param>
        /// <returns></returns>
        public bool TouchesWater(Vector3 pos)
        {
            return pos.y < GetWaterHeight(pos.x);
        }
    }
}