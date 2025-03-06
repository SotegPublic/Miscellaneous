using System;
using System.Collections.Generic;

namespace Dialogues
{
    public interface IDialogueNode
    {
        public Action<int> OnAnswerClick { get; set; }
        public string NpcText { get; }
        List<PlayerAnswer> PlayerAnswers { get; }
    }
}