using UnityEngine;
using UnityEditor;
using Locations;
using NPCCharacters;
using System.Collections.Generic;
using System;
using UnityEditor.AnimatedValues;
using MacFsWatcher;
using Quests;
using Configs.Enum;

namespace Dialogues
{
    [CustomEditor(typeof(DialogueGenerator))]

    public class DialogueGeneratorEditor : Editor
    {
        private DialogueGenerator _target;
        private GUIStyle _nodeLabelStyle;
        private GUIStyle _answerLabelStyle;
        private GUIStyle _errorLabelStyle;
        private GUIStyle _rewardLabelStyle;
        private SerializedObject _serializedObject;
        private bool _isDialogueShow;
        private List<BooleanShowParameters> _booleanParameters = new List<BooleanShowParameters>();

        private void OnEnable()
        {
            _target = (DialogueGenerator)target;
            _serializedObject = new SerializedObject(_target);

            _nodeLabelStyle = new GUIStyle();
            _nodeLabelStyle.fixedHeight = 10f;
            _nodeLabelStyle.fixedWidth = 200f;
            _nodeLabelStyle.font = EditorStyles.boldFont;
            _nodeLabelStyle.normal.textColor = Color.white;

            _answerLabelStyle = new GUIStyle();
            _answerLabelStyle.fixedHeight = 10f;
            _answerLabelStyle.fixedWidth = 200f;
            _answerLabelStyle.font = EditorStyles.boldFont;
            _answerLabelStyle.normal.textColor = Color.white;
            _answerLabelStyle.margin = new RectOffset(33, 0, 0, 0);

            _rewardLabelStyle = new GUIStyle();
            _rewardLabelStyle.fixedHeight = 10f;
            _rewardLabelStyle.fixedWidth = 200f;
            _rewardLabelStyle.normal.textColor = Color.white;
            _rewardLabelStyle.margin = new RectOffset(33, 0, 10, 5);

            _errorLabelStyle = new GUIStyle();
            _errorLabelStyle.fixedHeight = 10f;
            _errorLabelStyle.fixedWidth = 200f;
            _errorLabelStyle.font = EditorStyles.boldFont;
            _errorLabelStyle.normal.textColor = Color.red;

            for (int i = 0; i < _target.DialogueNodes.Count; i++)
            {
                var indexes = new BooleanShowParameters();
                indexes.IsAnswersShow = true;
                indexes.IsNodeShow = true;
                indexes.FillList(_target.DialogueNodes[i].PlayerAnswers.Count);

                _booleanParameters.Add(indexes);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _serializedObject.Update();

            if(_target.DialogueType != DialogueTypes.None)
            {


                switch (_target.DialogueType)
                {
                    case DialogueTypes.StartQuestDialogue:
                        _target.QuestConfigurator = EditorGUILayout.ObjectField("Quest Configurator", _target.QuestConfigurator, typeof(QuestConfigurator), false) as QuestConfigurator;
                        break;
                    case DialogueTypes.QuestDialogue:
                        _target.QuestConfigurator = EditorGUILayout.ObjectField("Quest Configurator", _target.QuestConfigurator, typeof(QuestConfigurator), false) as QuestConfigurator;

                        if (_target.QuestConfigurator != null)
                        {
                            var stageIndexArray = new int[_target.QuestConfigurator.QuestStages.Count];
                            var stageNamesArray = new string[_target.QuestConfigurator.QuestStages.Count];

                            for (int i = 0; i < _target.QuestConfigurator.QuestStages.Count; i++)
                            {
                                stageIndexArray[i] = i;
                                stageNamesArray[i] = _target.QuestConfigurator.QuestStages[i].StageName;
                            }

                            _target.DialogueForQuestStage = EditorGUILayout.IntPopup("Stage", _target.DialogueForQuestStage, stageNamesArray, stageIndexArray, GUILayout.Height(20));
                        }
                        break;
                    case DialogueTypes.EventDialogue:
                        // конфигуратор эвентов
                        _target.IsReputationRequired = EditorGUILayout.Toggle("Is Reputation Level Required?", _target.IsReputationRequired);
                        ShowReputationConfigField();
                        break;
                    case DialogueTypes.DialoigueWithReward:

                        _target.IsReputationRequired = EditorGUILayout.Toggle("Is Reputation Level Required?", _target.IsReputationRequired);
                        ShowReputationConfigField();
                        var rewardsProperty = _serializedObject.FindProperty(nameof(_target.Rewards));

                        rewardsProperty.isExpanded = EditorGUILayout.Foldout(rewardsProperty.isExpanded, "Rewards");

                        if (rewardsProperty.isExpanded)
                        {
                            for (int i = 0; i < _target.Rewards.Count; i++)
                            {
                                GUILayout.Label($"Reward {i}", _rewardLabelStyle);

                                EditorGUI.indentLevel += 2;
                                _target.Rewards[i].DialogueRewardType = (DialogueRewardTypes)EditorGUILayout.EnumPopup("Reward Type", _target.Rewards[i].DialogueRewardType);
                                DrowRewardFields(_target.Rewards[i].DialogueRewardType, i);
                                EditorGUI.indentLevel -= 2;
                            }

                            ShowRewardsButtons();
                        }

                        _serializedObject.ApplyModifiedProperties();
                        break;

                    case DialogueTypes.StandartDialogue:
                        _target.IsReputationRequired = EditorGUILayout.Toggle("Is Reputation Level Required?", _target.IsReputationRequired);
                        ShowReputationConfigField();
                        break;

                    default:
                    case DialogueTypes.None:
                        break;
                }

                EditorGUILayout.Space();

                _isDialogueShow = EditorGUILayout.Foldout(_isDialogueShow, "Dialogue Nodes");
                EditorGUILayout.Space(10f);

                if (_isDialogueShow)
                {
                    for (int i = 0; i < _target.DialogueNodes.Count; i++)
                    {
                        if (i != 0)
                        {
                            EditorGUILayout.Space(10f);
                        }
                        GUILayout.Label("Node " + _target.DialogueNodes[i].NodeID, _nodeLabelStyle);
                        EditorGUILayout.Space(5f);
                        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 2f), Color.white);
                        EditorGUILayout.Space(5f);

                        EditorGUI.indentLevel += 1;
                        _booleanParameters[i].IsNodeShow = EditorGUILayout.Foldout(_booleanParameters[i].IsNodeShow, _target.DialogueNodes[i].NPCText);

                        if (_booleanParameters[i].IsNodeShow)
                        {
                            EditorGUI.indentLevel += 1;
                            _target.DialogueNodes[i].NPCText = EditorGUILayout.TextField("NPC Text", _target.DialogueNodes[i].NPCText, GUILayout.Height(20));

                            _booleanParameters[i].IsAnswersShow = EditorGUILayout.Foldout(_booleanParameters[i].IsAnswersShow, "Answers");

                            if (_booleanParameters[i].IsAnswersShow)
                            {
                                EditorGUILayout.Space(5f);
                                var answers = _target.DialogueNodes[i].PlayerAnswers;

                                EditorGUI.indentLevel += 1;

                                for (int j = 0; j < answers.Count; j++)
                                {
                                    _booleanParameters[i].IsAnswerParametersShow[j] = EditorGUILayout.Foldout(_booleanParameters[i].IsAnswerParametersShow[j], answers[j].Text);

                                    if (_booleanParameters[i].IsAnswerParametersShow[j])
                                    {
                                        EditorGUILayout.Space(10f);
                                        answers[j].Text = EditorGUILayout.TextField("Answer Text", answers[j].Text, GUILayout.Height(20));

                                        if (!answers[j].IsExit)
                                        {
                                            if (answers[j].ToNode == -1)
                                            {
                                                answers[j].ToNode = 0;
                                            }
                                            answers[j].ToNode = EditorGUILayout.IntField("To Node", answers[j].ToNode, GUILayout.Height(20));
                                        }
                                        else
                                        {
                                            EditorGUI.BeginDisabledGroup(true);
                                            answers[j].ToNode = -1;
                                            answers[j].ToNode = EditorGUILayout.IntField("To Node", answers[j].ToNode, GUILayout.Height(20));
                                            EditorGUI.EndDisabledGroup();
                                        }
                                        answers[j].IsExit = EditorGUILayout.Toggle("Is Exit", answers[j].IsExit);

                                        if (answers[j].IsExit)
                                        {
                                            switch (_target.DialogueType)
                                            {
                                                case DialogueTypes.StartQuestDialogue:
                                                case DialogueTypes.DialoigueWithReward:
                                                case DialogueTypes.EventDialogue:
                                                case DialogueTypes.QuestDialogue:
                                                    answers[j].IsPositiveExitAnswer = EditorGUILayout.Toggle("Is Rewarded Exit Answer", answers[j].IsPositiveExitAnswer);
                                                    break;
                                                case DialogueTypes.None:
                                                case DialogueTypes.StandartDialogue:
                                                default:
                                                    break;
                                            }
                                        }

                                        EditorGUILayout.Space(10f);
                                    }
                                }
                                EditorGUI.indentLevel -= 1;
                                EditorGUILayout.Space(10f);
                                ShowAnswersButtons(i);
                            }
                            EditorGUI.indentLevel -= 1;
                        }

                        EditorGUI.indentLevel -= 1;
                    }
                    EditorGUILayout.Space(10f);
                    ShowDialogueNodesButtons();
                }

                if (GUILayout.Button("Generate Dialogue JSON File"))
                {
                    if (_target.LocalizationType == LocalizationTypes.None)
                    {
                        Debug.Log("Файл не был создан. Укажите локализацию");
                        return;
                    }

                    if (_target.LocationType == LocationsTypes.None)
                    {
                        Debug.Log("Файл не был создан. Укажите локацию");
                        return;
                    }

                    if (_target.CharacterType == NPCsTypes.None)
                    {
                        Debug.Log("Файл не был создан. Укажите персонажа");
                        return;
                    }

                    if ((_target.DialogueType == DialogueTypes.QuestDialogue || _target.DialogueType == DialogueTypes.StartQuestDialogue) &&
                        _target.QuestConfigurator == null)
                    {
                        Debug.Log("Квестовый диалог. Укажите конфигуратор связанного квеста");
                        return;
                    }

                    if (_target.DialogueType == DialogueTypes.EventDialogue && _target.EventConfigurator == null)
                    {
                        Debug.Log("Евентовый диалог. Укажите конфигуратор связанного евента");
                        return;
                    }

                    _target.Generate();
                }
            }
        }

