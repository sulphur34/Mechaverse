using UnityEngine;

namespace EntityActors
{
    public class WeaponActor : MonoBehaviour
    {
        [field: SerializeField] public Transform ShootPoint { get; private set; }
    }
}