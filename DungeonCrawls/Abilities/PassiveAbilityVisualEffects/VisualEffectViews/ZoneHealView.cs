using DG.Tweening;
using InGameConstants;
using PassiveAbilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneHealView : PassiveAbilityVisualEffectBase
{
    [SerializeField] private ParticleSystem _particleSystem;

    public override void ActivateEffect(Transform transform, float scale, float duration)
    {
        base.ActivateEffect(transform, scale, duration);            

        _particleSystem.transform.localScale = new Vector3(scale, scale, _particleSystem.transform.localScale.z);
        var mainModule = _particleSystem.main;
        mainModule.duration = duration;
        mainModule.startLifetime = duration;
        mainModule.stopAction = ParticleSystemStopAction.Callback;
        _particleSystem.Play();
        // onEnd
    }

    public override void PauseAnimation(bool isPaused)
    {
        if (isPaused)
        {
            _particleSystem.Stop();
        }
        else
        {
            _particleSystem.Play();
        }
    }

    protected override void SetDefaultParameters()
    {
        _particleSystem.gameObject.transform.localScale = Vector3.one;
    }

    protected override void StopAnimation()
    {
        SetDefaultParameters();
    }
}
