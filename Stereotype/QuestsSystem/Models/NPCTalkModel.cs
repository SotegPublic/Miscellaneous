using NPCCharacters;
using System;
using System.Collections.Generic;

namespace Quests
{
    [Serializable]
    public class NPCTalkModel
    {
        public NPCsTypes NPCType;
        public List<GlobalStatesTypes> StatesWhenCanTalk = new List<GlobalStatesTypes>();

        public void AddState()
        {
            StatesWhenCanTalk.Add(GlobalStatesTypes.None);
        }

        public void RemoveLastState()
        {
            if (StatesWhenCanTalk.Count == 0) return;
            StatesWhenCanTalk.RemoveAt(StatesWhenCanTalk.Count - 1);
        }

        public void ClearAllStates()
        {
            StatesWhenCanTalk.Clear();
        }
    }
}