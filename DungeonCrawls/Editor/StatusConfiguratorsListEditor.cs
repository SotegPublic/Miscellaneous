using Equipment;
using PassiveAbilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(StatusConfiguratorsList))]
public class StatusConfiguratorsListEditor : Editor
{
    private StatusConfiguratorsList _target;
    private GUIStyle _labelstyle;
    private SerializedObject _serializedObject;
    private int _weaponTypeArrayIndex;
    bool[] _showWeapons;

    private void OnEnable()
    {
        _target = (StatusConfiguratorsList)target;
        _serializedObject = new SerializedObject(_target);

        _labelstyle = new GUIStyle();
        _labelstyle.fixedHeight = 10f;
        _labelstyle.fixedWidth = 200f;
        _labelstyle.font = EditorStyles.boldFont;
        _labelstyle.normal.textColor = Color.white;

        CheckStatusTypes();
        _showWeapons = new bool[_target.StatusCongigurators.Count];
    }

    public override void OnInspectorGUI()
    {
        _serializedObject.Update();

        var statusConfigListProperty = _serializedObject.FindProperty(nameof(_target.StatusCongigurators));
        var weaponTypesNames = Enum.GetNames(typeof(WeaponTypes));

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(statusConfigListProperty, new GUIContent("Status Configurators List"), false);
        GUILayout.FlexibleSpace();
        var numberLabelStyle = new GUIStyle
        {
            fixedHeight = 10f,
            font = EditorStyles.boldFont
        };
        numberLabelStyle.normal.textColor = Color.white;

        GUILayout.Label(new GUIContent(_target.StatusCongigurators.Count.ToString()), numberLabelStyle);

        EditorGUILayout.EndHorizontal();

        if (statusConfigListProperty.isExpanded)
        {

            GUILayout.Space(10f);
            for (int i = 0; i < _target.StatusCongigurators.Count; i++)
            {
                var buffedWeaponsList = _target.StatusCongigurators[i].BuffedWeapons;
                GUILayout.Label($"{_target.StatusCongigurators[i].StatusType}", _labelstyle);
                GUILayout.Space(5f);
                _target.StatusCongigurators[i].Name = EditorGUILayout.TextField("Status Name", _target.StatusCongigurators[i].Name, GUILayout.Height(20));
                GUILayout.Label("Status Effect Discription", GUILayout.Height(20));
                _target.StatusCongigurators[i].Description = EditorGUILayout.TextArea(_target.StatusCongigurators[i].Description, new GUIStyle(EditorStyles.textArea), GUILayout.Height(60));
                _target.StatusCongigurators[i].Duration = EditorGUILayout.FloatField("Status Duration", _target.StatusCongigurators[i].Duration, GUILayout.Height(20));

                switch (_target.StatusCongigurators[i].StatusType)
                {
                    case StatusTypes.Bleeded:
                    case StatusTypes.DeepWounded:
                        _target.StatusCongigurators[i].Frequency = EditorGUILayout.FloatField("DoT Frequency", _target.StatusCongigurators[i].Frequency, GUILayout.Height(20));
                        break;
                    default:
                        break;
                }

                _target.StatusCongigurators[i].ProlongationTime = EditorGUILayout.FloatField("Status Prolongation Time", _target.StatusCongigurators[i].ProlongationTime, GUILayout.Height(20));
                EditorGUILayout.BeginHorizontal();
                _showWeapons[i] = EditorGUILayout.Foldout(_showWeapons[i], "Buffed Weapon Types");
                GUILayout.FlexibleSpace();
                GUILayout.Label(new GUIContent(buffedWeaponsList.Count.ToString()), numberLabelStyle);
                EditorGUILayout.EndHorizontal();

                if (_showWeapons[i])
                {
                    for (int j = 0; j < buffedWeaponsList.Count; j++)
                    {
                        _weaponTypeArrayIndex = (int)buffedWeaponsList[j];
                        _weaponTypeArrayIndex = EditorGUILayout.Popup("Weapon Type " + j, _weaponTypeArrayIndex, weaponTypesNames);
                        buffedWeaponsList[j] = (WeaponTypes)_weaponTypeArrayIndex;
                    }

                    ShowButtons(i);
                }

                _target.StatusCongigurators[i].StatusIcon = EditorGUILayout.ObjectField("Status Effect Icon", _target.StatusCongigurators[i].StatusIcon, typeof(Sprite), false,
                    GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
                GUILayout.Space(15f);
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

    private void ShowButtons(int index)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        var addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
        var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
        var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

        if (addButton)
        {
            _target.StatusCongigurators[index].AddBuffedWeapon();
        }

        if (deleteButton)
        {
            _target.StatusCongigurators[index].RemoveLastBuffedWeapon();
        }

        if (clearButton)
        {
            _target.StatusCongigurators[index].ClearBuffedWeapons();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void CheckStatusTypes()
    {
        var statusCount = Enum.GetNames(typeof(StatusTypes)).Length - 1;

        if (_target.StatusCongigurators.Count != statusCount)
        {
            if (_target.StatusCongigurators.Count == 0)
            {
                FillStatuses(statusCount);
            }
            else
            {
                List<StatusCongigurator> oldStatusList = new List<StatusCongigurator>(_target.StatusCongigurators);
                _target.StatusCongigurators.Clear();

                FillStatuses(statusCount);

                for (int i = 0; i < _target.StatusCongigurators.Count; i++)
                {
                    for (int j = 0; j < oldStatusList.Count; j++)
                    {
                        if (_target.StatusCongigurators[i].StatusType == oldStatusList[j].StatusType)
                        {
                            _target.StatusCongigurators[i].Description = oldStatusList[j].Description;
                            _target.StatusCongigurators[i].Name = oldStatusList[j].Name;
                            _target.StatusCongigurators[i].BuffedWeapons = oldStatusList[j].BuffedWeapons;
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
            _target.StatusCongigurators.Add(new StatusCongigurator()
            {
                StatusType = (StatusTypes)(i + 1),
                Name = "",
                Description = "",
                BuffedWeapons = new List<WeaponTypes>()
            });
        }
    }
}
