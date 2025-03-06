using AppliedEffectsSystem;
using BattleSystem;
using Engine;
using Units;

namespace PassiveAbilities
{
    public class TornSoulStatusEffectExecutor: StatusEffectExecutor
    {
        private TargetsSeeker _targetsSeeker;
        public TornSoulStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(target, model, battleController, appliedEffectsUIController)
        {
            _targetsSeeker = new TargetsSeeker();
        }

        public void ActivateTornSoulComboEffect(Unit target, int fractionID)
        {
            var targets = _targetsSeeker.FindAllAllyTargetsInRadius(target.transform.position, _statusExecutorModel.StatusComboRadius, fractionID);

            for(int i = 0; i < targets.Count; i++)
            {
                _battleController.AbilitiesActionController.HealUnit(targets[i], _statusExecutorModel.StatusComboValue, _statusExecutorModel.StatusEffectName);
            }
        }

        public void ActivateTornSoulStatusEffect(Unit owner, float dealingDamage)
        {
            _battleController.AbilitiesActionController.HealUnit(owner, dealingDamage, _statusExecutorModel.StatusEffectName);
        }

        protected override void Execute()
        {
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}