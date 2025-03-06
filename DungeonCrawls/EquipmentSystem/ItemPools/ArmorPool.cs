using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Equipment
{
    public class ArmorPool: ItemPool<IArmorConfigurator, IItemFactory<ArmorModel, IArmorConfigurator>, ArmorModel>
    {
        public ArmorPool(Transform poolTransform, Transform lootHolderTransform, GlobalConfigLoader globalConfigLoader):
            base(poolTransform, lootHolderTransform, 0) // TODO: вынести в конфиг
        {
            _configList = new List<IArmorConfigurator>(globalConfigLoader.ArmorComposite.ArmorList);
            _factory = new ArmorFactory(poolTransform, lootHolderTransform, globalConfigLoader);
            CreatePoolAsync();
        }
    }
}