using Leopotam.Ecs;

namespace Systems
{
    public class EcsBuilder
    {
        protected EcsWorld _world;

        public EcsBuilder(EcsWorld world)
        {
            _world = world;
        }
    }
}