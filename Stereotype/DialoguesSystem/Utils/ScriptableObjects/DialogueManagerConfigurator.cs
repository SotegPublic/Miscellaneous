using UnityEngine;
using System.Collections.Generic;

namespace Dialogues
{
    [CreateAssetMenu(fileName = nameof(DialogueManagerConfigurator), menuName = "Dialogues/" + nameof(DialogueManagerConfigurator), order = 0)]
    public class DialogueManagerConfigurator : ScriptableObject
    {
        [SerializeField] private LocalizationTypes _localizationType;
        [SerializeField] private List<LocationToFolderСomparison> _locationToFolderСomparisons;
        [SerializeField] private List<CharacterToFolderСomparison> _characterToFolderСomparisons;
        [SerializeField] private List<LocalizationToFolderСomparison> _localizationToFolderСomparisons;

        public LocalizationTypes LocalizationType => _localizationType;
        public List<LocationToFolderСomparison> LocationToFolderСomparisons => _locationToFolderСomparisons;
        public List<CharacterToFolderСomparison> CharacterToFolderСomparisons => _characterToFolderСomparisons;
        public List<LocalizationToFolderСomparison> LocalizationToFolderСomparisons => _localizationToFolderСomparisons;
    }
}