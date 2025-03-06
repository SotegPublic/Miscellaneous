using System;
using System.Collections.Generic;

namespace Dialogues
{
    public class DialogueNode: IDialogueNode, IDisposable
    {
        public Action<int> OnAnswerClick { get; set; }

        private string _npcText;
        private List<PlayerAnswer> _playerAnswers = new List<PlayerAnswer>();

        public string NpcText => _npcText;
        public List<PlayerAnswer> PlayerAnswers => _playerAnswers;

        public DialogueNode(DialogueNodeJSONModel jsonModel)
        {
            for (int i = 0; i < jsonModel.PlayerAnswers.Count; i++)
            {
                var answer = new PlayerAnswer(jsonModel.PlayerAnswers[i]);
                _playerAnswers.Add(answer);
                answer.OnAnswerClick += AnswerWasClicked;
            }

            _npcText = jsonModel.NPCText;
        }

        private void AnswerWasClicked(int nodeID)
        {
            OnAnswerClick?.Invoke(nodeID);
        }

        public void Dispose()
        {
            for (int i = 0; i < _playerAnswers.Count; i++)
            {
                _playerAnswers[i].OnAnswerClick -= AnswerWasClicked;
                _playerAnswers[i].Dispose();
            }

            _playerAnswers.Clear();
        }
    }
}