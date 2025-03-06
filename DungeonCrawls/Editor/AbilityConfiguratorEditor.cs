#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Units.UnitsParameters;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Abilities
{
    [CustomEditor(typeof(AbilityConfigurator))]
    public class AbilityConfiguratorEditor : Editor
    {
        private AbilityConfigurator _target;
        private GUIStyle _labelstyle;
        private int _abilityTypeArrayIndex;
        private int _targetTypeArrayIndex;
        private int _parameterTypeArrayIndex;
        private SerializedObject _serializedObject;

        private void OnEnable()
        {
            _target = (AbilityConfigurator)target;
            _serializedObject = new SerializedObject(_target);

            _labelstyle = new GUIStyle();
            _labelstyle.fixedHeight = 20f;
            _labelstyle.font = EditorStyles.boldFont;
            _labelstyle.normal.textColor = Color.white;
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            var abilityTypesNames = Enum.GetNames(typeof(AbilityTypes));
            var parameterTypesNames = Enum.GetNames(typeof(ParameterTypes));
            var targetTypesNames = Enum.GetNames(typeof(TargetTypes));

            var buffDataProperty = _serializedObject.FindProperty(nameof(_target.ChengableParameters));

            _target.AbilityID = EditorGUILayout.IntField("Ability ID", _target.AbilityID, GUILayout.Height(20));
            _target.AbilityName = EditorGUILayout.TextField("Ability name", _target.AbilityName, GUILayout.Height(20));

            GUIStyle areaStyle = new GUIStyle(EditorStyles.textArea);
            GUILayout.Label("Ability Discription", GUILayout.Height(20));
            _target.AbilityDiscription = EditorGUILayout.TextArea(_target.AbilityDiscription, areaStyle, GUILayout.Height(40));

            _targetTypeArrayIndex = (int)_target.TargetType;
            _targetTypeArrayIndex = EditorGUILayout.Popup("Target Type", _targetTypeArrayIndex, targetTypesNames);
            _target.TargetType = (TargetTypes)_targetTypeArrayIndex;

            _target.AbilityCost = EditorGUILayout.FloatField("Ability Cost", _target.AbilityCost, GUILayout.Height(20));
            _target.AbilityRange = EditorGUILayout.FloatField("Ability Range", _target.AbilityRange, GUILayout.Height(20));

            EditorGUILayout.Space();
            _target.MasteryRequirement = EditorGUILayout.FloatField("Mastery Requirement Value", _target.MasteryRequirement, GUILayout.Height(20));
            EditorGUILayout.Space();

            _target.AbilityCooldown = EditorGUILayout.FloatField("Ability Cooldown", _target.AbilityCooldown, GUILayout.Height(20));
            _target.AbilityIcon = EditorGUILayout.ObjectField("Ability Icon", _target.AbilityIcon, typeof(Sprite), false,
                GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
            _target.AbilityIcon = EditorGUILayout.ObjectField("", _target.AbilityIcon, typeof(Sprite), false) as Sprite;

            _abilityTypeArrayIndex = (int)_target.AbilityType;
            var oldIndex = _abilityTypeArrayIndex;

            _abilityTypeArrayIndex = EditorGUILayout.Popup("Ability Type", _abilityTypeArrayIndex, abilityTypesNames);
            _target.AbilityType = (AbilityTypes)_abilityTypeArrayIndex;

            if(_abilityTypeArrayIndex != oldIndex)
            {
                _target.ClearConfiguratorSecondaryParameters();
            }

            switch (_abilityTypeArrayIndex)
            {
                case 0:
                    break;
                case 1:
                    _target.Impact = EditorGUILayout.FloatField("Damage", _target.Impact, GUILayout.Height(20));
                    break;
                case 2:
                    _target.Impact = EditorGUILayout.FloatField("Heal", _target.Impact, GUILayout.Height(20));
                    break;
                case 3:
                    _target.ImpactDuration = EditorGUILayout.FloatField("Buff Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    ShowChangebleParameters(parameterTypesNames, buffDataProperty, "Buff Value");
                    break;
                case 4:
                    _target.ImpactDuration = EditorGUILayout.FloatField("Debuff Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    ShowChangebleParameters(parameterTypesNames, buffDataProperty, "Deuff Value");
                    break;
                case 5:
                    _target.Impact = EditorGUILayout.FloatField("Heal on tick", _target.Impact, GUILayout.Height(20));
                    _target.ImpactDuration = EditorGUILayout.FloatField("HoT Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.ImpactCounts = EditorGUILayout.IntField("HoT Frequency", _target.ImpactCounts, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    break;
                case 6:
                    _target.Impact = EditorGUILayout.FloatField("Damage on tick", _target.Impact, GUILayout.Height(20));
                    _target.ImpactDuration = EditorGUILayout.FloatField("DoT Duration", _target.ImpactDuration, GUILayout.Height(20));
                    _target.ImpactCounts = EditorGUILayout.IntField("Tick counts", _target.ImpactCounts, GUILayout.Height(20));
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("Ability Effect Icon", _target.AbilityEffectIcon, typeof(Sprite), false,
                        GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                    _target.AbilityEffectIcon = EditorGUILayout.ObjectField("", _target.AbilityEffectIcon, typeof(Sprite), false) as Sprite;
                    break;
                case 7:
                    _target.Impact = EditorGUILayout.FloatField("Damage", _target.Impact, GUILayout.Height(20));
                    _target.ImpactRadius = EditorGUILayout.FloatField("Damage Radius", _target.ImpactRadius, GUILayout.Height(20));
                    break;
                case 8:
                    _target.Impact = EditorGUILayout.FloatField("Heal", _target.Impact, GUILayout.Height(20));
                    _target.ImpactRadius = EditorGUILayout.FloatField("Heal Radius", _target.ImpactRadius, GUILayout.Height(20));
                    break;
                case 9:
                    ShowChangebleParameters(parameterTypesNames, buffDataProperty, "Stance Change Value");
                    break;
                case 10:
                    _target.ImpactFrequency = EditorGUILayout.FloatField("Stance Cost Frequency", _target.ImpactFrequency, GUILayout.Height(20));
                    ShowChangebleParameters(parameterTypesNames, buffDataProperty, "Stance Change Value");
                    break;
                case 11:
                    _target.Impact = EditorGUILayout.FloatField("Weapon Damage Modifier", _target.Impact, GUILayout.Height(20));
                    break;
                default:
                    break;
            }

            _serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(_target);

            EditorGUILayout.Space();
            var saveButton = GUILayout.Button(new GUIContent("Save Changes", "Save"), GUILayout.Width(100f));

            if(saveButton)
            {
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }

        private void ShowChangebleParameters(string[] parameterTypesNames, SerializedProperty buffDataProperty, string valueName)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(buffDataProperty, new GUIContent("Chengable Parameters"), false);
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(_target.ChengableParameters.Count.ToString()), _labelstyle);

            EditorGUILayout.EndHorizontal();

            if (buffDataProperty.isExpanded)
            {
                for (int i = 0; i < _target.ChengableParameters.Count; i++)
                {
                    GUILayout.Label("Parameter " + i, _labelstyle);
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