using ECS.Components;
using ECS.Components.Movement;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class AnimatedCharacterSystem : IEcsRunSystem
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");

        private readonly EcsFilter<AnimatedCharacterComponent, MovableComponent> _filter;

        public void Run()
        {
            foreach (var entity in _filter)
            {
                ref var animatedCharacterComponent = ref _filter.Get1(entity);
                ref var movableComponent = ref _filter.Get2(entity);

                animatedCharacterComponent.animator.SetBool(IsMoving, movableComponent.isMoving);
            }
        }
    }
}