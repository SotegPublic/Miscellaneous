using Engine;
using System;
using System.Collections.Generic;
using BattleSystem;
using Units;
using UnitsControlSystem;
using UnityEngine;

namespace Abilities
{
    public class InstantExecutorsCreator : ExecutorsCreator
    {

        public InstantExecutorsCreator(Unit unit, List<Unit> targets, Vector3 pont, AbilityModel model, BattleController battleController,
                                  Action<int, Unit, bool> executeCallback, MoveUnitsController moveUnitsController, AbilityExecutorFactory abilityExecutorFactory,
                                  AbilitiesController abilitiesController, StaminaParametersController staminaParametersController) :
                                  base(unit, targets, pont, model, battleController, executeCallback, moveUnitsController, abilityExecutorFactory, abilitiesController,
                                       staminaParametersController)
        {

        }

        protected override void CreateAndExecuteAbilityExecutors()
        {
            base.CreateAndExecuteAbilityExecutors();
            for (int i = 0; i < _targets.Count; i++)
            {
                var executor = _abilityExecutorFactory.CreateExecutor<IExecutor>(_owner, _targets[i], _targets[i].transform.position, _abilityModel);

                executor.UseAbility();
            }

            Clear();
        }

        public override void Clear()
        {
            base.Clear();
            _abilitiesController.InstantExecutorsCreators[_abilityModel.AbilitiID].Remove(this);
        }
    }
}