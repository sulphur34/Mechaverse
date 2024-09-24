using UnityEngine;

namespace EntityActors
{
    public class UnitActor : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
        [field: SerializeField] public ParticleSystem MoveParticleSystem { get; private set; }
        [field: SerializeField] public ParticleSystem CollisionParticleSystem { get; private set; }
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public Transform[] TurretPlaceholders { get; private set; }
        [field: SerializeField] public Collider2D[] InternalColliders { get; private set; }
        [field: SerializeField] public Transform[] FrontWeaponsPlaceholders { get; private set; }
    }
}