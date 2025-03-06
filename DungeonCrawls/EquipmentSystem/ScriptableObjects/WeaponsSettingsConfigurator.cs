using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = nameof(WeaponsSettingsConfigurator), menuName = "Equipment/Weapons Settings Config", order = 5)]
    public class WeaponsSettingsConfigurator : ScriptableObject
    {
        [Header("Таблица бонусного урона по типам")]
        [SerializeField] private List<WeaponBonusDamageMapping> _weaponBonusDamageMappings;
        [Header("Таблица базового урона и веса по типам")]
        [SerializeField] private List<WeaponParametersMapping> _weaponParametersMappings;
        [Header("Базовая скорость атаки")]
        [SerializeField] private float _baseAttackSpeed;
        [Header("Модификаторы грейда для расчета Power")]
        [SerializeField] private List<ItemGradePowerModifiers> _weaponGradePowerModifiers;
        [Header("Модификаторы типа урона")]
        [SerializeField] private List<ItemDamageTypeModifiers> _weaponTypeModifiers;

        public List<WeaponBonusDamageMapping> WeaponBonusDamageMappings => _weaponBonusDamageMappings;
        public float BaseAttackSpeed => _baseAttackSpeed;
        public List<ItemGradePowerModifiers> WeaponGradePowerModifiers => _weaponGradePowerModifiers;
        public List<WeaponParametersMapping> WeaponParametersMappings => _weaponParametersMappings;
        public List<ItemDamageTypeModifiers> WeaponTypeModifiers => _weaponTypeModifiers;
    }
}