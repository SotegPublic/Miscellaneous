using AppliedEffectsSystem;
using BattleSystem;
using Units;
using Units.UnitsParameters;

namespace PassiveAbilities
{
    public class TraumedStatusEffectExecutor : ChangableParametersStatusEffectExecutor 
    {
        public TraumedStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(target, model, battleController, appliedEffectsUIController)
        {
        }

        protected override void ApplyStatusEffect()
        {
            _valueBeforeChange = _target.UnitParameters.UnitAttack.AttackSpeed.Value;
            var changedValue = _valueBeforeChange * (1 - _statusExecutorModel.StatusEffectValue);
            _battleController.ChangeAttributeController.ChangeParameter(_target,
                ParameterTypes.AttackSpeed, changedValue);
        }

        protected override void DeclineStatusEffect()
        {
            var curentValue = _target.UnitParameters.UnitAttack.AttackSpeed.Value;
            var restoredValue = curentValue + (_valueBeforeChange * _statusExecutorModel.StatusEffectValue);
            _battleController.ChangeAttributeController.ChangeParameter(_target,
                ParameterTypes.AttackSpeed, restoredValue);
        }
    }
}