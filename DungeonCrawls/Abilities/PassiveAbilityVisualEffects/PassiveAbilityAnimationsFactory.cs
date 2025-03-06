using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using System.Linq;
using Object = UnityEngine.Object;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.VFX;

namespace PassiveAbilities
{
    public class PassiveAbilityAnimationsFactory
    {
        private GlobalConfigLoader _globalConfig;
        public PassiveAbilityAnimationsFactory(GlobalConfigLoader globalConfig)
        {
            _globalConfig = globalConfig;
        }

        public async Task<List<IVisualEffect>> GetAllTypeAnimations(int count)
        {
            var animationObjects = new List<IVisualEffect>();
            var holder = new GameObject("PassiveAbilityAnimationsHolder");
            holder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            List<Task<GameObject>> _tasks = new List<Task<GameObject>>();
            foreach (var visualEffectReference in _globalConfig.PassiveAbilityAnimanitonReferences.PassiveAbilityVisualEffects)
            {  
                for (int i = 0; i < count; i++)
                {
                    var visualEffectObject = Addressables.InstantiateAsync(visualEffectReference, holder.transform);
                    _tasks.Add( visualEffectObject.Task);
                }
            }

            await Task.WhenAll(_tasks);

            foreach (var item in _tasks)
            {
                var animationObject = item.Result.GetComponent<PassiveAbilityVisualEffectBase>();
                animationObjects.Add(animationObject);
            }

            return animationObjects;
        }
    }
}