using System;

namespace Dialogues
{
    public class PlayerAnswer: IDisposable
    {
        public event Action<int> OnAnswerClick;
        public event Action<PlayerAnswer> OnExit;

        private string _text;
        private int _toNode;
        private bool _isExit;
        private bool _isPositiveExitAnswer;
        private bool _isInteractable = true;

        public string AnswerText => _text;
        public bool IsExit => _isExit;
        public bool IsPositiveExitAnswer => _isPositiveExitAnswer;
        public bool IsInteractable => _isInteractable;

        public PlayerAnswer(PlayerAnswerJSONModel jsonModel)
        {
            _text = jsonModel.Text;
            _toNode = jsonModel.ToNode;
            _isExit = jsonModel.IsExit;
            _isPositiveExitAnswer = jsonModel.IsPositiveExitAnswer;
        }

        public PlayerAnswer(string text, int toNode, bool isExit, bool isPositiveExit)
        {
            _text = text;
            _toNode = toNode;
            _isExit = isExit;
            _isPositiveExitAnswer = isPositiveExit;
        }

        public void SetInteractable(bool isInteractable)
        {
            _isInteractable = isInteractable;
        }

        public void SelectAnswer()
        {
            OnAnswerClick?.Invoke(_toNode);

            if(_isExit)
            {
                OnExit?.Invoke(this);
            }
        }

        public void Dispose()
        {
            OnAnswerClick = null;
            OnExit = null;
        }
    }
}