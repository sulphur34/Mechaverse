using EntityActors;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "PickUpsInitData")]
    public class PickUpsInitData : ScriptableObject
    {
        [field: SerializeField] public PickUpActor PickUpActor { get; private set; }
        [field: SerializeField] public float HaalthRestoreValue { get; private set; }
    }
}