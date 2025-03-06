using AppliedEffectsSystem;
using Engine.Timer;
using System;
using BattleSystem;
using Units;
using UnityEngine;
using PassiveAbilities;

namespace Abilities
{
    public abstract class AoEAbilityExecutorWithTimer : AbilityExecutor, ITimerExecutor
    {
        protected Timer _abilityTimer;
        protected AppliedEffectsUIController _appliedEffectsUIController;
        protected DisplayedEffectModelForUI _displayedEffectModel;

        public Action<Unit> OnAbilityTimerEnd { get; set; }
        public Timer AbilityTimer => _abilityTimer;

        protected AoEAbilityExecutorWithTimer(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(unit, target, point, model, battleController)
        {
            _abilityTimer = new Timer(Clear);
            _appliedEffectsUIController = appliedEffectsUIController;
            _displayedEffectModel = new DisplayedEffectModelForUI(_abilityModel.AbilityName, _abilityModel.AbilityDescription,
                _abilityModel.AbilityEffectIcon, _owner);
        }

        public override void Clear()
        {
            OnAbilityTimerEnd?.Invoke(_target);
            if (_abilityTimer != null)
            {
                _abilityTimer.CancelTimer();
                _abilityTimer = null;
            }
            base.Clear();
        }

        public virtual void ReactivateAbility()
        {
            _abilityTimer.CancelTimer();
            _abilityTimer = new Timer(Clear);
            _abilityTimer.SetNewTimerDuration(_abilityModel.ImpactDuration);
            TimersList.AddTimer(_abilityTimer);
        }

        public override void UseAbility()
        {
            _abilityTimer.SetNewTimerDuration(_abilityModel.ImpactDuration);
            TimersList.AddTimer(_abilityTimer);

            base.UseAbility();
        }
    }
}