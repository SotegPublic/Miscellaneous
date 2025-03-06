using Configs.Enum;
using NPCCharacters;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogues
{

    [Serializable]
    public sealed class DialogueJSONModel
    {
        public DialogueTypes DialogueType;
        public List<RewardModel> Rewards;
        public string QuestID;
        public int QuestStage;
        public string EventID;
        public int ReputationLevelRequired;
        public string DialogueStartString;
        public List<DialogueNodeJSONModel> DialoguesNodes;
    }
}