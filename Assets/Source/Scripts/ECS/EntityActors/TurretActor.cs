using UnityEngine;

namespace EntityActors
{
    public class TurretActor : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
        [field: SerializeField] public ParticleSystem ShootParticle { get; private set; }
        [field: SerializeField] public HingeJoint2D HingeJoint { get; private set; }
    }
}