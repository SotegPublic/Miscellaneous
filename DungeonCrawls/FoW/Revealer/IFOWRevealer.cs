using UnityEngine;

namespace FoW
{
    public interface IFOWRevealer
    {
        bool IsValid();
        Vector3 GetPosition();
        float GetRadius();
        void Update(int deltaMS);
        void Release();
    }
}