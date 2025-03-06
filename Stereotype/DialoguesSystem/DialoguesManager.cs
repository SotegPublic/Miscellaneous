
using DialogueSystem;
using Locations;
using NPCCharacters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    public class DialoguesManager
    {
        private DialogueRepository _dialogueRepository;

        private DialogueManagerConfigurator _dialogueManagerConfigurator;
        private GreetingsConfigurator _greetingsConfigurator;

        private NPCPlacementManager _npcPlacementManager;
        private NPCsController _nPCsController;
        private TalkController _talkController;
        private LocationsTypes _currentLocationType;
        private List<Dialogue> _currentDialogues = new List<Dialogue>();

        private Dictionary<NPCsTypes, List<Dialogue>> _charactersDialogues = new Dictionary<NPCsTypes, List<Dialogue>>();
        private Dictionary<NPCsTypes, string> _charactersGreetings = new Dictionary<NPCsTypes, string>();

        public TalkController TalkController => _talkController;

        public DialoguesManager(DialogueManagerConfigurator dialogueManagerConfigurator, GreetingsConfigurator greetingsConfigurator,
            NPCPlacementManager npcPlacementManager, DialoguePanelController dialoguePanelController, NPCsController nPCsController)
        {
            //Контроллер квестов

            _dialogueRepository = new DialogueRepository();
            _dialogueManagerConfigurator = dialogueManagerConfigurator;
            _greetingsConfigurator = greetingsConfigurator;
            _npcPlacementManager = npcPlacementManager;
            _nPCsController = nPCsController;
            _talkController = new TalkController(dialoguePanelController);
            _nPCsController.OnStartDialogue += StartGreeting;
            _npcPlacementManager.OnChangeNpcLocation += UpdateDialogues;
        }

        private void UpdateDialogues(LocationsTypes locationType, NPCsTypes npcType)
        {
            if(locationType == _currentLocationType)
            {
                if(!_charactersDialogues.ContainsKey(npcType))
                {
                    var pathStringToLocation = GetPathString();
                    var npcFolder = _dialogueManagerConfigurator.CharacterToFolderСomparisons.Find(x => x.CharacterType == npcType).FolderName;

                    var fullPath = pathStringToLocation + npcFolder + "/";
                    var npcDialogues = _dialogueRepository.Load(fullPath);

                    if (npcDialogues.Count > 0)
                    {
                        _charactersDialogues.Add(npcType, npcDialogues);
                    }

                    var greeting = _greetingsConfigurator.Greetings.Find(x => x.Character == npcType).Greeting;

                    _charactersGreetings.Add(npcType, greeting);
                }
            }
            else
            {
                if (_charactersDialogues.ContainsKey(npcType))
                {
                    for (int i = 0; i < _charactersDialogues[npcType].Count; i++)
                    {
                        _charactersDialogues[npcType][i].Dispose();
                    }

                    _charactersDialogues[npcType].Clear();
                    _charactersDialogues.Remove(npcType);
                    _charactersGreetings.Remove(npcType);
                }
            }
        }

        public void SetCurrentLocation(LocationsTypes locationType)
        {
            _currentLocationType = locationType;
            _charactersDialogues.Clear(); //todo сделать диспоус диалогов перед очисткой, чтоб не какать в память
            _charactersGreetings.Clear();
        }

        private string GetPathString()
        {
            var locationFolder = _dialogueManagerConfigurator.LocationToFolderСomparisons.Find(x => x.LocationsType == _currentLocationType).FolderName;
            var localizationFolder = _dialogueManagerConfigurator.LocalizationToFolderСomparisons.
                Find(x => x.LocalizationType == _dialogueManagerConfigurator.LocalizationType).FolderName;

            return Application.dataPath + "/Resources/" + localizationFolder + "/" + locationFolder + "/";
        }

        private void StartGreeting(NPCView npcView)
        {
            var currentNPCType = npcView.CharacterType;
            var currentNPCModel = _nPCsController.GetNPCModel(npcView);

            _currentDialogues.Clear();

            if(_charactersDialogues.TryGetValue(currentNPCType, out var dialogues)) // todo - вот тут должна быть система отбора диалогов, доступных игроку
            {
                _currentDialogues = new List<Dialogue>(dialogues); 
            }

            var npcGreeting = _charactersGreetings[currentNPCType];

            _talkController.StartGreeting(currentNPCModel, _currentDialogues, npcGreeting);
        }

        public void Dispose()
        {
            _nPCsController.OnStartDialogue -= StartGreeting;
        }
    }
}