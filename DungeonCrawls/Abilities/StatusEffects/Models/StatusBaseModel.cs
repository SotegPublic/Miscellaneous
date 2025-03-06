using Equipment;
using PassiveAbilities;
using System.Collections.Generic;
using UnityEngine;

public class StatusBaseModel
{
    private StatusComboTypes _statusComboType;
    private string _name;
    private string _description;
    private float _duration;
    private float _frequency;
    private float _prolongationTime;
    private Sprite _statusEffectIcon;
    private List<WeaponTypes> _buffedWeapons;

    public StatusComboTypes StatusComboType => _statusComboType;
    public string Name => _name;
    public string Description => _description;
    public float Duration => _duration;
    public float Frequency => _frequency;
    public float ProlongationTime => _prolongationTime;
    public Sprite StatusEffectIcon => _statusEffectIcon;
    public List<WeaponTypes> BuffedWeapons => _buffedWeapons;

    public StatusBaseModel(StatusComboTypes statusComboType, StatusCongigurator statusCongigurator)
    {
        _statusComboType = statusComboType;
        _name = statusCongigurator.Name;
        _description = statusCongigurator.Description;
        _duration = statusCongigurator.Duration;
        _frequency = statusCongigurator.Frequency;
        _prolongationTime = statusCongigurator.ProlongationTime;
        _statusEffectIcon = statusCongigurator.StatusIcon;
        _buffedWeapons = statusCongigurator.BuffedWeapons;
    }
}