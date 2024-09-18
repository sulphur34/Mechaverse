using EntityActors;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "WeaponInitData")]
    public class WeaponInitData : ScriptableObject
    {
        [field: SerializeField] public ProjectileActor ProjectilePrefab { get; private set; }
        [field: SerializeField] public WeaponActor WeaponActor { get; private set; }
        [field: SerializeField] public int MagazineCapacity { get; private set; }
        [field: SerializeField] public float ShotDelay { get; private set; }
        [field: SerializeField] public float ShotForce { get; private set; }
        [field: SerializeField] public float ReloadingSpeed { get; private set; }
        [field: SerializeField] public float DamageValue { get; private set; }
    }
}