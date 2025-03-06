#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkinnedMeshRenderer))]
public class SkinMeshRendererEditor : Editor
{
    private SkinnedMeshRenderer _target;
    private SerializedObject _serializedObject;

    public Transform[] BonesArray;

    private void OnEnable()
    {
        _target = (SkinnedMeshRenderer)target;
        BonesArray = _target.bones;
        _serializedObject = new SerializedObject(this);
    }

    public override void OnInspectorGUI()
    {
        SerializedProperty property = _serializedObject.FindProperty("BonesArray");
        EditorGUILayout.PropertyField(property, true);
        property.isExpanded = true;
        if(property.hasChildren)
        {
            _serializedObject.ApplyModifiedProperties();
        }

        base.OnInspectorGUI();
    }
}
#endif
