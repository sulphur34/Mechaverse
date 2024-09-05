using UnityEngine;

namespace EntityActors
{
    public class UnitActor : MonoBehaviour
    {
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
    }
}