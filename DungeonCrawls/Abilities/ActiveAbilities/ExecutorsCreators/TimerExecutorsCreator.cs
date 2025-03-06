using Engine;
using System;
using System.Collections.Generic;
using BattleSystem;
using Units;
using UnitsControlSystem;
using UnityEngine;

namespace Abilities
{
    public class TimerExecutorsCreator : ExecutorsWithDurationCreator<ITimerExecutor>
    {
        public TimerExecutorsCreator(Unit unit, List<Unit> targets, Vector3 pont, AbilityModel model, BattleController battleController,
                          Action<int, Unit, bool> executeCallback, MoveUnitsController moveUnitsController, AbilityExecutorFactory abilityExecutorFactory,
                          AbilitiesController abilitiesController, StaminaParametersController staminaParametersController) :
                          base(unit, targets, pont, model, battleController, executeCallback, moveUnitsController, abilityExecutorFactory, abilitiesController,
                               staminaParametersController)
        {
        }

        public override void Clear()
        {
            base.Clear();
            _abilitiesController.TimerExecutorsCreators[_abilityModel.AbilitiID].Remove(this);
        }
    }
}