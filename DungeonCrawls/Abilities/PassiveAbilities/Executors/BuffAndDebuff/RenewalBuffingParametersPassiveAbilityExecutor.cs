using AppliedEffectsSystem;
using BattleSystem;
using Units;

namespace PassiveAbilities
{
    public class RenewalBuffingParametersPassiveAbilityExecutor : RenewalPassiveAbilityExecutor
    {
        private float _valueBeforeChange;
        public RenewalBuffingParametersPassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController, AppliedEffectsUIController appliedEffectsUIController) :
            base(unit, target, model, battleController, statusEffectsController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
            ApplyChanging();
        }

        private void ApplyChanging()
        {
            for (int i = 0; i < _passiveAbilityProtoModel.ChengableParameters.Count; i++)
            {
                _valueBeforeChange = _target.UnitParameters.UnitParametersList.Find(_ => _.ParameterType == _passiveAbilityProtoModel.ChengableParameters[i].ImpactParameter).Value;
                var newValue = _valueBeforeChange * (1 + _passiveAbilityProtoModel.ChengableParameters[i].Impact);
                _battleController.ChangeAttributeController.ChangeParameter(_owner, _target,
                    _passiveAbilityProtoModel.ChengableParameters[i].ImpactParameter, newValue);
            }
        }

        private void DeclineChanging()
        {
            for (int i = 0; i < _passiveAbilityProtoModel.ChengableParameters.Count; i++)
            {
                var curentValue = _target.UnitParameters.UnitParametersList.Find(_ => _.ParameterType == _passiveAbilityProtoModel.ChengableParameters[i].ImpactParameter).Value;
                var restoredValue = curentValue - (_valueBeforeChange * _passiveAbilityProtoModel.ChengableParameters[i].Impact);
                _battleController.ChangeAttributeController.ChangeParameter(_owner, _target,
                    _passiveAbilityProtoModel.ChengableParameters[i].ImpactParameter, restoredValue);
            }
        }

        public override void ClearExecutor()
        {
            DeclineChanging();
            OnPassiveAbilityExecutorEnded?.Invoke(_target, this);
            base.ClearExecutor();
        }
    }
}