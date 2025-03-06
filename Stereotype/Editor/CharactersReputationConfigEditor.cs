using NPCCharacters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Codice.CM.Common.CmCallContext;
using static PlasticPipe.PlasticProtocol.Messages.Serialization.ItemHandlerMessagesSerialization;

[CustomEditor(typeof(CharactersReputationConfig))]
public class CharactersReputationConfigEditor : Editor
{
    private CharactersReputationConfig _target;
    private GUIStyle _labelStyle;
    private GUIStyle _errorStyle;
    private SerializedObject _serializedObject;
    private bool _isRepLevelsShow;

    private void OnEnable()
    {
        _target = (CharactersReputationConfig)target;
        _serializedObject = new SerializedObject(_target);

        _labelStyle = new GUIStyle();
        _labelStyle.fixedHeight = 10f;
        _labelStyle.fixedWidth = 200f;
        _labelStyle.font = EditorStyles.boldFont;
        _labelStyle.normal.textColor = Color.white;

        _errorStyle = new GUIStyle();
        _errorStyle.fixedHeight = 10f;
        _errorStyle.fixedWidth = 200f;
        _errorStyle.font = EditorStyles.boldFont;
        _errorStyle.normal.textColor = Color.red;
    }

    public override void OnInspectorGUI()
    {
        _serializedObject.Update();
        EditorGUILayout.Space(10f);
        EditorGUI.BeginDisabledGroup(true);
        _target.MaxReputationLevelsCount = EditorGUILayout.IntField("Max Reputation Levels Count", _target.MaxReputationLevelsCount, GUILayout.Height(20));
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.Space(5f);

        EditorGUILayout.BeginHorizontal();
        _isRepLevelsShow = EditorGUILayout.Foldout(_isRepLevelsShow, "List Of Reputation Per Level");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10f);

        if (_isRepLevelsShow)
        {
            for (int i = 0; i < _target.ListOfReputationPerLevel.Count; i++)
            {
                GUILayout.Label("Level " + i, _labelStyle);
                _target.ListOfReputationPerLevel[i] = EditorGUILayout.IntField("Reputation Count for Up", _target.ListOfReputationPerLevel[i], GUILayout.Height(20));
            }
            EditorGUILayout.Space(10f);
            ShowButtons();
        }


        _serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_target);

        EditorGUILayout.Space();

        if(CheckForCorrectInput())
        {
            var saveButton = GUILayout.Button(new GUIContent("Save Changes", "Save"), GUILayout.Width(100f));

            if (saveButton)
            {
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }
        else
        {
            GUILayout.Label("Ошибка ввода данных\nЛибо ввод не был завершен. Проверьте данные", _errorStyle);
        }
    }

    private bool CheckForCorrectInput()
    {
        bool isError = false;

        for(int i = 0; i < _target.ListOfReputationPerLevel.Count; i++)
        {
            if(i == 0)
            {
                continue;
            }
            
            if (_target.ListOfReputationPerLevel[i] < _target.ListOfReputationPerLevel[i - 1])
            {
                isError = true;
            }
        }

        return !isError;
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
            _target.AddLevel();
            _target.MaxReputationLevelsCount = _target.ListOfReputationPerLevel.Count;
        }

        if (deleteButton)
        {
            _target.RemoveLastLevel();
            _target.MaxReputationLevelsCount = _target.ListOfReputationPerLevel.Count;
        }

        if (clearButton)
        {
            _target.ClearList();
            _target.MaxReputationLevelsCount = _target.ListOfReputationPerLevel.Count;
        }
        EditorGUILayout.EndHorizontal();
    }
}
