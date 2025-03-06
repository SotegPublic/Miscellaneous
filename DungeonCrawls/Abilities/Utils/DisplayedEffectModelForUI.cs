using Abilities;
using Engine;
using Units;
using UnityEngine;

namespace PassiveAbilities
{
    public class DisplayedEffectModelForUI: IDisplayedEffectModelForUI
    {
        public string DisplayedEffectName { get; private set; }
        public string DisplayedEffectDescription { get; private set; }
        public Sprite DisplayedEffectIcon { get; private set; }
        public Unit Owner { get; private set; }
        public SubscribableProperty<int> Stacks { get; private set; }

        public DisplayedEffectModelForUI(string name, string discription, Sprite icon, Unit owner)
        {
            DisplayedEffectName = name;
            DisplayedEffectDescription = discription;
            DisplayedEffectIcon = icon;
            Owner = owner;
            Stacks = new SubscribableProperty<int>();
        }
    }
}