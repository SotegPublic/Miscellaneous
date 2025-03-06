using Configs.Enum;
using NPCCharacters;
using System;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class RewardModel
    {
        public DialogueRewardTypes DialogueRewardType;
        public NPCsTypes ReputationRewardTarget;
        public ResourcesTypes ResourceType;
        public int RewardCount;
    }
}