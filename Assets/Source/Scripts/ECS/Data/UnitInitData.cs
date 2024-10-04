using Data;
using EntityActors;
using UnityEngine;

namespace ECS.Data
{
    [CreateAssetMenu(menuName = "UnitInitData")]
    public class UnitInitData : ScriptableObject
    {
        [field: SerializeField] public UnitActor UnitPrefab { get; private set; }
        [field: SerializeField] public MovingData MovingData { get; private set; }
        [field: SerializeField] public RotationData RotationData { get; private set; }
        [field: SerializeField] public float DefaultSpeed { get; private set; } = 1.0f;
        [field: SerializeField] public float HealthValue { get; private set; }
    }
}