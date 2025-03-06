#if UNITY_EDITOR
using PassiveAbilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatusEffectTypesMappingTable))]
public class StatusEffectTypesMappingTableEditor : Editor
{
    private StatusEffectTypesMappingTable _target;
    private GUIStyle _labelstyle;
    private int _statusInfluenceTypeArrayIndex;
    private SerializedObject _serializedObject;

    private void OnEnable()
    {
        _target = (StatusEffectTypesMappingTable)target;
        _serializedObject = new SerializedObject(_target);

        _labelstyle = new GUIStyle();
        _labelstyle.fixedHeight = 10f;
        _labelstyle.fixedWidth = 200f;
        _labelstyle.font = EditorStyles.boldFont;
        _labelstyle.normal.textColor = Color.white;

        CheckStatusTypes();
    }

    public override void OnInspectorGUI()
    {
        _serializedObject.Update();

        var statusInfluenceTypesNames = Enum.GetNames(typeof(StatusComboTypes));
        var statusEffectMatchingProperty = _serializedObject.FindProperty(nameof(_target.StatusEffectMappings));

        ShowMatchingTable(statusInfluenceTypesNames, statusEffectMatchingProperty, "Status Effect Mappings");

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

    private void ShowMatchingTable(string[] statusInfluenceTypesNames, SerializedProperty statusEffectMatchingProperty, string valueName)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(statusEffectMatchingProperty, new GUIContent(valueName), false);
        GUILayout.FlexibleSpace();
        var numberLabelStyle = new GUIStyle
        {
            fixedHeight = 10f,
            font = EditorStyles.boldFont
        };
        numberLabelStyle.normal.textColor = Color.white;

        GUILayout.Label(new GUIContent(_target.StatusEffectMappings.Count.ToString()), numberLabelStyle);

        EditorGUILayout.EndHorizontal();

        if (statusEffectMatchingProperty.isExpanded)
        {
            GUILayout.Space(10f);
            for (int i = 0; i < _target.StatusEffectMappings.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"{_target.StatusEffectMappings[i].StatusType}", _labelstyle);

                _statusInfluenceTypeArrayIndex = (int)_target.StatusEffectMappings[i].StatusComboType;
                _statusInfluenceTypeArrayIndex = EditorGUILayout.Popup("", _statusInfluenceTypeArrayIndex, statusInfluenceTypesNames);
                _target.StatusEffectMappings[i].StatusComboType = (StatusComboTypes)_statusInfluenceTypeArrayIndex;

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5f);
            }
        }
    }

    private void CheckStatusTypes()
    {
        var statusCount = Enum.GetNames(typeof(StatusTypes)).Length - 1;

        if (_target.StatusEffectMappings.Count != statusCount)
        {
            if (_target.StatusEffectMappings.Count == 0)
            {
                FillStatuses(statusCount);
            }
            else
            {
                List<StatusEffectTypesMapping> oldMappings = new List<StatusEffectTypesMapping>(_target.StatusEffectMappings);
                _target.StatusEffectMappings.Clear();

                FillStatuses(statusCount);

                for (int i = 0; i < _target.StatusEffectMappings.Count; i++)
                {
                    for (int j = 0; j < oldMappings.Count; j++)
                    {
                        if (_target.StatusEffectMappings[i].StatusType == oldMappings[j].StatusType)
                        {
                            _target.StatusEffectMappings[i].StatusComboType = oldMappings[j].StatusComboType;
                            continue;
                        }
                    }
                }
            }
        }
    }

    private void FillStatuses(int statusCount)
    {
        for (int i = 0; i < statusCount; i++)
        {
            _target.StatusEffectMappings.Add(new StatusEffectTypesMapping()
            {
                StatusType = (StatusTypes)(i + 1),
                StatusComboType = StatusComboTypes.None
            });
        }
    }
}
#endif
