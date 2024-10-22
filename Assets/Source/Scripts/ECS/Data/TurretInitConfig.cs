using EntityActors;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "TurretInitData")]
    public class TurretInitConfig : ScriptableObject
    {
        [field: SerializeField] public TurretActor TurretPrefab { get; private set; }
        [field: SerializeField] public WeaponInitConfig WeaponInitConfig { get; private set; }
        [field: SerializeField] public RotationData RotationData { get; private set; }

        [field: SerializeField] public float TrackerRange { get; private set; }
        [field: SerializeField] public float DetectAngle { get; private set; }
        [field: SerializeField] public float DetectRadius { get; private set; }
    }
}