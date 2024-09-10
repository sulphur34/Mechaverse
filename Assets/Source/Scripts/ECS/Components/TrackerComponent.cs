using Leopotam.Ecs;
using UnityEngine;
using Utilitiy;

namespace ECS.Components
{
    public struct TrackerComponent
    {
        public float searchRadius;
        public Teams selfTeam;
        public Transform selfTransform;
        public Transform targetTransform;
    }
}