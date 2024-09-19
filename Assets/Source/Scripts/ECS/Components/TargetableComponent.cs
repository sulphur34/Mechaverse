using UnityEngine;
using Utilitiy;

namespace ECS.Components
{
    public struct TargetableComponent
    {
        public Teams team;
        public Transform transform;
    }
}