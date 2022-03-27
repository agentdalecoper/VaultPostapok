using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;


public enum QuestStatus
{
    NotStarted,
    Started,
    Finished
}


public class EventUi : MonoBehaviour
{
    private Dictionary<string, DialogNodeCanvas> _dialogIdTracker;
    public Dictionary<DialogStartNode, DialogNodeCanvas> dialogStartsMap;

    public Dictionary<DialogStartNode, QuestStatus> quests = new Dictionary<DialogStartNode, QuestStatus>();

    [SerializeField] public GameObject messageBox;
    private Dictionary<string, MessageBoxHud> _messageBoxes;

    public DialogNodeCanvas dialogCanvas;

    private static EventUi instance;
    public static EventUi Instance => instance;

    public void Awake()
    {
        instance = this;

        _messageBoxes = new Dictionary<string, MessageBoxHud>();
        _dialogIdTracker = new Dictionary<string, DialogNodeCanvas>();
        dialogStartsMap = new Dictionary<DialogStartNode, DialogNodeCanvas>();


        if (dialogCanvas)
        {
            foreach (string id in dialogCanvas.GetAllDialogId())
            {
                _dialogIdTracker.Add(id, dialogCanvas);
            }

            foreach (DialogStartNode id in dialogCanvas.GetAllDialogStarts())
            {
                dialogStartsMap.Add(id, dialogCanvas);

                // слушай это полнеший g-code
                // я бы сделал это через ECS мб сразуу
                if (id.dialogStartType == DialogStartType.Quest)
                {
                    quests.Add(id, QuestStatus.NotStarted);
                }
            }
        }
        else
        {
            foreach (DialogNodeCanvas nodeCanvas in Resources.LoadAll<DialogNodeCanvas>("Saves/"))
            {
                foreach (string id in nodeCanvas.GetAllDialogId())
                {
                    _dialogIdTracker.Add(id, nodeCanvas);
                }

                foreach (var id in dialogCanvas.GetAllDialogStarts())
                {
                    dialogStartsMap.Add(id, dialogCanvas);
                }
            }
        }
    }

    // мб через рефлекшен сделать подстановку полей
    public void ShowDialogWithId(string dialogIdToLoad, bool goBackToBeginning = true, params object[] objects)
    {
        Debug.Log("Showind dialog with id " + dialogIdToLoad);
        // if (_messageBoxes.ContainsKey(dialogIdToLoad))
        // {
        //     return;
        // }
        
        DialogNodeCanvas nodeCanvas;
        if (_dialogIdTracker.TryGetValue(dialogIdToLoad, out nodeCanvas))
        {
            nodeCanvas.ActivateDialog(dialogIdToLoad, goBackToBeginning);
        }
        else
            Debug.LogError("ShowDialogWithId Not found Dialog with ID : " + dialogIdToLoad);
        
        MessageBoxHud messageBox = this.messageBox.GetComponent<MessageBoxHud>();
        this.messageBox.gameObject.SetActive(true);
        messageBox.Construct(dialogIdToLoad, this);
//        messageBox.transform.SetParent(transform.parent, false);

        DialogStartNode dialogStartNode = GetNodeForId(dialogIdToLoad) as DialogStartNode;
        messageBox.SetData(dialogStartNode);
        _messageBoxes.Add(dialogIdToLoad, messageBox);

        messageBox._sayingText.text = SubstituteText(dialogStartNode.DialogLine, objects);

        // Time.timeScale = 0;
    }
    
    
    /*
     * так ну а как можно динамическа засабситьютить филды в стрингу?
     * 
     */
    public string SubstituteText(string text, params object[] objects)
    {
        Debug.Log("Substituting text");
        Dictionary<string, object> dictionaryToSubstitute = new Dictionary<string, object>();
        foreach (object o in objects)
        {
            Debug.Log("Substituting text for " + o);
            foreach (var fieldInfo in o.GetType().GetFields())
            {
                string name = o.GetType() + "." + fieldInfo.Name;
                dictionaryToSubstitute[name] = fieldInfo.GetValue(o);
                dictionaryToSubstitute[fieldInfo.Name] = fieldInfo.GetValue(o);
                
                Debug.Log("name is " + name + " and " + fieldInfo.Name);
            }
        }

        foreach (KeyValuePair<string, object> kv in dictionaryToSubstitute)
        {
            Debug.Log("DictToSubst: " + kv.Key + " " + kv.Value);
        }

        string res = Regex.Replace(text, "{.+}", match =>
        {
            Debug.Log("value in replacenent regex " + match.Value);
            string matchValue = match.Value.Substring(1, match.Value.Length - 2);
            string toSubstitute = matchValue;

            if (dictionaryToSubstitute.ContainsKey(matchValue))
            {
                toSubstitute = dictionaryToSubstitute[matchValue].ToString();
            }

            Debug.Log($"Replaced with matchValue " + toSubstitute);

            return toSubstitute;
        });

        return res;
    }

    public BaseDialogNode GetNodeForId(string dialogIdToLoad)
    {
        DialogNodeCanvas nodeCanvas;
        if (_dialogIdTracker.TryGetValue(dialogIdToLoad, out nodeCanvas))
        {
            return nodeCanvas.GetDialog(dialogIdToLoad);
        }
        else
            Debug.LogError("getNodeForId Not found Dialog with ID : " + dialogIdToLoad);

        return null;
    }

    private void GiveInputToDialog(string dialogIdToLoad, int inputValue)
    {
        DialogNodeCanvas nodeCanvas;
        if (_dialogIdTracker.TryGetValue(dialogIdToLoad, out nodeCanvas))
        {
            nodeCanvas.InputToDialog(dialogIdToLoad, inputValue);
        }
        else
            Debug.LogError("GiveInputToDialog Not found Dialog with ID : " + dialogIdToLoad);
    }

    public void OkayPressed(string dialogId)
    {
        GiveInputToDialog(dialogId, (int) EDialogInputValue.Next);
        _messageBoxes[dialogId].SetData(GetNodeForId(dialogId));
    }

    public void BackPressed(string dialogId)
    {
        GiveInputToDialog(dialogId, (int) EDialogInputValue.Back);
        _messageBoxes[dialogId].SetData(GetNodeForId(dialogId));
    }

    public void DialogComplete(string dialogId)
    {
        _messageBoxes.Remove(dialogId);
        // Time.timeScale = 1;
    }

    public void OptionSelected(string dialogId, int optionSelected)
    {
        GiveInputToDialog(dialogId, optionSelected);
        _messageBoxes[dialogId].SetData(GetNodeForId(dialogId));
    }
}

public enum EDialogInputValue
{
    Next = -2,
    Back = -1,
}