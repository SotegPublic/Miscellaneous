using Engine.Timer;
using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public class WeaponAbilityModel: IAbilityDataForExecutor
    {
        public event Action OnCooldownEnd;
        public Action<int, Unit, bool> ExecuteCallback { get; private set; }

        [SerializeField][HideInInspector] private int _abilityID;
        private string _abilityName;
        private string _abilityDiscription;
        private float _abilityCost;
        [SerializeField][HideInInspector] private float _abilityCooldown;
        private float _abilityRadius;
        private Sprite _abilityIcon;
        private float _masteryRequirement;
        private AmingAbilityTypes _amingAbilityType;
        private TargetTypes _targetType;       
        
        //fillable fields
        private List<Unit> _targetList;
        private Timer _cooldownTimer;
        private Vector3 _castPoint;

        public bool IsOnCoolDown { get; private set; }

        public int AbilityID => _abilityID;
        public string AbilityName => _abilityName;
        public string AbilityDiscription => _abilityDiscription;
        public float AbilityCost => _abilityCost;
        public float AbilityCooldown => _abilityCooldown;
        public float AbilityRadius => _abilityRadius;
        public Sprite AbilityIcon => _abilityIcon;
        public float MasteryRequirement => _masteryRequirement;
        public AmingAbilityTypes AmingAbilityType => _amingAbilityType;
        public List<Unit> TargetList => _targetList;
        public Timer CooldownTimer => _cooldownTimer;
        public TargetTypes TargetType => _targetType;
        public Vector3 CastPoint => _castPoint;

        public WeaponAbilityModel(AbilityConfigurator abilityConfigurator)
        {
            _abilityID = abilityConfigurator.AbilityID;
            _abilityName = abilityConfigurator.AbilityName;
            _abilityDiscription = abilityConfigurator.AbilityDiscription;
            _abilityCost = abilityConfigurator.AbilityCost;
            _abilityCooldown = abilityConfigurator.AbilityCooldown;
            _abilityRadius = abilityConfigurator.ImpactRadius;
            _abilityIcon = abilityConfigurator.AbilityIcon;
            _masteryRequirement = abilityConfigurator.MasteryRequirement;
            _amingAbilityType = abilityConfigurator.AbilityType switch
            {
                AbilityTypes.AoEDamage => AmingAbilityTypes.Zone,
                AbilityTypes.AoEHeal => AmingAbilityTypes.Zone,
                _ => AmingAbilityTypes.Target
            };
            _targetType = abilityConfigurator.TargetType;

            _targetList = new List<Unit>();
            _cooldownTimer = new Timer(ReturnFromCoolDown);
        }

        public void InitCallback(Action<int, Unit, bool> callback)
        {
            ExecuteCallback = callback;
        }

        public void ClearCallback()
        {
            ExecuteCallback = null;
        }

        public void ReturnFromCoolDown()
        {
            IsOnCoolDown = false;
            OnCooldownEnd?.Invoke();
        }

        public void GoToCooldown()
        {
            IsOnCoolDown = true;
            _cooldownTimer.SetNewTimerDuration(_abilityCooldown);
            TimersList.AddTimer(_cooldownTimer);
        }
        public void GoToCooldown(float value)
        {
            IsOnCoolDown = true;
            _cooldownTimer.SetNewTimerDuration(value);
            TimersList.AddTimer(_cooldownTimer);
        }

        public void SetCastPoint(Vector3 point)
        {
            _castPoint = point;
        }

        public void ClearAbilityTargets()
        {
            _targetList.Clear();
        }
    }
}