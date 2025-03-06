#if UNITY_EDITOR
using Abilities;
using Codice.Client.BaseCommands.BranchExplorer.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Units.UnitsParameters;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PassiveAbilities
{
    [CustomEditor(typeof(PassiveAbilityConfigurator))]
    public class PassiveAbilityConfiguratorEditor : Editor
    {
        private PassiveAbilityConfigurator _target;
        private GUIStyle _labelStyle;
        private GUIStyle _warningStyle;
        private int _passiveAbilityTypeArrayIndex;
        private int _parameterTypeArrayIndex;
        private int _animationEffectArrayIndex;
        private SerializedObject _serializedObject;
        private bool _isLocked;


        private void OnEnable()
        {
            _target = (PassiveAbilityConfigurator)target;
            _serializedObject = new SerializedObject(_target);

            _labelStyle = new GUIStyle();
            _labelStyle.fixedHeight = 20f;
            _labelStyle.font = EditorStyles.boldFont;
            _labelStyle.normal.textColor = Color.white;

            _warningStyle = new GUIStyle();
            _warningStyle.fixedHeight = 20f;
            _warningStyle.normal.textColor = Color.red;
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            var abilityTypesNames = Enum.GetNames(typeof(PassiveAbilityTypes));
            var parameterTypesNames = Enum.GetNames(typeof(ParameterTypes));

            var buffDataProperty = _serializedObject.FindProperty(nameof(_target.ChengableParameters));

            _target.PassiveAbilityID = EditorGUILayout.IntField("Passive Ability ID", _target.PassiveAbilityID, GUILayout.Height(20));
            _target.PassiveAbilityName = EditorGUILayout.TextField("Passive Ability name", _target.PassiveAbilityName, GUILayout.Height(20));

            GUIStyle areaStyle = new GUIStyle(EditorStyles.textArea);
            GUILayout.Label("Passive Ability Discription", GUILayout.Height(20));
            _target.PassiveAbilityDiscription = EditorGUILayout.TextArea(_target.PassiveAbilityDiscription, areaStyle, GUILayout.Height(40));

            EditorGUILayout.Space();

            _target.PassiveAbilitySound = EditorGUILayout.ObjectField("Passive Ability Sound", _target.PassiveAbilitySound, typeof(AudioClip), false) as AudioClip;
            _target.PassiveAbilityIcon = EditorGUILayout.ObjectField("Passive Ability Icon", _target.PassiveAbilityIcon, typeof(Sprite), false,
                GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
            _target.PassiveAbilityIcon = EditorGUILayout.ObjectField("", _target.PassiveAbilityIcon, typeof(Sprite), false) as Sprite;

            Resources.LoadAll<PassiveAbilityVisualEffectBase>("Assets/Prefabs/PassiveAbilityAnimations");
            var animationEffects = Resources.FindObjectsOfTypeAll<PassiveAbilityVisualEffectBase>();

            var animationEffectsNames = new string[animationEffects.Length + 1];
            for (int element = 0; element < animationEffects.Length; element++)
            {
                if(element == 0)
                {
                    animationEffectsNames[element] = "Select animation effect";
                }
                animationEffectsNames[element + 1] = animationEffects[element].name;
            }


            if(_target.AnimationEffectName == "")
            {
                _animationEffectArrayIndex = 0;
            }
            else
            {
                for(int i = 0; i < animationEffectsNames.Length; i++)
                {
                    if (animationEffectsNames[i] == _target.AnimationEffectName)
                    {
                        _animationEffectArrayIndex = i;
                    }
                }
            }
            _animationEffectArrayIndex = EditorGUILayout.Popup("Ability Animation Effect", _animationEffectArrayIndex, animationEffectsNames);
            _target.AnimationEffectName = animationEffectsNames[_animationEffectArrayIndex];

            if (animationEffectsNames.Length > 0)
            {
                if (_animationEffectArrayIndex != 0)
                {
                    _target.PassiveAbilityAnimationEffectID = animationEffects[_animationEffectArrayIndex - 1].EffectID;
                }
                else
                {
                    _target.PassiveAbilityAnimationEffectID = PassiveAbilityConfigurator.NO_ANIMATION_EFFECT_ID;
                }
            } else
            {
                var style = new GUIStyle();
                style.fixedHeight = 20f;
                style.font = EditorStyles.boldFont;
                style.normal.textColor = Color.red;

                GUILayout.Label(new GUIContent("Ошибка загрузки префабов анимаций умений"), style);
            }

            _passiveAbilityTypeArrayIndex = (int)_target.PassiveAbilityType;
            var oldIndex = _passiveAbilityTypeArrayIndex;

            _passiveAbilityTypeArrayIndex = EditorGUILayout.Popup("Passive Ability Type", _passiveAbilityTypeArrayIndex, abilityTypesNames);
            _target.PassiveAbilityType = (PassiveAbilityTypes)_passiveAbilityTypeArrayIndex;

            if (_passiveAbilityTypeArrayIndex != oldIndex)
            {
                _target.ClearConfiguratorSecondaryParameters();
            }

            switch (_target.PassiveAbilityType)
            {
                case PassiveAbilityTypes.TargetDamage:
                    _target.Impact = EditorGUILayout.FloatField("Damage", _target.Impact, GUILayout.Height(20));
                    ShowStatusEffectOption();
                    break;
                case PassiveAbilityTypes.RandomRenewalBuffAroundCaster:
                case PassiveAbilityTypes.RandomRenewalBuffAroundTarget:
                    _target.ImpactDuration = EditorGUILayout.FloatField("Buff Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.ImpactRadius = EditorGUILayout.FloatField("Searching Radius", _target.ImpactRadius, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Passive Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    ShowChangebleParameters(parameterTypesNames, buffDataProperty, "Buff Value");
                    break;
                case PassiveAbilityTypes.RenewalTargetDebuff:
                    _target.ImpactDuration = EditorGUILayout.FloatField("Debuff Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Passive Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    ShowChangebleParameters(parameterTypesNames, buffDataProperty, "Deuff Value");
                    break;
                case PassiveAbilityTypes.RenewalHoTAroundCaster:
                case PassiveAbilityTypes.RenewalHoTAroundTarget:
                    _target.Impact = EditorGUILayout.FloatField("Heal on tick", _target.Impact, GUILayout.Height(20));
                    _target.ImpactDuration = EditorGUILayout.FloatField("HoT Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.ImpactCounts = EditorGUILayout.IntField("HoT Frequency", _target.ImpactCounts, GUILayout.Height(20));
                    _target.ImpactRadius = EditorGUILayout.FloatField("Heal Radius", _target.ImpactRadius, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Passive Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    break;
                case PassiveAbilityTypes.RenewalTargetDoT:
                    _target.Impact = EditorGUILayout.FloatField("Damage on tick", _target.Impact, GUILayout.Height(20));
                    _target.ImpactDuration = EditorGUILayout.FloatField("DoT Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.ImpactCounts = EditorGUILayout.IntField("Tick counts", _target.ImpactCounts, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Passive Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    break;
                case PassiveAbilityTypes.AoEDamageAroundCaster:
                case PassiveAbilityTypes.AoEDamageAroundTarget:
                    _target.Impact = EditorGUILayout.FloatField("Damage", _target.Impact, GUILayout.Height(20));
                    _target.ImpactRadius = EditorGUILayout.FloatField("Damage Radius", _target.ImpactRadius, GUILayout.Height(20));
                    ShowStatusEffectOption();
                    break;
                case PassiveAbilityTypes.RandomTargetHealAroundCaster:
                case PassiveAbilityTypes.AoEHealAroundCaster:
                case PassiveAbilityTypes.AoEHealAroundTarget:
                case PassiveAbilityTypes.RandomTargetHealAroundTarget:
                    _target.Impact = EditorGUILayout.FloatField("Heal", _target.Impact, GUILayout.Height(20));
                    _target.ImpactRadius = EditorGUILayout.FloatField("Heal Radius", _target.ImpactRadius, GUILayout.Height(20));
                    break;
                case PassiveAbilityTypes.MultiplicativeTargetDamage:
                    _target.Impact = EditorGUILayout.FloatField("Weapon Damage Modifier", _target.Impact, GUILayout.Height(20));
                    ShowStatusEffectOption();
                    break;
                case PassiveAbilityTypes.Provoke:
                    ShowStatusEffectOption();
                    break;
                case PassiveAbilityTypes.AOEProvoke:
                    _target.ImpactRadius = EditorGUILayout.FloatField("Provoke Radius", _target.ImpactRadius, GUILayout.Height(20));
                    ShowStatusEffectOption();
                    break;
                case PassiveAbilityTypes.StackableRandomBuffAroundCaster:
                case PassiveAbilityTypes.StackableRandomBuffAroundTarget:
                    _target.ImpactDuration = EditorGUILayout.FloatField("Buff Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.ImpactRadius = EditorGUILayout.FloatField("Search target Radius", _target.ImpactRadius, GUILayout.Height(20));
                    _target.MaxStackCount = EditorGUILayout.IntField("Max Stack Count", _target.MaxStackCount, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Passive Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    ShowChangebleParameters(parameterTypesNames, buffDataProperty, "Buff Value");
                    break;
                case PassiveAbilityTypes.StackableDebuff:
                    _target.ImpactDuration = EditorGUILayout.FloatField("Debuff Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.MaxStackCount = EditorGUILayout.IntField("Max Stack Count", _target.MaxStackCount, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Passive Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    ShowChangebleParameters(parameterTypesNames, buffDataProperty, "Deuff Value");
                    break;
                case PassiveAbilityTypes.StackableTargetDoT:
                    _target.Impact = EditorGUILayout.FloatField("Damage on tick", _target.Impact, GUILayout.Height(20));
                    _target.ImpactDuration = EditorGUILayout.FloatField("DoT Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.MaxStackCount = EditorGUILayout.IntField("Max Stack Count", _target.MaxStackCount, GUILayout.Height(20));
                    _target.ImpactCounts = EditorGUILayout.IntField("Tick counts", _target.ImpactCounts, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Passive Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    break;
                case PassiveAbilityTypes.None:
                default:
                    break;
            }

            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_target);

            EditorGUILayout.Space();
            var saveButton = GUILayout.Button(new GUIContent("Save Changes", "Save"), GUILayout.Width(100f));

            if (saveButton)
            {
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }

        private void ShowStatusEffectOption()
        {
            _target.IsHaveStatusEffect = EditorGUILayout.Toggle(new GUIContent("Have Status Effect?"), _target.IsHaveStatusEffect);

            if (_target.IsHaveStatusEffect)
            {
                var statusEffectProperty = _serializedObject.FindProperty(nameof(_target.PassiveAbilityStatusEffect));
                EditorGUILayout.PropertyField(statusEffectProperty, new GUIContent("Status Effect"), false);
            }
            else
            {
                if (_target.PassiveAbilityStatusEffect != null)
                {
                    _target.ClearStatusEffect();
                }
            }
        }

        private void ShowChangebleParameters(string[] parameterTypesNames, SerializedProperty buffDataProperty, string valueName)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(buffDataProperty, new GUIContent("Chengable Parameters"), false);
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(_target.ChengableParameters.Count.ToString()), _labelStyle);

            EditorGUILayout.EndHorizontal();

            if (buffDataProperty.isExpanded)
            {
                for (int i = 0; i < _target.ChengableParameters.Count; i++)
                {
                    GUILayout.Label("Parameter " + i, _labelStyle);
                    _target.ChengableParameters[i].Impact = EditorGUILayout.FloatField(valueName, _target.ChengableParameters[i].Impact, GUILayout.Height(20));
                    _parameterTypeArrayIndex = (int) _target.ChengableParameters[i].ImpactParameter;
                    _parameterTypeArrayIndex = EditorGUILayout.Popup("Parameter Type", _parameterTypeArrayIndex, parameterTypesNames);
                    _target.ChengableParameters[i].ImpactParameter = (ParameterTypes)_parameterTypeArrayIndex;
                }
                ShowButtons();
            }
        }

        private void ShowButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
            var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
            var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

            if (addButton)
            {
                _target.AddChangableParameter();
            }

            if (deleteButton)
            {
                _target.RemoveLastChangableParameter();
            }

            if (clearButton)
            {
                _target.ClearChangableParameters();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
#endif