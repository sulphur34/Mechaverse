using UnityEngine;

namespace EntityActors
{
    public class WeaponActor : MonoBehaviour
    {
        [field: SerializeField] public BulletActor BulletPrefab { get; private set; }
        [field: SerializeField] public Transform ShootPoint { get; private set; }
        [field: SerializeField] public float ShotDelay { get; private set; }
        [field: SerializeField] public float ShotForce { get; private set; }
        [field: SerializeField] public float ReloadingSpeed { get; private set; }
    }
}