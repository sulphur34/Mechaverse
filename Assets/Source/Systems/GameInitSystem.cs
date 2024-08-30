using Components;
using Data;
using Leopotam.Ecs;
using UnityEngine;

namespace Systems
{
    public class GameInitSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world;

        private readonly UnitInitData _playerInitData;
        private readonly UnitInitData _enemyInitData;

        private readonly Transform _spawnPoint;

        public GameInitSystem(UnitInitData playerInitData, UnitInitData enemyInitData, Transform spawnPoint)
        {
            _playerInitData = playerInitData;
            _enemyInitData = enemyInitData;
            _spawnPoint = spawnPoint;
        }

        public void Init()
        {
            var unitActor = Object.Instantiate(_playerInitData.UnitPrefab, _spawnPoint.position, Quaternion.identity);

            var player = _world.NewEntity();
            player.Get<PlayerComponent>();
            player.Get<InputEventComponent>();

            ref var movableComponent = ref player.Get<MovableComponent>();
            movableComponent.moveSpeed = _playerInitData.DefaultSpeed;
            movableComponent.transform = unitActor.transform;

            ref var rotatableComponent = ref player.Get<RotatableComponent>();
            rotatableComponent.transform = unitActor.transform;

            ref var animationsComponent = ref player.Get<AnimatedCharacterComponent>();
            animationsComponent.animator = unitActor.Animator;

            ref var moveParticleComponent = ref player.Get<MoveParticleComponent>();
            moveParticleComponent.particleSystem = unitActor.ParticleSystem;

            for (int i = 0; i < 10; i++)
            {
                var enemySpawnPosition = _spawnPoint.position +
                                         new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), 0f);
                CreateEnemy(enemySpawnPosition, unitActor.transform);
            }
        }

        private void CreateEnemy(Vector3 atPosition, Transform target)
        {
            var unitActor = Object.Instantiate(_enemyInitData.UnitPrefab, atPosition, Quaternion.identity);

            var enemy = _world.NewEntity();

            ref var enemyMovableComponent = ref enemy.Get<MovableComponent>();
            enemyMovableComponent.moveSpeed = _enemyInitData.DefaultSpeed;
            enemyMovableComponent.transform = unitActor.transform;

            ref var enemyRotatableComponent = ref enemy.Get<RotatableComponent>();
            enemyRotatableComponent.transform = unitActor.transform;

            ref var enemyAnimationsComponent = ref enemy.Get<AnimatedCharacterComponent>();
            enemyAnimationsComponent.animator = unitActor.Animator;

            ref var followComponent = ref enemy.Get<FollowComponent>();
            followComponent.target = target;

            ref var enemyMoveParticleComponent = ref enemy.Get<MoveParticleComponent>();
            enemyMoveParticleComponent.particleSystem = unitActor.ParticleSystem;
        }
    }
}