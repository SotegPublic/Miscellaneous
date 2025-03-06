using Engine;
using Units;
using UnityEngine;

namespace Abilities
{
    public interface IDisplayedEffectModelForUI
    {
        public string DisplayedEffectName { get; }
        public string DisplayedEffectDescription { get; }
        public Sprite DisplayedEffectIcon { get; }
        public Unit Owner { get; }
        public SubscribableProperty<int> Stacks { get; }
    }
}