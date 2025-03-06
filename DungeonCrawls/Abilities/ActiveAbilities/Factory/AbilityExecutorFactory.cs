using AppliedEffectsSystem;
using BattleSystem;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AbilityExecutorFactory
    {
        private BattleController _battleController;
        private AppliedEffectsUIController _appliedEffectsUIController;

        public AbilityExecutorFactory(BattleController battleController, AppliedEffectsUIController appliedEffectsUIController)
        {
            _battleController = battleController;
            _appliedEffectsUIController = appliedEffectsUIController;
        }

        public T CreateExecutor<T>(Unit unit, Unit target, Vector3 castPoint, AbilityModel model) where T: IExecutor
        {
            IExecutor executor = null;

            switch (model.AbilityType)
            {
                case AbilityTypes.TargetDamage:
                case AbilityTypes.AoEDamage:
                    executor = new AdditiveDamageAbilityExecutor(unit, target, castPoint, model, _battleController);
                    break;
                case AbilityTypes.TargetHeal:
                case AbilityTypes.AoEHeal:
                    executor = new AdditiveHealAbilityExecutor(unit, target, castPoint, model, _battleController);
                    break;
                case AbilityTypes.MultiplicativeTargetDamage:
                    executor = new MultiplicativeDamageAbilityExecutor(unit, target, castPoint, model, _battleController);
                    break;
                case AbilityTypes.HoT:
                    executor = new HoTAbility(unit, target, castPoint, model, _battleController, _appliedEffectsUIController);
                    break;
                case AbilityTypes.DoT:
                    executor = new DoTAbility(unit, target, castPoint, model, _battleController, _appliedEffectsUIController);
                    break;
                case AbilityTypes.Buff:
                case AbilityTypes.Debuff:
                    executor = new ChangingParametersExecutor(unit, target, castPoint, model, _battleController, _appliedEffectsUIController);
                    break;
                case AbilityTypes.None:
                default:
                    break;
            }

            return (T)executor;
        }
    }
}