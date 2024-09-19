using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Components
{
    public struct TriggerEnterComponent
    {
        public EcsEntity other;
        public EcsEntity self;
    }
}