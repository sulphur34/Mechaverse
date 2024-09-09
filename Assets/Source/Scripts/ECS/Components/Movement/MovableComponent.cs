using UnityEngine;

namespace ECS.Components.Movement
{
    public struct MovableComponent
    {
        public Transform transform;
        public float moveSpeed;
        public bool isMoving;
    }
}