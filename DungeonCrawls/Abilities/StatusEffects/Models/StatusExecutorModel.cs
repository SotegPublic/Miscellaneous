using Abilities;
using UnityEngine;

namespace PassiveAbilities
{
    public class StatusExecutorModel
    {
        private StatusTypes _statusType;
        private string _statusEffectName;
        private string _statusEffectDescription;
        private Sprite _statusEffectIcon;
        private float _statusEffectDuration;
        private float _statusEffectFrequency;
        private float _statusEffectProlongationTime;
        private float _statusComboValue;
        private float _statusComboRadius;
        private float _statusEffectValue;

        public StatusTypes StatusType => _statusType;
        public string StatusEffectName => _statusEffectName;
        public string StatusEffectDescription => _statusEffectDescription;
        public Sprite StatusEffectIcon => _statusEffectIcon;
        public float StatusEffectDuration => _statusEffectDuration;
        public float StatusEffectFrequency => _statusEffectFrequency;
        public float StatusEffectProlongationTime => _statusEffectProlongationTime;
        public float StatusComboValue => _statusComboValue;
        public float StatusComboRadius => _statusComboRadius;
        public float StatusEffectValue => _statusEffectValue;

        public StatusExecutorModel(StatusEffectProtoModel model, StatusBaseModel statusBaseModel)
        {
            _statusType = model.StatusType;
            _statusEffectName = statusBaseModel.Name;
            _statusEffectDescription = statusBaseModel.Description;
            _statusEffectIcon = statusBaseModel.StatusEffectIcon;
            _statusEffectDuration = statusBaseModel.Duration;
            _statusEffectFrequency = statusBaseModel.Frequency;
            _statusEffectProlongationTime = statusBaseModel.ProlongationTime;
            _statusComboValue = model.StatusComboValue;
            _statusComboRadius = model.StatusComboRadius;
            _statusEffectValue = model.StatusEffectValue;
        }

        public void UpgrageModel(float newComboValue, float newComboRadius, float newEffectValue)
        {
            _statusEffectValue = newEffectValue;
            _statusComboRadius = newComboRadius;
            _statusComboValue = newComboValue;
        }
    }
}