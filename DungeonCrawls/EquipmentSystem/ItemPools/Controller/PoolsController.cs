using Engine;
using Equipment.MarkerClasses;
using UnityEngine;
using Utils;

namespace Equipment
{
    public class PoolsController : IController, ICleanable
    {
        private ArmorPool _armorPool;
        private WeaponPool _weaponPool;
       // private Transform _lootDropPrefabHolder;
        private GameObject _weaponPoolHolder;
        private GameObject _armorPoolHolder;
        private GameObject _lootDropPrefabHolder;
       

        public ArmorPool ArmorPool => _armorPool;
        public WeaponPool WeaponPool => _weaponPool;
        public Transform LootDropPrefabHolder => _lootDropPrefabHolder.transform;

        public PoolsController(GlobalConfigLoader configLoader)
        {
            Vector3 holdersPosition = new Vector3(10,-10,10);
            if (!Object.FindObjectOfType<WeaponPoolMarker>())
            {
                _weaponPoolHolder = new GameObject("WeaponPoolHolder");
                _weaponPoolHolder.transform.position = holdersPosition;
                _weaponPoolHolder.AddComponent<WeaponPoolMarker>();
                Object.DontDestroyOnLoad(_weaponPoolHolder);
                
            }
            else
            {
                _weaponPoolHolder = Object.FindObjectOfType<WeaponPoolMarker>().gameObject;
            }
            if (!Object.FindObjectOfType<ArmorPoolMarker>())
            {
                _armorPoolHolder = new GameObject("ArmorPoolHolder");
                _armorPoolHolder.transform.position = holdersPosition;
                _armorPoolHolder.AddComponent<ArmorPoolMarker>();
                Object.DontDestroyOnLoad(_armorPoolHolder);
            }
            else
            {
                _armorPoolHolder = Object.FindObjectOfType<ArmorPoolMarker>().gameObject;
            }
            if (!Object.FindObjectOfType<DropHolderMarker>())
            {
                _lootDropPrefabHolder = new GameObject("LootPrefabHolder");
                _lootDropPrefabHolder.transform.position = holdersPosition;
                _lootDropPrefabHolder.AddComponent<DropHolderMarker>();
                Object.DontDestroyOnLoad(_lootDropPrefabHolder);
            }
            else
            {
                _lootDropPrefabHolder = Object.FindObjectOfType<DropHolderMarker>().gameObject;
            }
            _armorPool = new ArmorPool(_armorPoolHolder.transform, _lootDropPrefabHolder.transform,configLoader );
            _weaponPool = new WeaponPool (_weaponPoolHolder.transform,_lootDropPrefabHolder.transform, configLoader);
           // _lootDropPrefabHolder = poolsModel.LootPrefabHolderTransform;
        }

        public void CleanUp()
        {
            _weaponPool.ReleasePool();
            _armorPool.ReleasePool();
        }
    }
}