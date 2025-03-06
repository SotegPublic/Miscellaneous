using DG.Tweening;
using Engine;
using InGameants;
using InGameConstants;
using Pause;
using System.Collections.Generic;
using System.Linq;
using Units;
using Units.Animation;
using UnityEngine;
using UserInput;

namespace PassiveAbilities
{
    public class PassiveAbilityAnimationsController : IPauseHandler, ILateUpdatable, ICleanable, IController
    {
        private PassiveAbilityAnimationsManager _animationsManager;
        private List<IVisualEffect> _activatedEffects;
        private Dictionary<Unit, List<IVisualEffect>> _cancelableEffects;
        private Dictionary<IVisualEffect, Unit> _unitsWithEffects;

        private IVisualEffect _currentEff;

        public PassiveAbilityAnimationsController(UserInputSystem input, PassiveAbilityAnimationsManager animationsManager)
        {
            _animationsManager = animationsManager;
            _activatedEffects = new List<IVisualEffect>();
            _cancelableEffects = new Dictionary<Unit, List<IVisualEffect>>();
            _unitsWithEffects = new Dictionary<IVisualEffect, Unit>();

            //убрать
            var units = Object.FindObjectsOfType<Unit>().ToList();
            var unit = units.Find(player => player.FractionID == 1);

            input.AbilitiesUse.Slot01.performed += (context) => ActivateVisualEffect(unit.transform, 2002, 2);
            //input.AbilitiesUse.Slot02.performed += (context) => ActivateVisualEffect(unit, 2001);

            input.AbilitiesUse.Slot03.performed += (context) => CancelVisualEffect(_currentEff.InstanceID);
            //
        }

        public void SetPause(bool isPaused)
        {
            if (isPaused)
            {
                DOTween.Pause(StringConstants.PASSIVE_ABILITY_VISUAL_EFFECT);
            }
            else
            {
                DOTween.Play(StringConstants.PASSIVE_ABILITY_VISUAL_EFFECT);
            }
        }

        /// <summary>
        /// Activate visual effect at the target position, return effect GO instanceID (return 0, if NO_ANIMATION_EFFECT_ID)
        /// </summary>
        /// <param name="unit">
        /// The target for positioning visual effect GO
        /// </param>
        /// <param name="effectID">ID of statusEffect</param>
        public int ActivateVisualEffect(Unit unit, int effectID, float duration = default)
        {
            if (effectID == PassiveAbilityConfigurator.NO_ANIMATION_EFFECT_ID)
            {
                return default;
            }

            var effect = _animationsManager.GetVisualEffect(effectID);

            if (_cancelableEffects.ContainsKey(unit))
            {
                var effects = _cancelableEffects[unit];

                if (_cancelableEffects[unit].Contains(effect))
                {
                    var currentEffect = effects.Find(effect => effect.EffectID == effectID);
                    currentEffect.CancelEffect();
                    effects.Add(effect);
                }
                else
                {                   
                    effects.Add(effect);
                }
            }
            else
            {
                var effectsList = new List<IVisualEffect>();
                effectsList.Add(effect);
                _cancelableEffects.Add(unit, effectsList);
                unit.OnUnitDeath += CancelAllUnitEffects;
            }

            effect.ActivateEffect(unit.Transform, default, duration);
            _activatedEffects.Add(effect);
            _unitsWithEffects.Add(effect, unit);

            effect.OnEffectTimeEnd += DeactivateVisualEffect;

            return effect.InstanceID;
                
        }

        /// <summary>
        /// Activate visual effect at the target position, return effect GO instanceID (return 0, if NO_ANIMATION_EFFECT_ID)
        /// </summary>
        /// <param name="pointTransform">
        /// Transform of the floorCell, the center of the particle circle(check the pivot of cellPrefab is in the center of GO)
        /// </param>
        /// <param name="effectID">
        /// ID of statusEffect
        /// </param>
        public int ActivateVisualEffect(Transform pointTransform, int effectID, float duration = default)
        {
            if (effectID == PassiveAbilityConfigurator.NO_ANIMATION_EFFECT_ID)
            {
                return default;
            }

            var effect = _animationsManager.GetVisualEffect(effectID);

            effect.ActivateEffect(pointTransform, default, duration);
            _activatedEffects.Add(effect);

            effect.OnEffectTimeEnd += DeactivateVisualEffect;

            return effect.InstanceID;
        }

        /// <summary>
        /// Activate visual effect at the target position, return effect GO instanceID (return 0, if NO_ANIMATION_EFFECT_ID)
        /// </summary>
        /// <param name="pointTransform">
        /// Transform of the floorCell, the center of the particle circle(check the pivot of cellPrefab is in the center of GO)
        /// </param>
        /// <param name="effectID">
        /// ID of statusEffect
        /// </param>
        /// <param name="radius">
        /// radius of the particle circle
        /// </param>
        public int ActivateVisualEffect(Transform pointTransform, int effectID, float radius, float duration = default)
        {
            if (effectID == PassiveAbilityConfigurator.NO_ANIMATION_EFFECT_ID)
            {
                return default;
            }

            var effect = _animationsManager.GetVisualEffect(effectID);

            effect.ActivateEffect(pointTransform, radius, duration);
            _activatedEffects.Add(effect);

            effect.OnEffectTimeEnd += DeactivateVisualEffect;

            _currentEff = effect; // <== для тестов

            return effect.InstanceID;
        }

        private void DeactivateVisualEffect(IVisualEffect effect)
        {
            _activatedEffects.Remove(effect);
            _animationsManager.DeactivateVisualEffect(effect);

            if (_unitsWithEffects.ContainsKey(effect))
            {
                var effects = _cancelableEffects[_unitsWithEffects[effect]];
                effects.Remove(effect);                
                _unitsWithEffects.Remove(effect);
            }

            effect.OnEffectTimeEnd -= DeactivateVisualEffect;
        }

        public void CancelVisualEffect(int instanceID)
        {   
            var effect = _activatedEffects.Find(effect => effect.InstanceID == instanceID);

            if(effect != null)
            {
                effect.CancelEffect();
            }
        }

        private void CancelAllUnitEffects(Vector3 notUsed, Unit unit)
        {
            if (!_cancelableEffects.ContainsKey(unit)) return;

            var effects = _cancelableEffects[unit];

            foreach (var effect in effects)
            {
                effect?.CancelEffect();
            }
        }

        public void LocalLateUpdate(float deltaTime)
        {
            if(_activatedEffects.Count > 0)
            {
                foreach (var effect in _activatedEffects)
                {
                    effect.SetPosition();
                }
            }
        }

        public void CleanUp()
        {
            if(_cancelableEffects.Count > 0)
            {
                foreach(var KeyValuePair in _cancelableEffects)
                {
                    KeyValuePair.Key.OnUnitDeath -= CancelAllUnitEffects;
                }
            }
        }
    }
}