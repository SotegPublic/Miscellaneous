using Engine;
using System.Collections.Generic;

namespace PassiveAbilities
{
    public class StatusBaseParametersKeeper: IController, ICleanable
    {
        private Dictionary<StatusTypes, StatusBaseModel> _statusesBaseParameters= new Dictionary<StatusTypes, StatusBaseModel>();
        
        public StatusBaseParametersKeeper(StatusEffectTypesMappingTable statusEffectTypesMappingTable, StatusConfiguratorsList statusConfiguratorsList)
        {
            foreach (var mapping in statusEffectTypesMappingTable.StatusEffectMappings)
            {
                CreateStatusBaseModel(statusConfiguratorsList, mapping);
            }
        }

        private void CreateStatusBaseModel(StatusConfiguratorsList statusConfiguratorsList, StatusEffectTypesMapping mapping)
        {
            for (int i = 0; i < statusConfiguratorsList.StatusCongigurators.Count; i++)
            {
                if (mapping.StatusType == statusConfiguratorsList.StatusCongigurators[i].StatusType)
                {
                    _statusesBaseParameters.Add(mapping.StatusType, new StatusBaseModel(mapping.StatusComboType, statusConfiguratorsList.StatusCongigurators[i]));
                }
            }
        }

        public StatusBaseModel GetStatusBaseParameters(StatusTypes statusType)
        {
            return _statusesBaseParameters[statusType];
        }

        public void CleanUp()
        {
            _statusesBaseParameters.Clear();
        }
    }
}
