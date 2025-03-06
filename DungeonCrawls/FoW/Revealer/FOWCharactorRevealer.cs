using Units;
using UnityEngine;

namespace FoW
{
    public class FOWCharactorRevealer : FOWRevealer
    {
        protected Unit _unit;

        public FOWCharactorRevealer(Unit unit)
        {
            _unit = unit;
            _unit.OnUnitDeath += SetNotValid;
            _isValid = true;
        }

        private void SetNotValid(Vector3 position, Unit unit)
        {
            _isValid = false;
        }

        public override void OnInit()
        {
            base.OnInit();
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }

        public override void Update(int deltaMS)
        {
            Vector3 position;
            float radius;

            if (_isValid)
            {
                GetRendererParameters(out position, out radius);
                _position = position;
                _radius = radius;
                _isValid = true;
            }
        }

        public void GetRendererParameters(out Vector3 position, out float radius)
        {
            var player = _unit;
            position = player.transform.position;
            radius = player.UnitParameters.UnitCharacteristics.ViewRadiusParameter.Value;
        }
    }
}