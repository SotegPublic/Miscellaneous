using System;
using UnityEngine;

namespace PassiveAbilities
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(StatusEffectConfigurator), menuName = "StatusEffects/StatusEffectConfigurator", order = 3)]
    public class StatusEffectConfigurator : ScriptableObject
    {
        [SerializeField] public StatusEffectTypesMappingTable StatusEffectTypesMappingTable;
        [SerializeField] public int StatusEffectID;
        [SerializeField] public StatusTypes StatusType;
        [SerializeField] public float StatusComboValue; // �������� ����� �������, �������� �������� ��������� ����� ��� �������� ����� 
        [SerializeField] public float StatusComboRadius; // ������ ��� ����� �������, ���� ���������
        [SerializeField] public float StatusEffectValue; // �������� ������ �������, �������� ���� ����
    }
}