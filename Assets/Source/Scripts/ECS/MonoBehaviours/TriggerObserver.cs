using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.MonoBehaviours
{
    public class TriggerObserver : MonoBehaviour
    {
        private EcsWorld _ecsWorld;

        public void Initialize(EcsWorld ecsWorld, EcsEntity ecsEntity)
        {
            _ecsWorld = ecsWorld;
            EcsEntity = ecsEntity;
        }

        public EcsEntity EcsEntity {get; private set;}

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_ecsWorld == null)
                return;

            var otherObserver = other.GetComponent<TriggerObserver>();

            if (otherObserver != null)
            {
                var triggerEvent = _ecsWorld.NewEntity();
                triggerEvent.Get<TriggerEnterComponent>() = new TriggerEnterComponent
                {
                    self = EcsEntity,
                    other = otherObserver.EcsEntity
                };
            }
        }
    }
}