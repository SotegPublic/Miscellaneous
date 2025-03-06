using UnityEngine;
using System.Collections.Generic;
using Core;
using Locations;
using NPCCharacters;
using Quests;
using UnityEditor;

namespace Dialogues
{
    public class DialogueGenerator : MonoBehaviour
    {
        public DialogueManagerConfigurator DialogueManagerConfigurator;
        public CharactersReputationConfig CharactersReputationConfigurator;
        public LocationsTypes LocationType;
        public NPCsTypes CharacterType;
        public LocalizationTypes LocalizationType;
        public string FileName = "Example.txt";
        public string DialogueStartString = "";
        public DialogueTypes DialogueType;
        [HideInInspector] public List<RewardModel> Rewards = new List<RewardModel>();
        [HideInInspector] public bool IsReputationRequired;
        [HideInInspector] public int ReputationLevelRequired = 0;
        [HideInInspector] public List<DialogueNodeJSONModel> DialogueNodes = new List<DialogueNodeJSONModel>();
        [HideInInspector] public QuestConfigurator QuestConfigurator;
        [HideInInspector] public int DialogueForQuestStage;
        [HideInInspector] public EventConfigurator EventConfigurator;

        private DialogueRepository _dialoguesRepository = new DialogueRepository();

        public void Generate()
        {
            var path = "";
            var fileName = "";
            var saveModel = new DialogueJSONModel();
            var npcFolder = DialogueManagerConfigurator.CharacterToFolderÑomparisons.Find(x => x.CharacterType == CharacterType).FolderName;
            var locationFolder = DialogueManagerConfigurator.LocationToFolderÑomparisons.Find(x => x.LocationsType == LocationType).FolderName;
            var localizationFolder = DialogueManagerConfigurator.LocalizationToFolderÑomparisons.Find(x => x.LocalizationType == LocalizationType).FolderName;



            switch (DialogueType)
            {
                case DialogueTypes.StartQuestDialogue:
                case DialogueTypes.QuestDialogue:
                    path = Application.dataPath + "/Resources/" + localizationFolder + "/" + "Quests" + "/" + npcFolder + "/";
                    var questIDString = QuestConfigurator.QuestID.ToString("N");
                    saveModel.QuestID = questIDString;
                    fileName = questIDString;
                    break;
                case DialogueTypes.StandartDialogue:
                case DialogueTypes.DialoigueWithReward:
                    path = Application.dataPath + "/Resources/" + localizationFolder + "/" + locationFolder + "/" + npcFolder + "/";
                    fileName = FileName;
                    break;
                case DialogueTypes.EventDialogue:
                    var eventIDString = EventConfigurator.EventID.ToString("N");
                    saveModel.EventID = eventIDString;
                    fileName = eventIDString;
                    break;
                case DialogueTypes.None:
                default:
                    break;
            }

            saveModel.DialogueStartString = DialogueStartString;
            saveModel.DialoguesNodes = DialogueNodes;
            saveModel.DialogueType = DialogueType;
            saveModel.ReputationLevelRequired = ReputationLevelRequired;

            _dialoguesRepository.Save(path, fileName, saveModel);
        }

        public void AddNode()
        {
            var newNode = new DialogueNodeJSONModel();
            newNode.NodeID = DialogueNodes.Count;
            DialogueNodes.Add(newNode);
        }

        public void RemoveLastNode()
        {
            if (DialogueNodes.Count == 0) return;
            DialogueNodes.RemoveAt(DialogueNodes.Count - 1);
        }

        public void ClearAllNodes()
        {
            DialogueNodes.Clear();
        }

        public void AddReward()
        {
            var newReward = new RewardModel();
            Rewards.Add(newReward);
        }

        public void RemoveLastReward()
        {
            if (Rewards.Count == 0) return;
            Rewards.RemoveAt(Rewards.Count - 1);
        }

        public void ClearAllRewards()
        {
            Rewards.Clear();
        }
    }
}