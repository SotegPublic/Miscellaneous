using BattleSystem;
using Units;

namespace PassiveAbilities
{
    public abstract class PassiveAbilityExecutor : IPassiveAbilityExecutor
    {
        protected Unit _owner;
        protected Unit _target;
        protected PassiveAbilityProtoModel _passiveAbilityProtoModel;
        protected BattleController _battleController;
        protected StatusEffectsController _statusEffectsController;

        public int PassiveAbilityID => _passiveAbilityProtoModel.PassiveAbilityID;
        public Unit Owner => _owner;
        public Unit Target => _target;

        protected PassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController, StatusEffectsController statusEffectsController)
        {
            _owner = unit;
            _target = target;
            _passiveAbilityProtoModel = model;
            _battleController = battleController;
            _statusEffectsController = statusEffectsController;
        }

        public virtual void ActivateExecutor()
        {
            Execute();
        }

        public virtual void ClearExecutor()
        {
        }

        protected abstract void Execute();
    }

}