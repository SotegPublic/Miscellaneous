using Abilities;
using AppliedEffectsSystem;
using BattleSystem;
using Units;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace PassiveAbilities
{
    public class PassiveAbilityExecutorFactory
    {
        private BattleController _battleController;
        private StatusEffectsController _statusEffectsController;
        private AppliedEffectsUIController _appliedEffectsUIController;

        public PassiveAbilityExecutorFactory(BattleController battleController, StatusEffectsController statusEffectsController, AppliedEffectsUIController appliedEffectsUIController)
        {
            _battleController = battleController;
            _statusEffectsController = statusEffectsController;
            _appliedEffectsUIController = appliedEffectsUIController;
        }

        public T CreateExecutor<T>(Unit caster, Unit target, PassiveAbilityProtoModel model) where T : IPassiveAbilityExecutor
        {
            IPassiveAbilityExecutor executor = null;

            switch (model.PassiveAbilityType)
            {
                case PassiveAbilityTypes.TargetDamage:
                case PassiveAbilityTypes.AoEDamageAroundTarget:
                case PassiveAbilityTypes.AoEDamageAroundCaster:
                    executor = new AdditiveDamagePassiveAbilityExecutor(caster, target, model, _battleController, _statusEffectsController);
                    break;
                case PassiveAbilityTypes.MultiplicativeTargetDamage:
                    executor = new MultiplicativeDamagePassiveAbilityExecutor(caster, target, model, _battleController, _statusEffectsController);
                    break;
                case PassiveAbilityTypes.RandomTargetHealAroundCaster:
                case PassiveAbilityTypes.RandomTargetHealAroundTarget:
                case PassiveAbilityTypes.AoEHealAroundCaster:
                case PassiveAbilityTypes.AoEHealAroundTarget:
                    executor = new AdditiveHealPassiveAbilityExecutor(caster, target, model, _battleController, _statusEffectsController);
                    break;
                case PassiveAbilityTypes.RandomRenewalBuffAroundCaster:
                case PassiveAbilityTypes.RandomRenewalBuffAroundTarget:
                    executor = new RenewalBuffingParametersPassiveAbilityExecutor(caster, target, model, _battleController, _statusEffectsController, _appliedEffectsUIController);
                    break;
                case PassiveAbilityTypes.RenewalTargetDebuff:
                    executor = new RenewalDebuffingParametersPassiveAbilityExecutor(caster, target, model, _battleController, _statusEffectsController, _appliedEffectsUIController);
                    break;
                case PassiveAbilityTypes.RenewalHoTAroundCaster:
                case PassiveAbilityTypes.RenewalHoTAroundTarget:
                    executor = new RenewalHoTPassiveAbilityExecutor(caster, target, model, _battleController, _statusEffectsController, _appliedEffectsUIController);
                    break;
                case PassiveAbilityTypes.RenewalTargetDoT:
                    executor = new RenewalDoTPassiveAbilityExecutor(caster, target, model, _battleController, _statusEffectsController, _appliedEffectsUIController);
                    break;
                case PassiveAbilityTypes.Provoke:
                case PassiveAbilityTypes.AOEProvoke:
                    executor = new TauntPassiveAbilityExecutor(caster, target, model, _battleController, _statusEffectsController, _appliedEffectsUIController);
                    break;
                case PassiveAbilityTypes.StackableRandomBuffAroundCaster:
                case PassiveAbilityTypes.StackableRandomBuffAroundTarget:
                    break;
                case PassiveAbilityTypes.StackableDebuff:
                    break;
                case PassiveAbilityTypes.StackableTargetDoT:
                    break;
                case PassiveAbilityTypes.None:
                default:
                    break;
            }

            return (T)executor;
        }
     }
}