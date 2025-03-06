using Equipment;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PassiveAbilities
{
    [CustomEditor(typeof(PassiveAbilitiesProcChanceConfig))]
    public class PassiveAbilitiesProcChanceConfigEditor : Editor
    {
        private PassiveAbilitiesProcChanceConfig _target;
        private GUIStyle _labelstyle;
        private SerializedObject _serializedObject;
        bool[] _isShowGrades;

        private void OnEnable()
        {
            _target = (PassiveAbilitiesProcChanceConfig)target;
            _serializedObject = new SerializedObject(_target);
            _labelstyle = new GUIStyle();
            _labelstyle.fixedHeight = 10f;
            _labelstyle.fixedWidth = 200f;
            _labelstyle.font = EditorStyles.boldFont;
            _labelstyle.normal.textColor = Color.white;

            CheckWeaponTypes();
            _isShowGrades = new bool[_target.ProcChancesByWeaponTypes.Count];
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            var byWeaponTypesProperty = _serializedObject.FindProperty(nameof(_target.ProcChancesByWeaponTypes));

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(byWeaponTypesProperty, new GUIContent("Proc Chance By Weapon Type"), false);
            GUILayout.FlexibleSpace();
            var numberLabelStyle = new GUIStyle
            {
                fixedHeight = 10f,
                font = EditorStyles.boldFont
            };
            numberLabelStyle.normal.textColor = Color.white;

            GUILayout.Label(new GUIContent(_target.ProcChancesByWeaponTypes.Count.ToString()), numberLabelStyle);

            EditorGUILayout.EndHorizontal();

            if (byWeaponTypesProperty.isExpanded)
            {
                GUILayout.Space(10f);
                for (int i = 0; i < _target.ProcChancesByWeaponTypes.Count; i++)
                {
                    var byGradeProcs = _target.ProcChancesByWeaponTypes[i].ProcChancesByGrades;

                    EditorGUILayout.BeginHorizontal();
                    _isShowGrades[i] = EditorGUILayout.Foldout(_isShowGrades[i], $"{_target.ProcChancesByWeaponTypes[i].WeaponType}");
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(new GUIContent(byGradeProcs.Count.ToString()), numberLabelStyle);
                    EditorGUILayout.EndHorizontal();

                    if (_isShowGrades[i])
                    {
                        for (int j = 0; j < byGradeProcs.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label($"{byGradeProcs[j].GradeType} grade proc chance", _labelstyle);
                            byGradeProcs[j].ProcChance = EditorGUILayout.FloatField(byGradeProcs[j].ProcChance);
                            EditorGUILayout.EndHorizontal();
                            GUILayout.Space(5f);
                        }
                    }
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

        private void CheckWeaponTypes()
        {
            var weaponTypesCount = Enum.GetNames(typeof(WeaponTypes)).Length - 1;

            if (_target.ProcChancesByWeaponTypes.Count != weaponTypesCount)
            {
                if (_target.ProcChancesByWeaponTypes.Count == 0)
                {
                    FillWeaponTypes(weaponTypesCount);
                }
                else
                {
                    List<ProcChancesByWeaponTypesConfigurator> oldProcChanceConfigData = new List<ProcChancesByWeaponTypesConfigurator>(_target.ProcChancesByWeaponTypes);
                    _target.ProcChancesByWeaponTypes.Clear();

                    FillWeaponTypes(weaponTypesCount);

                    for (int i = 0; i < _target.ProcChancesByWeaponTypes.Count; i++)
                    {
                        for (int j = 0; j < oldProcChanceConfigData.Count; j++)
                        {
                            if (_target.ProcChancesByWeaponTypes[i].WeaponType == oldProcChanceConfigData[j].WeaponType)
                            {
                                _target.ProcChancesByWeaponTypes[i].ProcChancesByGrades = oldProcChanceConfigData[j].ProcChancesByGrades;
                                continue;
                            }
                        }
                    }
                }
            }
        }

        private void FillWeaponTypes(int weaponTypesCount)
        {
            for (int i = 0; i < weaponTypesCount; i++)
            {
                var procChancesByWeaponTypesModel = new ProcChancesByWeaponTypesConfigurator
                { 
                    WeaponType = (WeaponTypes)i + 1,
                };

                _target.ProcChancesByWeaponTypes.Add(procChancesByWeaponTypesModel);
                var gradesCount = Enum.GetNames(typeof(GradeTypes)).Length - 1;

                for (int j = 0; j < gradesCount; j++)
                {
                    procChancesByWeaponTypesModel.ProcChancesByGrades.Add(new ProcChancesByGradeConfigurator
                    {
                        GradeType = (GradeTypes)j + 1,
                        ProcChance = default
                    });
                }
            }
        }
    }
}