using System.Collections.Generic;
using Leopotam.Ecs;

namespace ECS.Components
{
    public struct TurretComponent
    {
        public List<EcsEntity> weapons;
    }
}