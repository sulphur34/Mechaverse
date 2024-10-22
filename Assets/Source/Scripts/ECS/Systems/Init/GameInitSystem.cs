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

        private readonly UnitInitConfig _playerInitConfig;
        private readonly UnitInitConfig _enemyInitConfig;

        private readonly Transform _spawnPoint;
        private readonly TurretInitConfig _turretInitConfig;
        private readonly WeaponInitConfig _turretWeaponInitConfig;
        private readonly PickUpsInitConfig _pickUpsInitConfig;
        private readonly WeaponInitConfig _mainWeaponInitConfig;
        private WeaponBuilder _weaponBuilder;
        private TurretBuilder _turretBuilder;
        private PickUpBuilder _pickUpBuilder;

        public GameInitSystem(UnitInitConfig playerInitConfig,
            UnitInitConfig enemyInitConfig,
            TurretInitConfig turretInitConfig,
            Transform spawnPoint,
            WeaponInitConfig turretWeaponInitConfig,
            PickUpsInitConfig pickUpsInitConfig,
            WeaponInitConfig mainWeaponInitConfig)
        {
            _playerInitConfig = playerInitConfig;
            _enemyInitConfig = enemyInitConfig;
            _turretInitConfig = turretInitConfig;
            _spawnPoint = spawnPoint;
            _turretWeaponInitConfig = turretWeaponInitConfig;
            _mainWeaponInitConfig = mainWeaponInitConfig;
            _pickUpsInitConfig = pickUpsInitConfig;
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
                unitBuilder.BuildEnemy(_enemyInitConfig, enemySpawnPosition, playerActor.transform);
                pickUpBuilder.Build(_pickUpsInitConfig, _spawnPoint.position);
            }
        }

        private UnitActor CreatePlayer(UnitBuilder builder)
        {
            var playerActor = builder.BuildPlayer(_playerInitConfig, _spawnPoint.position);

            foreach (var weaponPlace in playerActor.FrontWeaponsPlaceholders)
            {
                var weapon = _weaponBuilder.Build(_mainWeaponInitConfig, weaponPlace, weaponPlace.position);
                weapon.Get<ShootInputComponent>();
            }

            foreach (var placeholder in playerActor.TurretPlaceholders)
            {
                _turretBuilder.CreateTurret(_turretInitConfig, placeholder, playerActor.Rigidbody2D);
            }

            return playerActor;
        }
    }
}