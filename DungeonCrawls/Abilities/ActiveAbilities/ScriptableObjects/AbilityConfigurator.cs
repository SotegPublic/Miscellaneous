using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(AbilityConfigurator), menuName = "Ability/AbilityConfigurator", order = 0)]
    public class AbilityConfigurator : ScriptableObject
    {
        //main parameters
        [SerializeField] public int AbilityID;
        [SerializeField] public string AbilityName;
        [SerializeField] public string AbilityDiscription;
        [SerializeField] public AbilityTypes AbilityType;
        [SerializeField] public TargetTypes TargetType;
        [SerializeField] public float AbilityCost;
        [SerializeField] public float AbilityCooldown;
        [SerializeField] public float AbilityRange;
        [SerializeField] public Sprite AbilityIcon;
        [SerializeField] public float MasteryRequirement;

        //secondary parameters
        [SerializeField] public float Impact;
        [SerializeField] public float ImpactDuration;
        [SerializeField] public int ImpactCounts;
        [SerializeField] public float ImpactFrequency;
        [SerializeField] public float ImpactRadius;
        [SerializeField] public Sprite AbilityEffectIcon;
        [SerializeField] public List<ChangableParameter> ChengableParameters = new List<ChangableParameter>();

        public void ClearConfiguratorSecondaryParameters()
        {
            Impact = 0;
            ImpactDuration = 0;
            ImpactCounts = 0;
            ImpactRadius = 0;
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
    }
}