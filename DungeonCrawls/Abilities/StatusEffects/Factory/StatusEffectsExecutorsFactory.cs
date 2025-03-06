using AppliedEffectsSystem;
using BattleSystem;
using Units;
using UnityEngine;

namespace PassiveAbilities
{
    public class StatusEffectsExecutorsFactory
    {
        private BattleController _battleController;
        private AppliedEffectsUIController _appliedEffectsUIController;
        private StatusBaseParametersKeeper _statusBaseParametersKeeper;

        public StatusEffectsExecutorsFactory(BattleController battleController, AppliedEffectsUIController appliedEffectsUIController, StatusBaseParametersKeeper statusBaseParametersKeeper)
        {
            _battleController = battleController;
            _appliedEffectsUIController = appliedEffectsUIController;
            _statusBaseParametersKeeper = statusBaseParametersKeeper;
        }

        public IStatusEffectExecutor CreateExecutor(Unit target, StatusEffectProtoModel model)
        {
            IStatusEffectExecutor executor = null;
            var statusExecutorModel = new StatusExecutorModel(model, _statusBaseParametersKeeper.GetStatusBaseParameters(model.StatusType));

            switch (statusExecutorModel.StatusType)
            {
                case StatusTypes.Bleeded:
                case StatusTypes.DeepWounded:
                    executor = new DoTStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.Crushed:
                    executor = new CrushingStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.Stuned:
                    executor = new StunnedStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.Trauma:
                    executor = new TraumedStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.Marked:
                    executor = new MarkedStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.WeakToMagic:
                    executor = new WeakToMagicStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.TornSoul:
                    executor = new TornSoulStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.Blinded:
                    executor = new BlindedStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.Cursed:
                    executor = new CursedStatusEffectExecutor(target, statusExecutorModel, _battleController, _appliedEffectsUIController);
                    break;
                case StatusTypes.None:
                default:
                    Debug.LogError("Unknow Status Effect Type");
                    break;
            }

            return executor;
        }
    }
}