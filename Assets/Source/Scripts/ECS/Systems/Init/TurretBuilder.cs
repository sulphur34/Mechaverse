using System.Collections.Generic;
using Data;
using ECS.Components;
using ECS.Components.Movement;
using EntityActors;
using Leopotam.Ecs;
using UnityEngine;
using Utilitiy;
namespace Systems
{
    public class TurretBuilder
    {
        private EcsWorld _world;

        public TurretBuilder(EcsWorld world)
        {
            _world = world;
        }

        public TurretActor CreateTurret(TurretInitData turretInitData, Transform placeholder, Rigidbody2D connectBody)
        {
                var turretActor = Object.Instantiate(
                    turretInitData.TurretPrefab,
                    placeholder.transform.position,
                    Quaternion.identity);

                var turret = _world.NewEntity();
                turretActor.HingeJoint.connectedBody = connectBody;
                turretActor.HingeJoint.anchor = Vector2.zero;
                turretActor.HingeJoint.connectedAnchor = placeholder.transform.position;

                ref var trackerComponent = ref turret.Get<TrackerComponent>();
                trackerComponent.searchRadius = turretInitData.TrackerRange;
                trackerComponent.selfTeam = Teams.Player;
                trackerComponent.selfTransform = turretActor.transform;

                ref var rotatableComponent = ref turret.Get<RigidbodyRotatableComponent>();
                rotatableComponent.rigidbody = turretActor.Rigidbody2D;
                rotatableComponent.rotationData = turretInitData.RotationData;

                ref var detectionComponent = ref turret.Get<DetectionComponent>();
                detectionComponent.angle = turretInitData.DetectAngle;
                detectionComponent.radius = turretInitData.DetectRadius;

                turret.Get<FollowComponent>();

                ref var turretComponent = ref turret.Get<TurretComponent>();

                turretComponent.weapons = new List<EcsEntity>();

                var weaponBuilder = new WeaponBuilder(_world);

                foreach (var position in turretActor.WeaponPositions)
                {
                    var weapon = weaponBuilder.Build(turretInitData.WeaponInitData, turretActor.transform,
                        position);
                    turretComponent.weapons.Add(weapon);
                }

                return turretActor;
        }
    }
}