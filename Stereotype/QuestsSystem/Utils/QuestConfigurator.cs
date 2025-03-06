using Core;
using Dialogues;
using NPCCharacters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = nameof(QuestConfigurator), menuName = "Quests/" + nameof(QuestConfigurator), order = 0)]
    public class QuestConfigurator : ScriptableObject
    {
        [HideInInspector] public CharactersReputationConfig CharactersReputationConfigurator;
        [HideInInspector] public Guid QuestID = Guid.NewGuid();
        [HideInInspector] public string QuestName;
        [HideInInspector] public string QuestDescription;
        [HideInInspector] public NPCsTypes QuestGiver;
        [HideInInspector] public List<GlobalStatesTypes> GlobalStatesWhenCanStartQuest = new List<GlobalStatesTypes>();
        [HideInInspector] public bool IsReputationRequired;
        [HideInInspector] public int ReputationLevelRequired = 0;
        [HideInInspector] public List<QuestStageModel> QuestStages = new List<QuestStageModel>();

        public void AddStage()
        {
            QuestStages.Add(new QuestStageModel());
        }

        public void RemoveLastStage()
        {
            if (QuestStages.Count == 0) return;
            QuestStages.RemoveAt(QuestStages.Count - 1);
        }

        public void ClearAllStages()
        {
            QuestStages.Clear();
        }
    }
}