using Components;
using Leopotam.Ecs;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

namespace Systems
{
    public class PlayerMoveInputSystem : IEcsRunSystem
    {
        private const KeyCode UpKey = KeyCode.W;
        private const KeyCode DownKey = KeyCode.S;
        private const KeyCode LeftKey = KeyCode.A;
        private const KeyCode RightKey = KeyCode.D;

        private readonly EcsFilter<MoveInputEventComponent> _inputEventsFilter;

        public void Run()
        {
            Vector2 direction = Vector2.zero;

            if(Input.GetKey(UpKey))
                direction += Vector2.up;

            if(Input.GetKey(DownKey))
                direction += Vector2.down;

            if(Input.GetKey(LeftKey))
                direction += Vector2.left;

            if(Input.GetKey(RightKey))
                direction += Vector2.right;

            direction = direction.normalized;

            foreach (var input in _inputEventsFilter)
            {
                ref var inputEvent = ref _inputEventsFilter.Get1(input);
                inputEvent.direction = new Vector2(direction.y, -direction.x);
            }
        }
    }
}