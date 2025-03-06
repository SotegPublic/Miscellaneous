using BattleSystem;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class AbilityExecutor : IExecutor
    {
        protected Unit _owner;
        protected Unit _target;
        protected Vector3 _castPoint;
        protected AbilityModel _abilityModel;
        protected BattleController _battleController;

        public int AbilityID => _abilityModel.AbilitiID;
        public Unit Owner => _owner;
        public Unit Target => _target;

        protected AbilityExecutor(Unit unit, Unit target, Vector3 pont, AbilityModel model, BattleController battleController)
        {
            _owner = unit;
            _target = target;
            _castPoint = pont;
            _abilityModel = model;
            _battleController = battleController;
        }

        public virtual void UseAbility()
        {
            Execute();
        }

        public virtual void Clear()
        {
        }

        protected abstract void Execute();
    }
}