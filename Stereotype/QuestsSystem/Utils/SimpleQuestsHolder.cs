using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SimpleQuestsHolder), menuName = "Quests/" + nameof(SimpleQuestsHolder), order = 1)]
public class SimpleQuestsHolder : ScriptableObject
{
    [SerializeField] public int MaxActiveSimpleQuestsCount;
    [SerializeField] public List<QuestConfigurator> questConfigurators;
}