        private void ShowReputationConfigField()
        {
            if (_target.IsReputationRequired)
            {
                if (_target.CharactersReputationConfigurator != null)
                {
                    _target.ReputationLevelRequired = EditorGUILayout.IntSlider("Reputation Level Required", _target.ReputationLevelRequired,
                                            0, _target.CharactersReputationConfigurator.MaxReputationLevelsCount, GUILayout.Height(20));
                }
                else
                {
                    GUILayout.Label("Не указан файл конфигурации уровней репутации.\nЗаполните поле файла конфигурации уровней репутации", _errorLabelStyle);
                    EditorGUILayout.Space(15f);
                }
            }
        }

        private void DrowRewardFields(DialogueRewardTypes dialogueRewardType, int index)
        {
            switch (_target.Rewards[index].DialogueRewardType)
            {
                case DialogueRewardTypes.Reputation:
                    _target.Rewards[index].ReputationRewardTarget = (NPCsTypes)EditorGUILayout.EnumPopup("Reputation Target", _target.Rewards[index].ReputationRewardTarget);
                    _target.Rewards[index].RewardCount = EditorGUILayout.IntField("Value", _target.Rewards[index].RewardCount, GUILayout.Height(20));
                    break;
                case DialogueRewardTypes.WantedLevel:
                    _target.Rewards[index].RewardCount = EditorGUILayout.IntField("Value", _target.Rewards[index].RewardCount, GUILayout.Height(20));
                    break;
                case DialogueRewardTypes.Item:
                    _target.Rewards[index].ResourceType = (ResourcesTypes)EditorGUILayout.EnumPopup("Resource Type", _target.Rewards[index].ResourceType);
                    _target.Rewards[index].RewardCount = EditorGUILayout.IntField("Resource count", _target.Rewards[index].RewardCount, GUILayout.Height(20));
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

        private void ShowAnswersButtons(int dialogueIndex)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
            var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
            var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

            if (addButton)
            {
                _target.DialogueNodes[dialogueIndex].AddAnswer();
                _booleanParameters[dialogueIndex].IsAnswerParametersShow.Add(true);
            }

            if (deleteButton)
            {
                _target.DialogueNodes[dialogueIndex].RemoveLastAnswer();
                _booleanParameters[dialogueIndex].IsAnswerParametersShow.RemoveAt(_booleanParameters[dialogueIndex].IsAnswerParametersShow.Count - 1);
            }

            if (clearButton)
            {
                _target.DialogueNodes[dialogueIndex].ClearAllAnswers();
                _booleanParameters[dialogueIndex].IsAnswerParametersShow.Clear();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ShowRewardsButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("+", "Add"), EditorStyles.miniButtonLeft, GUILayout.Width(20));
            var deleteButton = GUILayout.Button(new GUIContent("-", "Delete"), EditorStyles.miniButtonMid, GUILayout.Width(20));
            var clearButton = GUILayout.Button(new GUIContent("clear", "Clear"), EditorStyles.miniButtonRight, GUILayout.Width(40));

            if (addButton)
            {
                _target.AddReward();
            }

            if (deleteButton)
            {
                _target.RemoveLastReward();
            }

            if (clearButton)
            {
                _target.ClearAllRewards();
            }
            EditorGUILayout.EndHorizontal();
        }


        private void ShowDialogueNodesButtons()
        {
            GUILayout.Space(5f);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var addButton = GUILayout.Button(new GUIContent("Add New Node", "Add"), GUILayout.Width(120));
            var deleteButton = GUILayout.Button(new GUIContent("Delete Last Node", "Delete"), GUILayout.Width(120));
            var clearButton = GUILayout.Button(new GUIContent("Clear Node List", "Clear"), GUILayout.Width(120));

            if (addButton)
            {
                _target.AddNode();

                var indexes = new BooleanShowParameters();
                indexes.IsAnswersShow = true;
                indexes.IsNodeShow = true;
                _booleanParameters.Add(indexes);
            }

            if (deleteButton)
            {
                _target.RemoveLastNode();
                _booleanParameters.RemoveAt(_booleanParameters.Count - 1);
            }

            if (clearButton)
            {
                _target.ClearAllNodes();
                _booleanParameters.Clear();
            }
            EditorGUILayout.EndHorizontal();
        }

        private class BooleanShowParameters
        {
            public bool IsAnswersShow;
            public bool IsNodeShow;
            public List<bool> IsAnswerParametersShow = new List<bool>();

            public void FillList(int answersCount)
            {
                for (int i = 0; i < answersCount; i++)
                {
                    IsAnswerParametersShow.Add(false);
                }
            }
        }
    }
}