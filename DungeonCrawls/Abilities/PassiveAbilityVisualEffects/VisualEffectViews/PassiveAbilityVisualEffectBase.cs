using DG.Tweening;
using PassiveAbilities;
using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace PassiveAbilities
{
    [Serializable]
    public abstract class PassiveAbilityVisualEffectBase : MonoBehaviour, IVisualEffect
    {
        [SerializeField] protected PassiveAbilityTypes _effectType;
        [SerializeField] protected int _effectID;

        public PassiveAbilityTypes EffectType => _effectType;
        public int EffectID => _effectID;
        public int InstanceID { get { return GetInstanceID(); } }
        public Action<IVisualEffect> OnEffectTimeEnd { get; set; }

        protected bool _isFollow;
        protected Transform _targetTransform;
        protected Sequence _effectSequence;
        protected float _duration;

        public void SetPosition()
        {
            if (_isFollow)
            {
                transform.position = _targetTransform.position;
            }
        }

        public virtual void ActivateEffect(Transform transform, float scale, float duration)
        {
            _isFollow = true;
            _targetTransform = transform;
            gameObject.transform.position = transform.position;
            gameObject.transform.rotation = transform.rotation;
            _duration = duration;
            gameObject.SetActive(true);
        }

        public void DeactivateEffect()
        {
            _isFollow = false;
            _targetTransform = null;
            gameObject.SetActive(false);          
        }

        public void CancelEffect()
        {            
            StopAnimation();
            OnEffectTimeEnd?.Invoke(this);
        }

        public abstract void PauseAnimation(bool isPaused);        
        protected abstract void StopAnimation();
        protected abstract void SetDefaultParameters();       
    }
}