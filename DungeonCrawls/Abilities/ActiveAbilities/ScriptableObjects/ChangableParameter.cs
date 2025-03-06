using System;
using Units.UnitsParameters;
using UnityEngine;

namespace Abilities
{
    [Serializable]
    public class ChangableParameter
    {
        [SerializeField] public float Impact;
        [SerializeField] public ParameterTypes ImpactParameter;
    }
}