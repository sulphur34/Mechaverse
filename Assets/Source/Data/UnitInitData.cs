using EntityActors;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "UnitInitData")]
    public class UnitInitData : ScriptableObject
    {
        [field: SerializeField] public UnitActor UnitPrefab { get; private set; }
        [field: SerializeField] public float DefaultSpeed { get; private set; } = 1.0f;
    }
}