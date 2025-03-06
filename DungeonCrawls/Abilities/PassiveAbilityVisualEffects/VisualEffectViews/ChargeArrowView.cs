using DG.Tweening;
using PassiveAbilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChargeArrowView : PassiveAbilityVisualEffectBase
{
    [SerializeField] private MeshRenderer _arrow;
    [SerializeField] private MeshRenderer _arrowSegment1;
    [SerializeField] private MeshRenderer _arrowSegment2; 
    [SerializeField] private MeshRenderer _arrowSegment3;
    [SerializeField] private MeshRenderer _arrowSegment4;
    [SerializeField] private Transform _startArrowTransform;
    [SerializeField] private Transform _endArrowTransform;

    private float _deltaDuration;
    private float _denominator = 5;
    private List<MeshRenderer> _arrowSegments;

    public override void ActivateEffect(Transform transform, float scale, float duration)
    {
        base.ActivateEffect(transform, scale, duration);

        _deltaDuration = _duration / _denominator;

        _arrowSegment1.enabled = false;
        _arrowSegment2.enabled = false;
        _arrowSegment3.enabled = false;
        _arrowSegment4.enabled = false;

        _arrowSegments = new List<MeshRenderer> { _arrowSegment1, _arrowSegment2, _arrowSegment3, _arrowSegment4 };

        MovaArrowSequence();

        void MovaArrowSequence()
        {
            _arrow.transform.position = _startArrowTransform.position;

            _effectSequence = DOTween.Sequence();
            _effectSequence.SetId(gameObject.name);
            _effectSequence.Append(_arrow.transform.DOMove(_endArrowTransform.position, _deltaDuration));
            _effectSequence.OnComplete(() =>
            {
                OnEffectTimeEnd?.Invoke(this);
                _denominator -= 1;
                CheckDuration();
            });
            //_effectSequence.OnKill(SetDefaultParameters);
        }

        void CheckDuration()
        {
            if(_denominator > 0)
            {
                var segment = _arrowSegments.Find(segment => segment.enabled == false);
                segment.enabled = true;

                MovaArrowSequence();
            }
            else
            {
                SetDefaultParameters();
            }
        }       
    }

    protected override void StopAnimation()
    {
        DOTween.Kill(_effectSequence, false);
    }

    protected override void SetDefaultParameters()
    {
        Debug.Log("SetDefault");

        _arrow.enabled = false;
        _arrowSegment1.enabled = false;
        _arrowSegment2.enabled = false;
        _arrowSegment3.enabled = false;
        _arrowSegment4.enabled = false;

        _denominator = 5;
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
