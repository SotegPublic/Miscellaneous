using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Equipment
{
    public class WeaponPool: ItemPool<IWeaponConfigurator, IItemFactory<WeaponModel, IWeaponConfigurator>, WeaponModel>
    {
        public WeaponPool(Transform poolTransform, Transform lootHolderTransform, GlobalConfigLoader globalConfigLoader):
            base(poolTransform, lootHolderTransform, 0)
        {
            _configList = new List<IWeaponConfigurator>(globalConfigLoader.WeaponComposite.WeaponList);
            _factory = new WeaponFactory(poolTransform, poolTransform, globalConfigLoader);
            CreatePoolAsync();
        }
    }
}