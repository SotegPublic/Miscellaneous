using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Abilities
{
    public interface IAbilityDataForExecutor
    {
        public int AbilityID { get; }
        public List<Unit> TargetList { get; }
        public Vector3 CastPoint { get; }
        public Action<int, Unit, bool> ExecuteCallback { get; }
    }
}