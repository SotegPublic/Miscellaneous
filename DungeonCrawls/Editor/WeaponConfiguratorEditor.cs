#if UNITY_EDITOR
using Abilities;
using PassiveAbilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Equipment
{
    [CustomEditor(typeof(WeaponConfigurator)), CanEditMultipleObjects]
    public class WeaponConfiguratorEditor : Editor
    {
        private WeaponConfigurator _target;
        private GUIStyle _labelstyle;
        private SerializedObject _serializedObject;

        private bool _isAbilitiesShow;
        private bool _isPassiveAbilitiesShow;

        private void OnEnable()
        {
            _target = (WeaponConfigurator)target;
            _serializedObject = new SerializedObject(_target);
            _labelstyle = new GUIStyle();
            _labelstyle.fixedHeight = 20f;
            _labelstyle.font = EditorStyles.boldFont;
            _labelstyle.normal.textColor = Color.white;
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            _isAbilitiesShow = EditorGUILayout.Foldout(_isAbilitiesShow, "Weapon Abilities");
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(_target.AbilityConfigurators.Count.ToString()), _labelstyle);

            EditorGUILayout.EndHorizontal();

            if (_isAbilitiesShow)
            {
                for (int i = 0; i < _target.AbilityConfigurators.Count; i++)
                {
                    GUILayout.Label("Ability " + i, _labelstyle);
                    _target.AbilityConfigurators[i] = EditorGUILayout.ObjectField("Ability Config", _target.AbilityConfigurators[i], typeof(AbilityConfigurator),
                        false, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as AbilityConfigurator;
                }

                ShowAbilitiesButtons();
            }

            EditorGUILayout.BeginHorizontal();

            _isPassiveAbilitiesShow = EditorGUILayout.Foldout(_isPassiveAbilitiesShow, "Weapon Passive Abilities");
            GUILayout.FlexibleSpace();
            GUILayout.Label(new GUIContent(_target.PassiveAbilityConfigurators.Count.ToString()), _labelstyle);

            EditorGUILayout.EndHorizontal();

            if (_isPassiveAbilitiesShow)
            {
                for (int i = 0; i < _target.PassiveAbilityConfigurators.Count; i++)
                {
                    GUILayout.Label("PassiveAbility " + i, _labelstyle);
                    _target.PassiveAbilityConfigurators[i] = EditorGUILayout.ObjectField("Passive Ability Config", _target.PassiveAbilityConfigurators[i], typeof(PassiveAbilityConfigurator),
                        false, GUILayout.Height(EditorGUIUtility.singleLineHeight)) as PassiveAbilityConfigurator;
                }

                ShowPassiveAbilitiesButtons();
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

        private void ShowAbilitiesButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            bool addButton = false;

            if (_target.WeaponGripType != WeaponGripTypes.TwoHand)
            {
                if (_target.AbilityConfigurators.Count < 5)
                {
                    addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
                }
            } else
            {
                if (_target.AbilityConfigurators.Count < 10)
                {
                    addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
                }
            }

            var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
            var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

            if (addButton)
            {
                _target.AddAbilityConfigurator();
            }

            if (deleteButton)
            {
                _target.RemoveLastAbilityConfigurator();
            }

            if (clearButton)
            {
                _target.ClearAbilityConfigurators();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowPassiveAbilitiesButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            bool addButton = false;

            if (_target.WeaponGripType != WeaponGripTypes.TwoHand)
            {
                if (_target.PassiveAbilityConfigurators.Count < 1)
                {
                    addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
                }
            }
            else
            {
                if (_target.PassiveAbilityConfigurators.Count < 2)
                {
                    addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
                }
            }

            var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
            var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

            if (addButton)
            {
                _target.AddPassiveAbilityConfigurator();
            }

            if (deleteButton)
            {
                _target.RemoveLastPassiveAbilityConfigurator();
            }

            if (clearButton)
            {
                _target.ClearPassiveAbilityConfigurators();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif