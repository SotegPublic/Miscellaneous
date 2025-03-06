using System;

namespace Dialogues
{
    [Serializable]
    public class PlayerAnswerJSONModel
    {
        public string Text;
        public int ToNode;
        public bool IsExit;
        public bool IsPositiveExitAnswer;
    }
}