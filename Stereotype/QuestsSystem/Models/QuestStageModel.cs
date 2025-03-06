using Dialogues;
using System;
using System.Collections.Generic;

namespace Quests
{


    [Serializable]
    public class QuestStageModel
    {
        public QuestStagesTypes QuestStageType;
        public string StageName;
        public string StageDescription;
        public List<RequiredResources> RequiredResources = new List<RequiredResources>();
        public List<NPCTalkModel> RequiredNPCs = new List<NPCTalkModel>();
        public List<RewardModel> StageRewards = new List<RewardModel>();

        public void AddRequiredResource()
        {
            RequiredResources.Add(new RequiredResources());
        }

        public void RemoveLastRequiredResource()
        {
            if (RequiredResources.Count == 0) return;
            RequiredResources.RemoveAt(RequiredResources.Count - 1);
        }

        public void ClearAllRequiredResource()
        {
            RequiredResources.Clear();
        }

        public void AddReward()
        {
            StageRewards.Add(new RewardModel());
        }

        public void RemoveLastReward()
        {
            if (StageRewards.Count == 0) return;
            StageRewards.RemoveAt(StageRewards.Count - 1);
        }

        public void ClearAllRewards()
        {
            StageRewards.Clear();
        }

        public void ClearStageRequiredParameters()
        {
            RequiredNPCs.Clear();
            RequiredResources.Clear();
        }

        public void AddRequiredNPC()
        {
            RequiredNPCs.Add(new NPCTalkModel());
        }

        public void RemoveLastRequiredNPC()
        {
            if (RequiredNPCs.Count == 0) return;
            RequiredNPCs.RemoveAt(RequiredNPCs.Count - 1);
        }

        public void ClearAllRequiredNPC()
        {
            RequiredNPCs.Clear();
        }
    }
}