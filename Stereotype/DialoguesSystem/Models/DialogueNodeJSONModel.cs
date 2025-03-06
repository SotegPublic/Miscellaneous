using System.Collections.Generic;
using System;

namespace Dialogues
{
    [Serializable]
    public class DialogueNodeJSONModel
    {
        public int NodeID;
        public string NPCText;
        public List<PlayerAnswerJSONModel> PlayerAnswers = new List<PlayerAnswerJSONModel>();

        public void AddAnswer()
        {
            var newPlayerAnswer = new PlayerAnswerJSONModel();
            PlayerAnswers.Add(newPlayerAnswer);
        }

        public void RemoveLastAnswer()
        {
            if (PlayerAnswers.Count == 0) return;
            PlayerAnswers.RemoveAt(PlayerAnswers.Count - 1);
        }

        public void ClearAllAnswers()
        {
            PlayerAnswers.Clear();
        }
    }
}