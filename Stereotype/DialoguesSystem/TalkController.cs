
using DialogueSystem;
using NPCCharacters;
using System;
using System.Collections.Generic;

namespace Dialogues
{
    public class TalkController
    {
        public Action<NPCsTypes> OnTalkEnd;
        
        private DialoguePanelController _dialoguePanelController;
        private GreetingNode _greetingNode;

        private NPCModel _currentNPC;
        private List<Dialogue> _currentNPCDialogues;
        private Dialogue _currentDialogue;
        private IDialogueNode _currentDialogueNode;

        public TalkController(DialoguePanelController dialoguePanelController)
        {
            _dialoguePanelController = dialoguePanelController;
        }

        public void StartGreeting(NPCModel npc, List<Dialogue> npcDialogues, string npcGreeting)
        {
            _currentNPC = npc;
            _currentNPCDialogues = npcDialogues;

            _greetingNode = new GreetingNode(npcDialogues, npcGreeting);

            _dialoguePanelController.InitCompanion(_currentNPC);
            _dialoguePanelController.VisualizeDialogueNode(_greetingNode);
            _greetingNode.OnAnswerClick += GetDialogueIndex;
        }

        private void GetDialogueIndex(int dialogueIndex)
        {
            if (dialogueIndex == -1)
            {
                EndCurrentTalk(_greetingNode.ExitAnswer);
            }
            else
            {
                _currentDialogue = _currentNPCDialogues.Find(x => x.DialogueInListID == dialogueIndex);
                StartSelectedDialogue();
            }

            _greetingNode.OnAnswerClick -= GetDialogueIndex;
            _greetingNode.Dispose();
            _greetingNode = null;
        }

        private void StartSelectedDialogue()
        {
            _currentDialogue.OnDialogueEnd += EndCurrentTalk;

            _currentDialogueNode = _currentDialogue.GetDialogueNode(0);
            _currentDialogueNode.OnAnswerClick += GoToNextDialogueNode;

            _dialoguePanelController.VisualizeDialogueNode(_currentDialogueNode);

        }

        private void GoToNextDialogueNode(int nodeIndex)
        {
            _currentDialogueNode.OnAnswerClick -= GoToNextDialogueNode;

            if (nodeIndex != -1)
            {
                _currentDialogueNode = _currentDialogue.GetDialogueNode(nodeIndex);
                _currentDialogueNode.OnAnswerClick += GoToNextDialogueNode;

                //todo - если диалог квестовые, проверять завершающие ответы в ноде на возможность сдачи квеста (например наличие ресурса для сдачи)
                // и выставлять параметр IsInteractable

                _dialoguePanelController.VisualizeDialogueNode(_currentDialogueNode);
            }
        }

        private void EndCurrentTalk(PlayerAnswer endAnswer)
        {
            OnTalkEnd?.Invoke(_currentNPC.NPCType);
            _dialoguePanelController.ClearDialogueView();

            if (_currentDialogue != null)
            {
                _currentDialogue.OnDialogueEnd -= EndCurrentTalk;
            }

            for(int i = 0; i < _currentNPCDialogues.Count; i++)
            {
                _currentNPCDialogues[i].Dispose();
            }
            _currentNPCDialogues.Clear();

            _currentNPC = null;
            _currentDialogue = null;
            _currentDialogueNode = null;
            _currentNPCDialogues = null;
        }
    }
}