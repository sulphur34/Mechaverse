using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class MoveParticleSystem : IEcsRunSystem
    {
        private readonly EcsFilter<MovableComponent, MoveParticleComponent> _particleMoveFilter;

        public void Run()
        {
            foreach (var entity in _particleMoveFilter)
            {
                ref var movableComponent = ref _particleMoveFilter.Get1(entity);
                ref var moveParticleComponent = ref _particleMoveFilter.Get2(entity);

                if (!movableComponent.isMoving)
                {
                    moveParticleComponent.particleSystem.Stop();
                    return;
                }

                if(moveParticleComponent.particleSystem.isEmitting)
                    return;

                moveParticleComponent.particleSystem.Play();
            }
        }
    }
}