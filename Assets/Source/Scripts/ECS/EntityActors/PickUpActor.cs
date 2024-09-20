using UnityEngine;

namespace EntityActors
{
    public class PickUpActor : MonoBehaviour
    {
        [field: SerializeField] public ParticleSystem CollisionParticleSystem { get; private set; }
    }
}