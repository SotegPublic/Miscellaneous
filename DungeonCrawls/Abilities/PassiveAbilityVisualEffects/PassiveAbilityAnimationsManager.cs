using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace PassiveAbilities
{
    public class PassiveAbilityAnimationsManager
    {
        private List<IVisualEffect> _effects;
        private int _examplesCount = 20;

        public PassiveAbilityAnimationsManager(PassiveAbilityAnimationsFactory factory)
        {
            GetVisualEffects(factory);
        }       

        private async void GetVisualEffects(PassiveAbilityAnimationsFactory factory)
        {
            _effects = await factory.GetAllTypeAnimations(_examplesCount);
        }

        public IVisualEffect GetVisualEffect(int effectID)
        {
            var visualEffect = _effects.Find(effect => effect.EffectID == effectID);

            if(visualEffect != null)
            {
                _effects.Remove(visualEffect);
                return visualEffect;
            }
            else
            {
                throw new System.Exception($"No Available effect with this ID {effectID}");
            }
        }

        public void DeactivateVisualEffect(IVisualEffect effect)
        {
            effect.DeactivateEffect();
            _effects.Add(effect);

            Debug.Log(_effects.Count);
        }
    }
}