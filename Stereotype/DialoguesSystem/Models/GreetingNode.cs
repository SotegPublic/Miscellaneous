using System;
using System.Collections.Generic;

namespace Dialogues
{
    public class GreetingNode: IDialogueNode, IDisposable
    {
        public Action<int> OnAnswerClick { get; set; }

        private string _npcText;
        private List<PlayerAnswer> _playerAnswers = new List<PlayerAnswer>();
        private PlayerAnswer _exitAnswer;

        public string NpcText => _npcText;
        public List<PlayerAnswer> PlayerAnswers => _playerAnswers;
        public PlayerAnswer ExitAnswer => _exitAnswer;

        public GreetingNode(List<Dialogue> dialogues, string greetingText)
        {
            _npcText = greetingText;

            for(int i = 0; i < dialogues.Count; i++)
            {
                var answer = new PlayerAnswer(dialogues[i].DialogueStartString, dialogues[i].DialogueInListID, false, false);
                RegistrateAnswer(answer);
            }

            _exitAnswer = new PlayerAnswer("Бывай", -1, true, false);
            RegistrateAnswer(_exitAnswer);
        }

        private void RegistrateAnswer(PlayerAnswer answer)
        {
            answer.OnAnswerClick += AnswerWasClicked;
            _playerAnswers.Add(answer);
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
            OnAnswerClick = null;
        }
    }
}