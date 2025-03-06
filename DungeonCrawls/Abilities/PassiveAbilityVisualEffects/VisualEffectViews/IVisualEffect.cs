using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace PassiveAbilities
{
    public interface IVisualEffect
    {
        public Action<IVisualEffect> OnEffectTimeEnd { get; set; }
        public int EffectID { get; } 
        public int InstanceID { get; }
        public PassiveAbilityTypes EffectType { get; }
        public void ActivateEffect(Transform transform, float scale, float duration);
        public void DeactivateEffect();
        public void SetPosition();
        public void CancelEffect();
        public void PauseAnimation(bool isPaused);
    }
}