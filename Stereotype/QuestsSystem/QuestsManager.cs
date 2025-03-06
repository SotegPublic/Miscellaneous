using Core;
using DG.Tweening;
using NPCCharacters;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestsManager
    {
        private NPCsController _nPCsController;
        private int _maxActiveSimpleQuestsCount;
        private SubscribableProperty<int> _currentActiveSimpleQuestCount;
        private Dictionary<NPCsTypes, List<QuestConfigurator>> _simpleQuestsConfigsByCharacter = new Dictionary<NPCsTypes, List<QuestConfigurator>>();
        private Dictionary<NPCsTypes, SimpleQuest> _activeSimpleQuestsByCharacter = new Dictionary<NPCsTypes, SimpleQuest>();
        private Dictionary<NPCsTypes, SimpleQuestInitModel> _preparedAvailableSimpleQuestsForCharacter = new Dictionary<NPCsTypes, SimpleQuestInitModel>();

        public QuestsManager(SimpleQuestsHolder simpleQuestsHolder, NPCsController nPCsController)
        {
            _nPCsController = nPCsController;
            _maxActiveSimpleQuestsCount = simpleQuestsHolder.MaxActiveSimpleQuestsCount;
            _currentActiveSimpleQuestCount = new SubscribableProperty<int>();

            InitManager(simpleQuestsHolder);
        }

        private void InitManager(SimpleQuestsHolder simpleQuestsHolder)
        {
            for (int i = 0; i < simpleQuestsHolder.questConfigurators.Count; i++)
            {
                var questGiver = simpleQuestsHolder.questConfigurators[i].QuestGiver;
                if (_simpleQuestsConfigsByCharacter.ContainsKey(questGiver))
                {
                    _simpleQuestsConfigsByCharacter[questGiver].Add(simpleQuestsHolder.questConfigurators[i]);
                }
                else
                {
                    _simpleQuestsConfigsByCharacter.Add(questGiver,
                        new List<QuestConfigurator>()
                        {
                        simpleQuestsHolder.questConfigurators[i]
                        });
                }
            }

            _currentActiveSimpleQuestCount.SubscribeOnValue(CheckActiveSimpleQuestCount);
            PrepareAvailableSimpleQuests();
        }

        private void CheckActiveSimpleQuestCount(int simpleQuestsCount)
        {
            if(simpleQuestsCount < _maxActiveSimpleQuestsCount)
            {
                PrepareAvailableSimpleQuests();
            }
        }

        private void PrepareAvailableSimpleQuests()
        {
            ClearNPCModels();

            foreach (var item in _simpleQuestsConfigsByCharacter)
            {
                if (!_activeSimpleQuestsByCharacter.ContainsKey(item.Key))
                {
                    if(item.Value.Count > 1)
                    {
                        var questNumber = Random.Range(0, item.Value.Count);
                        _preparedAvailableSimpleQuestsForCharacter.Add(item.Key, new SimpleQuestInitModel(item.Value[questNumber]));
                    }
                    else
                    {
                        _preparedAvailableSimpleQuestsForCharacter.Add(item.Key, new SimpleQuestInitModel(item.Value[0]));
                    }
                }
            }

            UpdateNPCModels();
        }

        private void ClearNPCModels()
        {
            foreach (var item in _preparedAvailableSimpleQuestsForCharacter)
            {
                var model = _nPCsController.GetNPCModel(item.Key);
                model.IsCanGiveSimpleQuest = false;
            }

            _preparedAvailableSimpleQuestsForCharacter.Clear();
        }

        private void UpdateNPCModels()
        {
            foreach(var item in _preparedAvailableSimpleQuestsForCharacter)
            {
                var model = _nPCsController.GetNPCModel(item.Key);
                model.IsCanGiveSimpleQuest = true;
            }
        }
    }
}