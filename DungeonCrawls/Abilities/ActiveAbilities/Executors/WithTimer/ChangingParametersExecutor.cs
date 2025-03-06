using AppliedEffectsSystem;
using BattleSystem;
using Units;
using UnityEngine;

namespace Abilities
{
    public class ChangingParametersExecutor : AbilityExecutorWithTimer, ITimerExecutor
    {
        public ChangingParametersExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(unit, target, point, model, battleController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
            ApplyChanging();
        }

        private void ApplyChanging()
        {
            for (int i = 0; i < _abilityModel.ChengableParameters.Count; i++)
            {
                var curentValue = _target.UnitParameters.UnitParametersList.Find(_ => _.ParameterType == _abilityModel.ChengableParameters[i].ImpactParameter).Value;
                var newValue = curentValue + _abilityModel.ChengableParameters[i].Impact;
                _battleController.ChangeAttributeController.ChangeParameter(_owner, _target,
                    _abilityModel.ChengableParameters[i].ImpactParameter, newValue);
            }
        }

        private void DeclineChanging()
        {
            for (int i = 0; i < _abilityModel.ChengableParameters.Count; i++)
            {
                var curentValue = _target.UnitParameters.UnitParametersList.Find(_ => _.ParameterType == _abilityModel.ChengableParameters[i].ImpactParameter).Value;
                var newValue = curentValue - _abilityModel.ChengableParameters[i].Impact;
                _battleController.ChangeAttributeController.ChangeParameter(_owner, _target,
                    _abilityModel.ChengableParameters[i].ImpactParameter, newValue);
            }
        }

        public override void Clear()
        {
            DeclineChanging();
            base.Clear();
        }
    }
}