using Configs.Enum;
using Dialogues;
using NPCCharacters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Quests
{
    [CustomEditor(typeof(QuestConfigurator))]

    public class QuestConfiguratorEditor : Editor
    {
        private QuestConfigurator _target;
        private GUIStyle _stageLabelStyle;
        private GUIStyle _stageDiscriptionLabelStyle;
        private GUIStyle _errorLabelStyle;
        private GUIStyle _rewardLabelStyle;
        private SerializedObject _serializedObject;
        private List<QuestStagesTypes> _questStagesTypes = new List<QuestStagesTypes>();

        private void OnEnable()
        {
            _target = (QuestConfigurator)target;
            _serializedObject = new SerializedObject(_target);

            _stageLabelStyle = new GUIStyle();
            _stageLabelStyle.fixedHeight = 10f;
            _stageLabelStyle.fixedWidth = 200f;
            _stageLabelStyle.font = EditorStyles.boldFont;
            _stageLabelStyle.normal.textColor = Color.white;

            _stageDiscriptionLabelStyle = new GUIStyle();
            _stageDiscriptionLabelStyle.fixedHeight = 10f;
            _stageDiscriptionLabelStyle.fixedWidth = 200f;
            _stageDiscriptionLabelStyle.normal.textColor = new Color(200, 200, 200, 255);
            _stageDiscriptionLabelStyle.margin = new RectOffset(34, 0, 0, 10);

            _rewardLabelStyle = new GUIStyle();
            _rewardLabelStyle.fixedHeight = 10f;
            _rewardLabelStyle.fixedWidth = 200f;
            _rewardLabelStyle.normal.textColor = Color.white;
            _rewardLabelStyle.margin = new RectOffset(64, 0, 10, 5);

            _errorLabelStyle = new GUIStyle();
            _errorLabelStyle.fixedHeight = 10f;
            _errorLabelStyle.fixedWidth = 200f;
            _errorLabelStyle.font = EditorStyles.boldFont;
            _errorLabelStyle.normal.textColor = Color.red;

            for (int i = 0; i < _target.QuestStages.Count; i++)
            {
                _questStagesTypes.Add(_target.QuestStages[i].QuestStageType);
            }
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            _target.QuestGiver = (NPCsTypes)EditorGUILayout.EnumPopup("Quest Giver", _target.QuestGiver);

            _serializedObject.ApplyModifiedProperties();

            _target.QuestName = EditorGUILayout.TextField("Quest name", _target.QuestName, GUILayout.Height(20));
            GUILayout.Label("Quest Discription", GUILayout.Height(20));
            _target.QuestDescription = EditorGUILayout.TextArea(_target.QuestDescription, new GUIStyle(EditorStyles.textArea), GUILayout.Height(60));

            _target.IsReputationRequired = EditorGUILayout.Toggle("Is Reputation Level Required?", _target.IsReputationRequired);

            if (_target.IsReputationRequired)
            {
                _target.CharactersReputationConfigurator = (CharactersReputationConfig)EditorGUILayout.ObjectField("CharactersReputationConfig", _target.CharactersReputationConfigurator,
                    typeof(CharactersReputationConfig), false, GUILayout.Height(20));

                if (_target.CharactersReputationConfigurator != null)
                {
                    _target.ReputationLevelRequired = EditorGUILayout.IntSlider("Reputation Level Required", _target.ReputationLevelRequired,
                                            0, _target.CharactersReputationConfigurator.MaxReputationLevelsCount - 1, GUILayout.Height(20));
                }
                else
                {
                    GUILayout.Label("Не указан файл конфигурации уровней репутации.\nЗаполните поле файла конфигурации уровней репутации", _errorLabelStyle);
                    EditorGUILayout.Space(15f);
                }
            }

            var globalStatesProperty = serializedObject.FindProperty(nameof(_target.GlobalStatesWhenCanStartQuest));

            EditorGUILayout.PropertyField(globalStatesProperty, new GUIContent("Global States When Can Start Quest"), true);
            globalStatesProperty.serializedObject.ApplyModifiedProperties();

            var stagesProperty = _serializedObject.FindProperty(nameof(_target.QuestStages));

            EditorGUILayout.Space(10f);

            stagesProperty.isExpanded = EditorGUILayout.Foldout(stagesProperty.isExpanded, "Quest Stages");

            if (stagesProperty.isExpanded)
            {
                EditorGUILayout.Space(10f);

                for (int i = 0; i < _target.QuestStages.Count; i++)
                {
                    var questStageProperty = stagesProperty.GetArrayElementAtIndex(i);


                    if (i != 0)
                    {
                        EditorGUILayout.Space(10f);
                    }
                    GUILayout.Label($"Stage {i + 1} ({_target.QuestStages[i].StageName})", _stageLabelStyle);
                    EditorGUILayout.Space(5f);
                    EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2f), Color.white);
                    EditorGUILayout.Space(5f);

                    EditorGUI.indentLevel += 1;

                    _target.QuestStages[i].QuestStageType = (QuestStagesTypes)EditorGUILayout.EnumPopup("Stage Type", _target.QuestStages[i].QuestStageType);

                    if (_questStagesTypes[i] != _target.QuestStages[i].QuestStageType)
                    {
                        _target.QuestStages[i].ClearStageRequiredParameters();
                        _questStagesTypes[i] = _target.QuestStages[i].QuestStageType;
                    }

                    _target.QuestStages[i].StageName = EditorGUILayout.TextField("Stage name", _target.QuestStages[i].StageName, GUILayout.Height(20));
                    GUILayout.Label("Stage Discription", _stageDiscriptionLabelStyle);
                    _target.QuestStages[i].StageDescription = EditorGUILayout.TextArea(_target.QuestStages[i].StageDescription, new GUIStyle(EditorStyles.textArea), GUILayout.Height(60));

                    
                    switch (_target.QuestStages[i].QuestStageType)
                    {
                        case QuestStagesTypes.BringResources:

                            var resourcesListProperty = questStageProperty.FindPropertyRelative("RequiredResources");

                            resourcesListProperty.isExpanded = EditorGUILayout.Foldout(resourcesListProperty.isExpanded, "Required Resources");

                            if (resourcesListProperty.isExpanded)
                            {
                                var resourcesList = _target.QuestStages[i].RequiredResources;

                                for (int j = 0; j < resourcesList.Count; j++)
                                {
                                    GUILayout.Label($"Required Resources {j + 1}", _rewardLabelStyle);

                                    EditorGUI.indentLevel += 2;
                                    resourcesList[j].ResourceType = (ResourcesTypes)EditorGUILayout.EnumPopup("Resource Type", resourcesList[j].ResourceType);
                                    resourcesList[j].ResourceCount = EditorGUILayout.IntField("Value", resourcesList[j].ResourceCount, GUILayout.Height(20));
                                    EditorGUI.indentLevel -= 2;
                                }

                                ShowResourcesButtons(i);
                            }

                            break;
                        case QuestStagesTypes.TalkWithNPC:

                            var npcToTalkListProperty = questStageProperty.FindPropertyRelative("RequiredNPCs");
                            npcToTalkListProperty.isExpanded = EditorGUILayout.Foldout(npcToTalkListProperty.isExpanded, "Required NPC to Talk");

                            if(npcToTalkListProperty.isExpanded)
                            {
                                DrowNPCToTalkFields(_target.QuestStages[i].RequiredNPCs, npcToTalkListProperty);
                                ShowRequredNPCButtons(i);
                            }

                            break;
                        case QuestStagesTypes.None:
                        default:
                            break;
                    }

                    var rewardsProperty = questStageProperty.FindPropertyRelative("StageRewards");

                    rewardsProperty.isExpanded = EditorGUILayout.Foldout(rewardsProperty.isExpanded, "Rewards");
                    var stageRevards = _target.QuestStages[i].StageRewards;

                    if (rewardsProperty.isExpanded)
                    {
                        for (int j = 0; j < stageRevards.Count; j++)
                        {
                            GUILayout.Label($"Reward {j + 1}", _rewardLabelStyle);

                            EditorGUI.indentLevel += 2;
                            stageRevards[j].DialogueRewardType = (DialogueRewardTypes)EditorGUILayout.EnumPopup("Reward Type", stageRevards[j].DialogueRewardType);
                            DrowRewardFields(stageRevards, j);
                            EditorGUI.indentLevel -= 2;
                        }

                        ShowRewardsButtons(i);
                    }

                    EditorGUI.indentLevel -= 1;
                }

                EditorGUILayout.Space(10f);
                ShowStageButtons();
            }

            _serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            var saveButton = GUILayout.Button(new GUIContent("Save Changes", "Save"), GUILayout.Width(100f));

            if (saveButton)
            {
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }

        private void ShowStageButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("Add New Stage", "Add"), GUILayout.Width(120));
            var deleteButton = GUILayout.Button(new GUIContent("Delete Last Stage", "Delete"), GUILayout.Width(120));
            var clearButton = GUILayout.Button(new GUIContent("Clear Stage List", "Clear"), GUILayout.Width(120));

            if (addButton)
            {
                _target.AddStage();
                _questStagesTypes.Add(QuestStagesTypes.None);
            }

            if (deleteButton)
            {
                _target.RemoveLastStage();
                _questStagesTypes.RemoveAt(_questStagesTypes.Count - 1);
            }

            if (clearButton)
            {
                _target.ClearAllStages();
                _questStagesTypes.Clear();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrowNPCToTalkFields(List<NPCTalkModel> talkModels, SerializedProperty npcToTalkListProperty)
        {
            for (int i = 0; i < talkModels.Count; i++)
            {
                var npcToTalkElement = npcToTalkListProperty.GetArrayElementAtIndex(i);

                GUILayout.Label($"Required NPC {i + 1}", _rewardLabelStyle);

                EditorGUI.indentLevel += 2;
                talkModels[i].NPCType = (NPCsTypes)EditorGUILayout.EnumPopup("NPC Type", talkModels[i].NPCType);

                var globalStatesListProperty = npcToTalkElement.FindPropertyRelative("StatesWhenCanTalk");

                EditorGUI.indentLevel += 1;
                globalStatesListProperty.isExpanded = EditorGUILayout.Foldout(globalStatesListProperty.isExpanded, "States When Can Talk");
                EditorGUI.indentLevel -= 1;

                if (globalStatesListProperty.isExpanded )
                {
                    var statesList = talkModels[i].StatesWhenCanTalk;

                    for (int j = 0; j < statesList.Count; j++)
                    {
                        EditorGUI.indentLevel += 2;
                        statesList[j] = (GlobalStatesTypes)EditorGUILayout.EnumPopup($"Global State {j}", statesList[j]);
                        EditorGUI.indentLevel -= 2;
                    }

                    ShowGlobalStateButtons(talkModels[i]);
                }
                EditorGUI.indentLevel -= 2;


            }
        }

        private void DrowRewardFields(List<RewardModel> rewardModels, int index)
        {
            switch (rewardModels[index].DialogueRewardType)
            {
                case DialogueRewardTypes.Reputation:
                    rewardModels[index].ReputationRewardTarget = (NPCsTypes)EditorGUILayout.EnumPopup("Reputation Target", rewardModels[index].ReputationRewardTarget);
                    rewardModels[index].RewardCount = EditorGUILayout.IntField("Value", rewardModels[index].RewardCount, GUILayout.Height(20));
                    break;
                case DialogueRewardTypes.WantedLevel:
                    rewardModels[index].RewardCount = EditorGUILayout.IntField("Value", rewardModels[index].RewardCount, GUILayout.Height(20));
                    break;
                case DialogueRewardTypes.Item:
                    rewardModels[index].ResourceType = (ResourcesTypes)EditorGUILayout.EnumPopup("Resource Type", rewardModels[index].ResourceType);
                    rewardModels[index].RewardCount = EditorGUILayout.IntField("Resource count", rewardModels[index].RewardCount, GUILayout.Height(20));
                    break;
                case DialogueRewardTypes.NPCSpecialItem:
                // тут будет инспектор для особых предметов, которых пока нет
                case DialogueRewardTypes.Escape:
                    break;
                default:
                case DialogueRewardTypes.None:
                    break;
            }
        }

        private void ShowGlobalStateButtons(NPCTalkModel nPCTalkModel)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("Add Global State", "Add"), GUILayout.Width(110));
            var deleteButton = GUILayout.Button(new GUIContent("Delete Last State", "Delete"), GUILayout.Width(110));
            var clearButton = GUILayout.Button(new GUIContent("Clear State List", "Clear"), GUILayout.Width(110));

            if (addButton)
            {
                nPCTalkModel.AddState();
            }

            if (deleteButton)
            {
                nPCTalkModel.RemoveLastState();
            }

            if (clearButton)
            {
                nPCTalkModel.ClearAllStates();
            }
            EditorGUILayout.EndHorizontal();
        }


        private void ShowRewardsButtons(int index)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
            var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
            var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

            if (addButton)
            {
                _target.QuestStages[index].AddReward();
            }

            if (deleteButton)
            {
                _target.QuestStages[index].RemoveLastReward();
            }

            if (clearButton)
            {
                _target.QuestStages[index].ClearAllRewards();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowRequredNPCButtons(int index)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
            var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
            var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

            if (addButton)
            {
                _target.QuestStages[index].AddRequiredNPC();
            }

            if (deleteButton)
            {
                _target.QuestStages[index].RemoveLastRequiredNPC();
            }

            if (clearButton)
            {
                _target.QuestStages[index].ClearAllRequiredNPC();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowResourcesButtons(int index)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
            var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
            var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

            if (addButton)
            {
                _target.QuestStages[index].AddRequiredResource();
            }

            if (deleteButton)
            {
                _target.QuestStages[index].RemoveLastRequiredResource();
            }

            if (clearButton)
            {
                _target.QuestStages[index].ClearAllRequiredResource();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}