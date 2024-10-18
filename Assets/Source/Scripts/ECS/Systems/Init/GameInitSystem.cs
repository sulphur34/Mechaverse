using Data;
using ECS.Components.Input;
using ECS.Data;
using EntityActors;
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
        private readonly TurretInitData _turretInitData;
        private readonly WeaponInitData _turretWeaponInitData;
        private readonly PickUpsInitData _pickUpsInitData;
        private readonly WeaponInitData _mainWeaponInitData;
        private WeaponBuilder _weaponBuilder;
        private TurretBuilder _turretBuilder;
        private PickUpBuilder _pickUpBuilder;

        public GameInitSystem(UnitInitData playerInitData,
            UnitInitData enemyInitData,
            TurretInitData turretInitData,
            Transform spawnPoint,
            WeaponInitData turretWeaponInitData,
            PickUpsInitData pickUpsInitData,
            WeaponInitData mainWeaponInitData)
        {
            _playerInitData = playerInitData;
            _enemyInitData = enemyInitData;
            _turretInitData = turretInitData;
            _spawnPoint = spawnPoint;
            _turretWeaponInitData = turretWeaponInitData;
            _mainWeaponInitData = mainWeaponInitData;
            _pickUpsInitData = pickUpsInitData;
        }

        public void Init()
        {
            _weaponBuilder = new WeaponBuilder(_world);
            _turretBuilder = new TurretBuilder(_world);
            var unitBuilder = new UnitBuilder(_world);
            var pickUpBuilder = new PickUpBuilder(_world);

            var playerActor = CreatePlayer(unitBuilder);

            for (int i = 0; i < 1; i++)
            {
                var enemySpawnPosition = _spawnPoint.position +
                                         new Vector3(Random.Range(-150f, 150f), Random.Range(-150f, 150f), 0f);
                unitBuilder.BuildEnemy(_enemyInitData, enemySpawnPosition, playerActor.transform);
                pickUpBuilder.Build(_pickUpsInitData, _spawnPoint.position);
            }
        }

        private UnitActor CreatePlayer(UnitBuilder builder)
        {
            var playerActor = builder.BuildPlayer(_playerInitData, _spawnPoint.position);

            foreach (var weaponPlace in playerActor.FrontWeaponsPlaceholders)
            {
                var weapon = _weaponBuilder.Build(_mainWeaponInitData, weaponPlace, weaponPlace.position);
                weapon.Get<ShootInputComponent>();
            }

            // foreach (var placeholder in playerActor.TurretPlaceholders)
            // {
            //     _turretBuilder.CreateTurret(_turretInitData, placeholder, playerActor.Rigidbody2D);
            // }

            return playerActor;
        }
    }
}