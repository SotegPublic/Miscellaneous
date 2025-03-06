using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = nameof(WeaponComposite), menuName = "Equipment/WeaponList", order = 2)]
    public class WeaponComposite : ScriptableObject
    {
        [SerializeField] private List<WeaponConfigurator> _weaponList;

        public List<WeaponConfigurator> WeaponList => _weaponList;
    }
}