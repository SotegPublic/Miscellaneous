using System.Collections.Generic;
using UnityEngine;

namespace PassiveAbilities
{
    [CreateAssetMenu(fileName = nameof(StatusConfiguratorsList), menuName = "StatusEffects/StatusConfiguratorsList", order = 0)]
    public class StatusConfiguratorsList : ScriptableObject
    {
        [SerializeField] public List<StatusCongigurator> StatusCongigurators = new List<StatusCongigurator>();
    }
}