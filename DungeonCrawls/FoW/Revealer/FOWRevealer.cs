using UnityEngine;

namespace FoW
{
    public class FOWRevealer : IFOWRevealer
    {
        protected bool _isValid;
        protected Vector3 _position;
        protected float _radius;

        public virtual void OnInit()
        {
            _position = Vector3.zero;
            _radius = 0f;
            _isValid = false;
        }

        public virtual void OnRelease()
        {
            _isValid = false;
        }

        public virtual Vector3 GetPosition()
        {
            return _position;
        }

        public virtual float GetRadius()
        {
            return _radius;
        }

        public bool IsValid()
        {
            return _isValid;
        }

        public virtual void Update(int deltaMS)
        {
        }

        public void Release()
        {
            this.OnRelease();
        }
    }
}