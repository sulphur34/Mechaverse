using Components;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class PlayerRotationInputSystem : IEcsRunSystem
    {
        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";

        private readonly EcsFilter<RotationInputEventComponent> _inputEventsFilter;

        public void Run()
        {
            var mousePosition = Input.mousePosition;
            var horizontal = (mousePosition.x / Screen.width) * 2 - 1;
            var vertical = (mousePosition.y / Screen.height) * 2 - 1;

            foreach (var input in _inputEventsFilter)
            {
                ref var inputEvent = ref _inputEventsFilter.Get1(input);
                inputEvent.direction = new Vector2(horizontal, vertical);
            }
        }
    }
}