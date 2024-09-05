using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class AnimatedCharacterSystem : IEcsRunSystem
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        private readonly EcsFilter<AnimatedCharacterComponent, MovableComponent> _animatedFilter;

        public void Run()
        {
            foreach (var entity in _animatedFilter)
            {
                ref var animatedCharacterComponent = ref _animatedFilter.Get1(entity);
                ref var movableComponent = ref _animatedFilter.Get2(entity);

                animatedCharacterComponent.animator.SetBool(IsMoving, movableComponent.isMoving);
            }
        }
    }
}