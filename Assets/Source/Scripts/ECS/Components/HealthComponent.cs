using EntityActors;
using UnityEngine;

namespace ECS.Components
{
    public struct HealthComponent
    {
        public GameObject unit;
        public float maxValue;
        public float minValue;
        public float currentValue;
    }
}