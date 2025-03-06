using DG.Tweening;
using InGameConstants;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PassiveAbilities
{
    [Serializable]
    public class StrikeTargetView : PassiveAbilityVisualEffectBase
    {
        [SerializeField] private GameObject _leftWeapon;
        [SerializeField] private GameObject _rightWeapon;
        [SerializeField] Transform _leftWeaponStartPosition;
        [SerializeField] Transform _rightWeaponStartPosition;

        public override void ActivateEffect(Transform transform, float scale, float duration)
        {
            base.ActivateEffect(transform, scale, duration);

            _effectSequence = DOTween.Sequence();
            _effectSequence.SetId(gameObject.name);
            _effectSequence.Append(_leftWeapon.transform.DORotate(new Vector3(0, 0, 20), 0.2f, RotateMode.LocalAxisAdd));
            _effectSequence.Join(_rightWeapon.transform.DORotate(new Vector3(0, 0, 20), 0.2f, RotateMode.LocalAxisAdd));
            _effectSequence.Append(_leftWeapon.transform.DORotate(new Vector3(0, 0, -180), 0.3f, RotateMode.LocalAxisAdd));
            _effectSequence.Join(_rightWeapon.transform.DORotate(new Vector3(0, -0, -180), 0.3f, RotateMode.LocalAxisAdd));
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
            _leftWeapon.transform.rotation = _leftWeaponStartPosition.rotation;
            _rightWeapon.transform.rotation = _rightWeaponStartPosition.rotation;
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