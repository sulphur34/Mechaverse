using EntityActors;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "PickUpsInitData")]
    public class PickUpsInitConfig : ScriptableObject
    {
        [field: SerializeField] public PickUpActor PickUpActor { get; private set; }
        [field: SerializeField] public float HealthRestoreValue { get; private set; }
    }
}