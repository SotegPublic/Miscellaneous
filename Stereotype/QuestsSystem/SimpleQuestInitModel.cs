using NPCCharacters;

namespace Quests
{
    public class SimpleQuestInitModel
    {
        private NPCsTypes _characterType;
        private QuestConfigurator _questConfigurator;

        public SimpleQuestInitModel(QuestConfigurator questConfigurator)
        {
            _characterType = questConfigurator.QuestGiver;
            _questConfigurator = questConfigurator;
        }
    }
}