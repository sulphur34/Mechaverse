using UnityEngine;

namespace EntityActors
{
    public class TurretActor : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
        [field: SerializeField] public HingeJoint2D HingeJoint { get; private set; }
        [field: SerializeField] public WeaponActor WeaponActor { get; private set; }
        [field: SerializeField] public Vector2[] WeaponPositions  { get; private set; }
    }
}