using Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace PassiveAbilities
{
    public class PassiveAbilityProtoModel
    {
        private readonly int _passiveAbilityID;
        private readonly string _passiveAbilityName;
        private readonly string _passiveAbilityDescription;
        private readonly PassiveAbilityTypes _passiveAbilityType;
        private readonly Sprite _passiveAbilityIcon;
        private readonly float _impact;
        private readonly float _impactDuration;
        private readonly int _impactCounts;
        private readonly float _impactRadius;
        private readonly int _maxStackCount;
        private readonly Sprite _passiveAbilityEffectIcon;
        private readonly AudioClip _passiveAbilityAudioClip;
        private readonly List<ChangableParameter> _chengableParameters = new List<ChangableParameter>();
        private readonly int _statusEffectID = NO_STATUS_EFFECT_ID;
        private readonly int _animationEffectID;

        public const int NO_STATUS_EFFECT_ID = -1;

        public int PassiveAbilityID => _passiveAbilityID;
        public string PassiveAbilityName => _passiveAbilityName;
        public string PassiveAbilityDescription => _passiveAbilityDescription;
        public PassiveAbilityTypes PassiveAbilityType => _passiveAbilityType;
        public Sprite PassiveAbilityIcon => _passiveAbilityIcon;
        public float Impact => _impact;
        public float ImpactDuration => _impactDuration;
        public int ImpactCounts => _impactCounts;
        public float ImpactRadius => _impactRadius;
        public int MaxStackCount => _maxStackCount;
        public Sprite PassiveAbilityEffectIcon => _passiveAbilityEffectIcon;
        public AudioClip PassiveAbilityAudioClip => _passiveAbilityAudioClip;
        public List<ChangableParameter> ChengableParameters => _chengableParameters;
        public int StatusEffectID => _statusEffectID;
        public int AnimationEffectID => _animationEffectID;

        public PassiveAbilityProtoModel(PassiveAbilityConfigurator passiveAbilityConfigurator) 
        {
            _passiveAbilityID = passiveAbilityConfigurator.PassiveAbilityID;
            _passiveAbilityName = passiveAbilityConfigurator.PassiveAbilityName;
            _passiveAbilityDescription = passiveAbilityConfigurator.PassiveAbilityDiscription;
            _passiveAbilityType = passiveAbilityConfigurator.PassiveAbilityType;
            _passiveAbilityIcon = passiveAbilityConfigurator.PassiveAbilityIcon;
            _impact = passiveAbilityConfigurator.Impact;
            _impactDuration= passiveAbilityConfigurator.ImpactDuration;
            _impactCounts = passiveAbilityConfigurator.ImpactCounts;
            _impactRadius = passiveAbilityConfigurator.ImpactRadius;
            _maxStackCount= passiveAbilityConfigurator.MaxStackCount;
            _passiveAbilityEffectIcon = passiveAbilityConfigurator.AbilityEffectIcon;
            _passiveAbilityAudioClip = passiveAbilityConfigurator.PassiveAbilitySound;
            _chengableParameters = passiveAbilityConfigurator.ChengableParameters;
            _animationEffectID = passiveAbilityConfigurator.PassiveAbilityAnimationEffectID;
            if(passiveAbilityConfigurator.PassiveAbilityStatusEffect != null)
            {
                _statusEffectID = passiveAbilityConfigurator.PassiveAbilityStatusEffect.StatusEffectID;
            }
        }

        public void Clear()
        {
            ChengableParameters.Clear();
        }
    }

}