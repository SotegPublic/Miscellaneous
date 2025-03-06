using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = nameof(GreetingsConfigurator), menuName = "Dialogues/" + nameof(GreetingsConfigurator), order = 0)]
    public class GreetingsConfigurator: ScriptableObject
    {
        [SerializeField] public List<GreetingsByCharacter> Greetings;
    }
}