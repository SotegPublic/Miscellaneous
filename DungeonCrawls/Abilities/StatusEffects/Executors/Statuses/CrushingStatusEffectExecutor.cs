using AppliedEffectsSystem;
using BattleSystem;
using Units;
using Units.UnitsParameters;

namespace PassiveAbilities
{
    public class CrushingStatusEffectExecutor: ChangableParametersStatusEffectExecutor
    {
        public CrushingStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController): base(target, model, battleController, appliedEffectsUIController)
        {
        }

        protected override void ApplyStatusEffect()
        {
            _valueBeforeChange = _target.UnitParameters.UnitDefense.DefenseParameter.Value;
            var changedValue = _valueBeforeChange * (1 - _statusExecutorModel.StatusEffectValue);
            _battleController.ChangeAttributeController.ChangeParameter(_target,
                ParameterTypes.TotalDefence, changedValue);
        }

        protected override void DeclineStatusEffect()
        {
            var curentValue = _target.UnitParameters.UnitDefense.DefenseParameter.Value;
            var restoredValue = curentValue + (_valueBeforeChange * _statusExecutorModel.StatusEffectValue);
            _battleController.ChangeAttributeController.ChangeParameter(_target,
                ParameterTypes.TotalDefence, restoredValue);
        }
    }
}