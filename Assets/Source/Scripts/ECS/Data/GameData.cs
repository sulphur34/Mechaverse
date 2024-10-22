using Data;
using UnityEngine;

namespace ECS.Data
{
    [CreateAssetMenu(menuName = "GameConfigs")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField] public UnitInitConfig EnemyInitConfig {get; private set;}
        [field: SerializeField] public UnitInitConfig PlayerInitConfig {get; private set;}
        [field: SerializeField] public WeaponInitConfig TurretWeaponInitConfig {get; private set;}
        [field: SerializeField] public WeaponInitConfig MainWeaponInitConfig {get; private set;}
        [field: SerializeField] public TurretInitConfig TurretInitConfig {get; private set;}
        [field: SerializeField] public PickUpsInitConfig PickUpsInitConfig {get; private set;}
    }
}