using PassiveAbilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatusEffectConfigurator))]
public class StatusEffectConfiguratorEditor : Editor
{
    private StatusEffectConfigurator _target;
    private GUIStyle _labelstyle;
    private int _statusTypeArrayIndex;
    private SerializedObject _serializedObject;
    private bool _isMappingsLoad;

    private void OnEnable()
    {
        _target = (StatusEffectConfigurator)target;
        _serializedObject = new SerializedObject(_target);

        _labelstyle = new GUIStyle();
        _labelstyle.fixedHeight = 10f;
        _labelstyle.fixedWidth = 200f;
        _labelstyle.font = EditorStyles.boldFont;
        _labelstyle.normal.textColor = Color.white;
    }

    public override void OnInspectorGUI()
    {
        _serializedObject.Update();

        _isMappingsLoad = _target.StatusEffectTypesMappingTable != null;
        var statusTypesNames = Enum.GetNames(typeof(StatusTypes));

        if (!_isMappingsLoad)
        {
            _target.StatusEffectTypesMappingTable = EditorGUILayout.ObjectField("Status Effect Types Mapping Table",
                _target.StatusEffectTypesMappingTable, typeof(StatusEffectTypesMappingTable), false) as StatusEffectTypesMappingTable;
        } else
        {
            _target.StatusEffectID = EditorGUILayout.IntField("Status Effect ID", _target.StatusEffectID, GUILayout.Height(20));
            _statusTypeArrayIndex = (int)_target.StatusType;
            _statusTypeArrayIndex = EditorGUILayout.Popup("", _statusTypeArrayIndex, statusTypesNames);
            _target.StatusType = (StatusTypes)_statusTypeArrayIndex;

            var statusInfluenceType = StatusComboTypes.None;

            for (int i = 0; i < _target.StatusEffectTypesMappingTable.StatusEffectMappings.Count; i++)
            {
                if(_target.StatusType == _target.StatusEffectTypesMappingTable.StatusEffectMappings[i].StatusType)
                {
                    statusInfluenceType = _target.StatusEffectTypesMappingTable.StatusEffectMappings[i].StatusComboType;
                    break;
                }
            }

            _target.StatusComboValue = EditorGUILayout.FloatField("Status Combo Value", _target.StatusComboValue, GUILayout.Height(20));

            switch (statusInfluenceType)
            {
                case StatusComboTypes.HealingAround:
                case StatusComboTypes.DamagingAround:
                    _target.StatusComboRadius = EditorGUILayout.FloatField("Status Combo Radius", _target.StatusComboRadius, GUILayout.Height(20));
                    break;
            }

            switch (_target.StatusType)
            {
                case StatusTypes.Bleeded:
                case StatusTypes.DeepWounded:
                    _target.StatusEffectValue = EditorGUILayout.FloatField("DoT Per Tick Damage", _target.StatusEffectValue, GUILayout.Height(20));
                    break;
                case StatusTypes.Crushed:
                case StatusTypes.Marked:
                case StatusTypes.WeakToMagic:
                case StatusTypes.Cursed:
                case StatusTypes.Trauma:
                    _target.StatusEffectValue = EditorGUILayout.FloatField("Staus Debuff Value", _target.StatusEffectValue, GUILayout.Height(20));
                    break;
            }
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
}
