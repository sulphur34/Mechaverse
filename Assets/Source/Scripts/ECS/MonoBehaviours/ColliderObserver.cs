using System;
using ECS.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.MonoBehaviours
{
    public class ColliderObserver : MonoBehaviour
    {
        private EcsWorld _ecsWorld;

        public void Initialize(EcsWorld ecsWorld, EcsEntity ecsEntity)
        {
            _ecsWorld = ecsWorld;
            EcsEntity = ecsEntity;
        }

        public EcsEntity EcsEntity { get; private set; }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_ecsWorld == null)
                return;

            var otherObserver = other.gameObject.GetComponent<ColliderObserver>();
            Vector2 collisionPoint = other.GetContact(0).point;

            if (otherObserver != null)
            {
                EcsEntity.Get<CollisionEnterComponent>() = new CollisionEnterComponent
                {
                    other = otherObserver.EcsEntity,
                    collisionPoint = collisionPoint
                };
            }
        }
    }
}