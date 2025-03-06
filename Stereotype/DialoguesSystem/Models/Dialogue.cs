using Configs.Enum;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Dialogues
{
    public class Dialogue: IDisposable
    {
        public event Action<PlayerAnswer> OnDialogueEnd;

        private int _dialogueInListID;
        private string _dialogueStartString;
        private DialogueTypes _dialogueType;
        private List<RewardModel> _rewards = new List<RewardModel>();
        private Guid _questID;
        private int _questStage;
        private Guid _eventID;
        private int _reputationLevelRequired;
        private List<PlayerAnswer> _endAnswers = new List<PlayerAnswer>();
        private Dictionary<int, DialogueNode> _dialogueNodes = new Dictionary<int, DialogueNode>();

        public int DialogueInListID => _dialogueInListID;
        public string DialogueStartString => _dialogueStartString;
        public DialogueTypes DialogueType => _dialogueType;
        public List<RewardModel> Rewards => _rewards;
        public Guid QuestID => _questID;
        public int QuestStage => _questStage;
        public Guid EventID => _eventID;
        public int ReputationLevelRequired => _reputationLevelRequired;

        public Dialogue(DialogueJSONModel saveModel, int inListID)
        {
            _dialogueInListID = inListID;
            for (int i = 0; i < saveModel.DialoguesNodes.Count; i++)
            {
                var dialogueNode = new DialogueNode(saveModel.DialoguesNodes[i]);
                _dialogueNodes.Add(saveModel.DialoguesNodes[i].NodeID, dialogueNode);

                for(int j = 0; j < dialogueNode.PlayerAnswers.Count; j++)
                {
                    if (dialogueNode.PlayerAnswers[j].IsExit)
                    {
                        _endAnswers.Add(dialogueNode.PlayerAnswers[j]);
                    }
                }
            }

            _dialogueStartString = saveModel.DialogueStartString;
            _dialogueType = saveModel.DialogueType;
            _rewards = saveModel.Rewards;
            _questID = Guid.TryParseExact(saveModel.QuestID, "N", out var questID) ? questID : default;
            _questStage = saveModel.QuestStage;
            _eventID = Guid.TryParseExact(saveModel.EventID, "N", out var eventID) ? eventID : default;
            _reputationLevelRequired = saveModel.ReputationLevelRequired;

            SubscribeOnEnd(_endAnswers);
        }

        public DialogueNode GetDialogueNode(int nodeID)
        {
            return _dialogueNodes[nodeID];
        }

        public void Dispose()
        {
            for (int i = 0; i < _endAnswers.Count; i++)
            {
                _endAnswers[i].OnExit += EndDialogue;
            }
            _endAnswers.Clear();

            foreach (var node in _dialogueNodes)
            {
                node.Value.Dispose();
            }
            _dialogueNodes.Clear();
        }

        private void SubscribeOnEnd(List<PlayerAnswer> playerAnswers)
        {
            for (int i = 0; i < playerAnswers.Count; i++)
            {
                playerAnswers[i].OnExit += EndDialogue;
            }
        }

        private void EndDialogue(PlayerAnswer endAnswer)
        {
            OnDialogueEnd?.Invoke(endAnswer);
        }
    }
}