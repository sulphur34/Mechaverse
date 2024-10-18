using Data;
using EntityActors;
using UnityEngine;

namespace ECS.Data
{
    [CreateAssetMenu(menuName = "ProjectileInitData")]
    public class ProjectileInitData : ScriptableObject
    {
        [field: SerializeField] public ProjectileActor ProjectilePrefab { get; private set; }
        [field: SerializeField] public MovingData MovingData { get; private set; }
        [field: SerializeField] public RotationData RotationData { get; private set; }
        [field: SerializeField] public float TrackerRange { get; private set; }
    }
}