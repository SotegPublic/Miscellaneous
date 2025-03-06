using Engine;
using PassiveAbilities;
using Pause;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserInput;
using Utils;

namespace PassiveAbilities
{
    public class PassiveAbilityAnimationsInitialization
    {
        public PassiveAbilityAnimationsController PassiveAbilityAnimationsController { get; private set; }

        public PassiveAbilityAnimationsInitialization(UserInputSystem input, GlobalConfigLoader globalConfig,
                    ControllersManager controllersManager, PauseManager pauseManager)
        {
            var passiveAbilityVisualEffectsFactory = new PassiveAbilityAnimationsFactory(globalConfig);
            var passiveAbilityAnimationsManager = new PassiveAbilityAnimationsManager(passiveAbilityVisualEffectsFactory);
            PassiveAbilityAnimationsController = new PassiveAbilityAnimationsController(input, passiveAbilityAnimationsManager);

            pauseManager.Register(PassiveAbilityAnimationsController);
        }
    }
}