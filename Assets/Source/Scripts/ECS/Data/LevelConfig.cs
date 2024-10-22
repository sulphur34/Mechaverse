using EntityActors;
using UnityEngine;

namespace ECS.Data
{
    [CreateAssetMenu(menuName = "LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [field: SerializeField] public LevelActor LevelPrefab { get; private set; }
        [field: SerializeField] public float GridDensity { get; private set; }
    }
}