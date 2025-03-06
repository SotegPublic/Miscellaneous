using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    public class AbilityModel
    {
        private int _abilitiID;
        private string _abilityName;
        private string _abilityDescription;
        private float _castCost;
        private float _impact;
        private float _range;
        private AbilityTypes _abilityType;
        private float _impactDuration;
        private int _impactCounts;
        private float _impactFrequency;
        private float _impactRadius;
        private List<ChangableParameter> _chengableParameters = new List<ChangableParameter>();
        private Sprite _abilityIcon;
        private Sprite _abilityEffectIcon;

        public int AbilitiID => _abilitiID;
        public string AbilityName => _abilityName;
        public string AbilityDescription => _abilityDescription;
        public float Impact => _impact;
        public float Range => _range;
        public AbilityTypes AbilityType => _abilityType;
        public float ImpactDuration => _impactDuration;
        public int ImpactCounts => _impactCounts;
        public float ImpactFrequency => _impactFrequency;
        public float ImpactRadius => _impactRadius;
        public List<ChangableParameter> ChengableParameters => _chengableParameters;
        public float CastCost => _castCost;
        public Sprite AbilityIcon => _abilityIcon;
        public Sprite AbilityEffectIcon => _abilityEffectIcon;
            
        public AbilityModel(AbilityConfigurator abilityConfigurator)
        {
            _abilityName = abilityConfigurator.AbilityName;
            _abilityDescription = abilityConfigurator.AbilityDiscription;
            _abilityIcon = abilityConfigurator.AbilityIcon;
            _abilityEffectIcon = abilityConfigurator.AbilityEffectIcon;
            _impact = abilityConfigurator.Impact;
            _abilityType = abilityConfigurator.AbilityType;
            _castCost = abilityConfigurator.AbilityCost;
            _impactDuration = abilityConfigurator.ImpactDuration;
            _impactCounts = abilityConfigurator.ImpactCounts;
            _impactFrequency = abilityConfigurator.ImpactFrequency;
            _impactRadius = abilityConfigurator.ImpactRadius;
            _abilitiID = abilityConfigurator.AbilityID;
            _range = abilityConfigurator.AbilityRange;
            _chengableParameters = abilityConfigurator.ChengableParameters;
        }
    }
}