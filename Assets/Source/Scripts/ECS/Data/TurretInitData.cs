using EntityActors;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "TurretInitData")]
    public class TurretInitData : ScriptableObject
    {
        [field: SerializeField] public TurretActor TurretPrefab { get; private set; }
        [field: SerializeField] public RotationData RotationData { get; private set; }
    }
}