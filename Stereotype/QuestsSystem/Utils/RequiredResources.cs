using Configs.Enum;
using System;

namespace Quests
{
    [Serializable]
    public class RequiredResources
    {
        public ResourcesTypes ResourceType;
        public int ResourceCount;
    }
}