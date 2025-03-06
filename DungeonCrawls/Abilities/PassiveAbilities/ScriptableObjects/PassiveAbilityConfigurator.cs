using Abilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PassiveAbilities
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(PassiveAbilityConfigurator), menuName = "PassiveAbility/PassiveAbilityConfigurator", order = 0)]
    public class PassiveAbilityConfigurator : ScriptableObject
    {
        //main parameters
        [SerializeField] public int PassiveAbilityID;
        [SerializeField] public string PassiveAbilityName;
        [SerializeField] public string PassiveAbilityDiscription;
        [SerializeField] public PassiveAbilityTypes PassiveAbilityType;
        [SerializeField] public Sprite PassiveAbilityIcon;
        [SerializeField] public AudioClip PassiveAbilitySound;
        [SerializeField][HideInInspector] public int PassiveAbilityAnimationEffectID;
        [SerializeField][HideInInspector] public string AnimationEffectName;

        //secondary parameters
        [SerializeField] public float Impact;
        [SerializeField] public float ImpactDuration;
        [SerializeField] public int ImpactCounts;
        [SerializeField] public float ImpactRadius;
        [SerializeField] public int MaxStackCount;
        [SerializeField] public Sprite AbilityEffectIcon;
        [SerializeField] public List<ChangableParameter> ChengableParameters = new List<ChangableParameter>();
        [SerializeField] public bool IsHaveStatusEffect; 
        [SerializeField] public StatusEffectConfigurator PassiveAbilityStatusEffect;

        public const int NO_ANIMATION_EFFECT_ID = -1;

        public void ClearConfiguratorSecondaryParameters()
        {
            Impact = 0;
            ImpactDuration = 0;
            ImpactCounts = 0;
            ImpactRadius = 0;
            MaxStackCount = 0;
            ChengableParameters.Clear();
            AbilityEffectIcon = null;
        }

        public void AddChangableParameter()
        {
            ChengableParameters.Add(new ChangableParameter());
        }

        public void RemoveLastChangableParameter()
        {
            if (ChengableParameters.Count == 0) return;

            int lastIndex = ChengableParameters.Count - 1;
            ChengableParameters.Remove(ChengableParameters[lastIndex]);
        }

        public void ClearChangableParameters()
        {
            ChengableParameters.Clear();
        }

        public void ClearStatusEffect()
        {
            PassiveAbilityStatusEffect = null;
        }
    }
}