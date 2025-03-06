using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class ItemGradePowerModifiers
    {
        [SerializeField] private GradeTypes _grade;
        [SerializeField] private float _mainModifier;
        [SerializeField] private float _otherModifier;

        public GradeTypes Grade => _grade;
        public float MainModifier => _mainModifier;
        public float OtherModifier => _otherModifier;
    }
}