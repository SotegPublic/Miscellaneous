using AppliedEffectsSystem;
using BattleSystem;
using Engine;
using Units;

namespace PassiveAbilities
{
    public class WeakToMagicStatusEffectExecutor : StatusEffectExecutor
    {
        private TargetsSeeker _targetsSeeker;
        public WeakToMagicStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(target, model, battleController, appliedEffectsUIController)
        {
            _targetsSeeker = new TargetsSeeker();
        }

        public void ActivateWeakToMagicComboEffect(Unit target, int fractionID)
        {
            var targets = _targetsSeeker.FindAllEnemyTargetsInRadius(target.transform.position, _statusExecutorModel.StatusComboRadius, fractionID);

            for (int i = 0; i < targets.Count; i++)
            {
                _battleController.AbilitiesActionController.DealDamage(targets[i], _statusExecutorModel.StatusComboValue, true, _statusExecutorModel.StatusEffectName);
            }
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