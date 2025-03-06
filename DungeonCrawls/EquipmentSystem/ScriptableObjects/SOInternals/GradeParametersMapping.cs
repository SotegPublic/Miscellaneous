using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class GradeParametersMapping
    {
        [SerializeField] private GradeTypes _grade;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;
        [SerializeField] private float _minWeight;
        [SerializeField] private float _maxWeight;

        public GradeTypes Grade => _grade;
        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public float MinWeight => _minWeight;
        public float MaxWeight => _maxWeight;
    }
}