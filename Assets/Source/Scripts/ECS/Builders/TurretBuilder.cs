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
    public class TurretBuilder : EcsBuilder
    {
        public TurretBuilder(EcsWorld world) : base(world)
        {
        }

        public TurretActor CreateTurret(TurretInitConfig turretInitConfig, Transform placeholder, Rigidbody2D connectBody)
        {
                var turretActor = Object.Instantiate(
                    turretInitConfig.TurretPrefab,
                    placeholder.transform.position,
                    Quaternion.identity);

                var turret = _world.NewEntity();
                turretActor.HingeJoint.connectedBody = connectBody;
                turretActor.HingeJoint.anchor = Vector2.zero;
                turretActor.HingeJoint.connectedAnchor = placeholder.transform.position;

                ref var trackerComponent = ref turret.Get<TrackerComponent>();
                trackerComponent.searchRadius = turretInitConfig.TrackerRange;
                trackerComponent.selfTeam = Teams.Player;
                trackerComponent.selfTransform = turretActor.transform;

                ref var rotatableComponent = ref turret.Get<RigidbodyRotatableComponent>();
                rotatableComponent.rigidbody = turretActor.Rigidbody2D;
                rotatableComponent.rotationData = turretInitConfig.RotationData;

                ref var detectionComponent = ref turret.Get<DetectionComponent>();
                detectionComponent.angle = turretInitConfig.DetectAngle;
                detectionComponent.radius = turretInitConfig.DetectRadius;

                turret.Get<FollowComponent>();

                ref var turretComponent = ref turret.Get<TurretComponent>();

                turretComponent.weapons = new List<EcsEntity>();

                var weaponBuilder = new WeaponBuilder(_world);

                foreach (var position in turretActor.WeaponPositions)
                {
                    var weapon = weaponBuilder.Build(turretInitConfig.WeaponInitConfig, turretActor.transform,
                        position);
                    turretComponent.weapons.Add(weapon);
                }

                return turretActor;
        }
    }
}