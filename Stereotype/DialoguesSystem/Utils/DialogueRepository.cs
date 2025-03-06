using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Dialogues
{
    public sealed class DialogueRepository
    {
        private IJsonData<DialogueJSONModel> _jsonData;

        public DialogueRepository()
        {
            _jsonData = new JsonData<DialogueJSONModel>();
        }

        public void Save(string path, string fileName, DialogueJSONModel dialogueSaveModel)
        {
            if (!Directory.Exists(Path.Combine(path)))
            {
                Directory.CreateDirectory(path);
            }

            _jsonData.Save(dialogueSaveModel, Path.Combine(path, fileName));
            Debug.Log("Save");
        }

        public List<Dialogue> Load(string path)
        {
            var dialogues = new List<Dialogue>();
            DirectoryInfo dir = new DirectoryInfo(path);

            if(dir.Exists)
            {
                var files = dir.GetFiles("*.txt");

                for (int i = 0; i < files.Length; i++)
                {
                    var dialogueModel = _jsonData.Load(files[i].FullName);

                    var dialogue = new Dialogue(dialogueModel, i);
                    dialogues.Add(dialogue);
                }
            }

            return dialogues;
        }
    }
}