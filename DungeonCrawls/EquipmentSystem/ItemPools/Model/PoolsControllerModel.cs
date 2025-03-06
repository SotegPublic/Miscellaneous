using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class PoolsControllerModel
    {
        [SerializeField] private int _countItemCopies;
        public int CountItemCopies => _countItemCopies;
    }
}