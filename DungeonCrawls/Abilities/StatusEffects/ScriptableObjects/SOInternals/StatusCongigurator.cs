using Equipment;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PassiveAbilities
{
    [Serializable]
    public class StatusCongigurator
    {
        [SerializeField] public StatusTypes StatusType;
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [SerializeField] public float Duration;
        [SerializeField] public float Frequency;
        [SerializeField] public float ProlongationTime;
        [SerializeField] public Sprite StatusIcon;
        [SerializeField] public List<WeaponTypes> BuffedWeapons;


        public void AddBuffedWeapon()
        {
            BuffedWeapons.Add(WeaponTypes.None);
        }

        public void RemoveLastBuffedWeapon()
        {
            if (BuffedWeapons.Count == 0) return;

            int lastIndex = BuffedWeapons.Count - 1;
            BuffedWeapons.Remove(BuffedWeapons[lastIndex]);
        }

        public void ClearBuffedWeapons()
        {
            BuffedWeapons.Clear();
        }
    }
}