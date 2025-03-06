using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Equipment
{
    public class ItemPool<T, T2, T3> where T : IItemConfigurator where T2 : IItemFactory<T3, T> where T3 : ItemModel
    {
        protected List<T> _configList;
        protected T2 _factory;
        protected Transform _poolTransform;
        protected Transform _lootPrefabTransform;
        protected int _countItemCopies;

        protected List<T3> _pool = new List<T3>();

        protected ItemPool(Transform poolTransform, Transform lootHolderTransform, int countItemCopies)
        {
            _poolTransform = poolTransform;
            _countItemCopies = countItemCopies;
            _lootPrefabTransform = lootHolderTransform;
        }

        protected async Task CreatePoolAsync()
        {
            for (int i = 0; i < _configList.Count; i++)
            {
                var countCopies = 0;

                while (countCopies <= _countItemCopies)
                {
                    var armorModel = await _factory.CreateItemAsync(_configList[i]);
                    _pool.Add(armorModel);
                    countCopies++;
                }
            }
        }

        protected void ResetItemModel(T3 itemModel)
        {
            itemModel.ItemObject.transform.position = _poolTransform.position;
            itemModel.ItemObject.transform.SetParent(_poolTransform);
            itemModel.LoadableElementsModel.ItemObject.SetActive(true);
            itemModel.IsDroped = false;
            if (itemModel.LoadableElementsModel.AccessoryObject != null)
            {
                itemModel.LoadableElementsModel.AccessoryObject.SetActive(true);
                itemModel.LoadableElementsModel.AccessoryObject.transform.position = _poolTransform.position;
                itemModel.LoadableElementsModel.AccessoryObject.transform.SetParent(_poolTransform);
            }
            if (itemModel.LoadableElementsModel.BackslotObject != null)
            {
                itemModel.LoadableElementsModel.BackslotObject.SetActive(true);
                itemModel.LoadableElementsModel.BackslotObject.transform.position = _poolTransform.position;
                itemModel.LoadableElementsModel.BackslotObject.transform.SetParent(_poolTransform);
            }
        }

        protected T3 FindFreeModel(int itemID)
        {
            T3 itemModel = null;

            for(int i = _pool.Count-1; i >= 0; i--)
            {
                if(_pool[i].ItemID == itemID && !_pool[i].IsDroped)
                {
                    itemModel = _pool[i];
                    break; // В конвенции написано что break вроде как нельзя использовать, но уточнить, конекретно тут он для ускорения работы алгоритма
                }
            }
            return itemModel; 
        }

        public async Task<T3> GetItemAsync(int itemID)
        {
            T3 itemModel = null;

            itemModel = FindFreeModel(itemID);
            if (itemModel is null)
            {
                var itemProtoModel = _configList.Find(config => config.ItemID == itemID);
                itemModel = await _factory.CreateItemAsync(itemProtoModel);
            }

            _pool.Remove(itemModel);
            return itemModel;
        }

        public async Task<T3> GetItemForDropAsync (int itemID)
        {
            T3 itemModel = null;

            itemModel = FindFreeModel(itemID);
            if (itemModel is null)
            {
                var itemProtoModel = _configList.Find(config => config.ItemID == itemID);
                itemModel = await _factory.CreateItemAsync(itemProtoModel);
            }

            return itemModel;
        }

        public void RemoveSelectedItemFromPool (T3 selectedItemModel)
        {
            var itemModel = _pool.Find(model => model == selectedItemModel);
            if (itemModel != null)
            {
                _pool.Remove(itemModel);
                itemModel.IsDroped = false;
            }
        }

        public void ReturnItem(T3 itemModel)
        {
            ResetItemModel(itemModel);
            _pool.Add(itemModel);
        }

        public void ReleasePool()
        {
            for (int i = _pool.Count-1; i >= 0; i--)
                //foreach (var itemModel in _pool)
            {
                _pool[i].LoadableElementsModel.Release();
                _pool.Remove(_pool[i]);
            }
        }
    }
}