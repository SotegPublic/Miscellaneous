using Engine;
using System;
using System.Collections.Generic;
using BattleSystem;
using Units;
using UnitsControlSystem;
using UnityEngine;

namespace Abilities
{
    public abstract class ExecutorsCreator
    {        
        protected Unit _owner;
        protected List<Unit> _targets;
        protected Vector3 _castPoint;
        protected AbilityExecutorFactory _abilityExecutorFactory;
        protected AbilityModel _abilityModel;
        protected BattleController _battleController;
        protected AbilitiesController _abilitiesController;
        protected MoveUnitsController _moveUnitsController;
        protected StaminaParametersController _staminaParametersController;
        protected Action<int, Unit, bool> _executeCallback;
        protected bool _isCanceled;
        protected bool _isExecuted;

        public Unit Owner => _owner;
        public bool IsCanceled => _isCanceled;
        public AbilityModel AbilityModel => _abilityModel;

        protected ExecutorsCreator(Unit unit, List<Unit> targets, Vector3 pont, AbilityModel model, BattleController battleController,
                                  Action<int, Unit, bool> executeCallback, MoveUnitsController moveUnitsController, AbilityExecutorFactory abilityExecutorFactory,
                                  AbilitiesController abilitiesController, StaminaParametersController staminaParametersController)
        {
            _targets = new List<Unit>(targets);
            _owner = unit;
            _castPoint = pont;
            _abilityModel = model;
            _battleController = battleController;
            _executeCallback = executeCallback;
            _moveUnitsController = moveUnitsController;
            _abilityExecutorFactory = abilityExecutorFactory;
            _abilitiesController = abilitiesController;
            _staminaParametersController = staminaParametersController;
            _owner.OnBattleEnter += CancelOnChangeTargetBeforExecute;
            _moveUnitsController.OnUnitStartMove += CancelOnMoveBaforeExecute;
        }

        protected virtual void CreateAndExecuteAbilityExecutors()
        {
            _staminaParametersController.DecreaseValue(_owner, _abilityModel.CastCost);
        }

        protected virtual void CreateExecutors(bool isTargetAlive)
        {
            if (_isCanceled || _isExecuted) return;

            if (isTargetAlive)
            {
                _executeCallback?.Invoke(_abilityModel.AbilitiID, _owner, true);
                CreateAndExecuteAbilityExecutors();
                _isExecuted = true;
            }
            else
            {
                CancelExecute();
            }
        }

        protected virtual void CheckDistanceToTarget()
        {
            if (_abilityModel.AbilityType == AbilityTypes.AoEDamage || _abilityModel.AbilityType == AbilityTypes.AoEHeal)
            {
                _battleController.AoeAbilityActivator(_owner, _castPoint, _targets, _abilityModel.Range, CreateExecutors);
            }
            else
            {
                _battleController.TargetAbilityActivator(_owner, _targets, _abilityModel.Range, CreateExecutors);
            }
        }

        protected void CancelOnChangeTargetBeforExecute(Unit unit, Unit target)
        {
            if (!_isCanceled)
            {
                CancelExecute();
            }
        }

        protected void CancelOnMoveBaforeExecute(Unit unit, Transform pont)
        {
            if (unit == _owner && !_isCanceled)
            {
                CancelExecute();
            }
        }

        protected virtual void CancelExecute()
        {
            if (!_isExecuted)
            {
                _executeCallback?.Invoke(_abilityModel.AbilitiID, _owner, false);
                Clear();
                _isCanceled = true;
            }
        }

        public virtual void RunCreator()
        {
            CheckDistanceToTarget();
        }

        public virtual void Clear()
        {
            _owner.OnBattleEnter -= CancelOnChangeTargetBeforExecute;
            _moveUnitsController.OnUnitStartMove -= CancelOnMoveBaforeExecute;
            _executeCallback = null;
        }
    }
}