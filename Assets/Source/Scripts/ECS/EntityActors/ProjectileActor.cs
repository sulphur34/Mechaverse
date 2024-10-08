using UnityEngine;

namespace EntityActors
{
    public class ProjectileActor : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
        [field: SerializeField] public ParticleSystem MoveParticleSystem { get; private set; }
        [field: SerializeField] public ParticleSystem CollisionParticleSystem { get; private set; }
    }
}