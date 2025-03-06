using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PassiveAbilities
{
    [CreateAssetMenu(menuName = "PassiveAbility/PassiveAbilitiyAnimationReferences", fileName = nameof(PassiveAbilityAnimanitonReferences))]
    public class PassiveAbilityAnimanitonReferences : ScriptableObject
    {
        //[field: SerializeField] public AssetReferenceGameObject StrikeAOE { get; private set; }
        //[field: SerializeField] public AssetReferenceGameObject StrikeTarget { get; private set; }

        [field: SerializeField] public List<AssetReferenceGameObject> PassiveAbilityVisualEffects { get; private set; }
    }
}