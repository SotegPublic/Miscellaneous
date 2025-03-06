using DG.Tweening;
using InGameConstants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PassiveAbilities
{
    [Serializable]
    public class StrikeAOEView : PassiveAbilityVisualEffectBase
    {
        [SerializeField] private GameObject _objectToRotate;

        public override void ActivateEffect(Transform transform, float scale, float duration)
        {
            base.ActivateEffect(transform, scale, duration);

            _effectSequence = DOTween.Sequence();
            _effectSequence.SetId(gameObject.name);
            _effectSequence.Append(_objectToRotate.transform.DORotate(new Vector3 (0, 360, 0), 0.5f, RotateMode.LocalAxisAdd));
            _effectSequence.OnComplete(() => OnEffectTimeEnd?.Invoke(this));
            _effectSequence.OnComplete(() => 
            {
                OnEffectTimeEnd?.Invoke(this);
                SetDefaultParameters();
            });
            _effectSequence.OnKill(SetDefaultParameters);
        }

        protected override void StopAnimation()
        {
            DOTween.Kill(_effectSequence, false);
        }

        protected override void SetDefaultParameters()
        {
            _objectToRotate.transform.localRotation = Quaternion.identity;
        }

        public override void PauseAnimation(bool isPaused)
        {
            if (isPaused)
            {
                DOTween.Pause(_effectSequence);
            }
            else
            {
                DOTween.Play(_effectSequence);
            }
        }
    }
}