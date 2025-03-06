using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = nameof(ArmorComposite), menuName = "Equipment/ArmorList", order = 2)]
    public class ArmorComposite : ScriptableObject
    {
        [SerializeField] private List<ArmorConfigurator> _armorList;

        public List<ArmorConfigurator> ArmorList => _armorList;
    }
}