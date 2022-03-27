using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using DefaultNamespace;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxHud : MonoBehaviour
{
    [SerializeField] private GameObject _backButton;
    [SerializeField] private ButtonTextHandler _okButton;
    [SerializeField] private Image _characterPortrait;
    [SerializeField] private Text _characterName;
    [SerializeField] public TextMeshProUGUI _sayingText;
    [SerializeField] private Text _titleText;
    [SerializeField] private OptionsHandler _optionsHolder;

    private string _dialogId;
    private EventUi eventUi;

    private float _initialHeight = 170;


    public void Construct(string dialogId, EventUi eventUi)
    {
        _dialogId = dialogId;
        this.eventUi = eventUi;
        _backButton.SetActive(false);
        _okButton.SetText(EButtonText.OKAY);
    }

    //coming form button
    public void OkayPressed()
    {
        // DialogStartNode dialogStartNode = (DialogStartNode) eventUi.GetNodeForId(_dialogId);
        //
        // if (dialogStartNode.dialogStartType == DialogStartType.BattleEvent)
        // {
        //     if (PlayerService.Instance.playerArmy.Get<Army>().battle.HasValue)
        //     {
        //         BattleTickDelaySystem.Instance.DelayEntity(PlayerService.Instance.playerArmy.Get<Army>().battle.Value,
        //             2000);
        //
        //         Debug.Log("Started delay on battle of player");
        //     }
        // }
        // eventUi.OkayPressed(_dialogId);
    }

    //coming form button
    public void BackPressed()
    {
        eventUi.BackPressed(_dialogId);
    }

    public void SetData(BaseDialogNode dialogNode)
    {
        Debug.Log("zashel");
        
        
        ResetMessageBox();
        if (dialogNode == null || dialogNode.GetID == null)
            DialogComplete();
        else if (dialogNode is DialogStartNode)
            SetAsDialogStartNode((DialogStartNode) dialogNode);
        else if (dialogNode is DialogNode)
            SetAsDialogNode((DialogNode) dialogNode);
        else if (dialogNode is CardNode)
            SetAsMultiOptionsNode((CardNode) dialogNode);
        else if (dialogNode is DialogActionNode)
        {
            Debug.Log("firing event");
            DialogActionNode actionNode = (DialogActionNode) dialogNode;
            actionNode.onChangeEvent?.Invoke();
        }
        else
            Debug.LogError("Wrong Dialog type Sent Here");

        if (dialogNode == null)
        {
            return;
        }
        if (dialogNode.GetID == "")
        {
            Debug.Log("check check nothatormy");
            // QuestManager.Instance.AddQuestToActive(null);
        }
        
        if (dialogNode != null && dialogNode.DialogLine.StartsWith("{dontShow}"))
        {
            Debug.Log("not showing because dontShow specified");
            gameObject.SetActive(false);
        }
    }

    private void ResetMessageBox()
    {
        _optionsHolder.ClearList();
    }

    private void DialogComplete()
    {
        eventUi.DialogComplete(_dialogId);
        gameObject.SetActive(false);
    }

    private void SetAsDialogNode(DialogNode dialogNode)
    {
        _backButton.SetActive(dialogNode.IsBackAvailable());
        _okButton.ShowButton(true);
        _okButton.SetText(dialogNode.IsNextAvailable() ? EButtonText.NEXT : EButtonText.OKAY);

        _characterPortrait.sprite = dialogNode.CharacterPotrait;
        _characterName.text = dialogNode.CharacterName;
        _sayingText.text = dialogNode.DialogLine;
    }

    private void SetAsDialogStartNode(DialogStartNode dialogStartNode)
    {
        _backButton.SetActive(dialogStartNode.IsBackAvailable());
        _okButton.ShowButton(true);
        _okButton.SetText(dialogStartNode.IsNextAvailable() ? EButtonText.NEXT : EButtonText.OKAY);

        _characterPortrait.sprite = dialogStartNode.CharacterPotrait;
        _characterName.text = dialogStartNode.CharacterName;
        _sayingText.text = dialogStartNode.DialogLine;
    }


    private void SetAsMultiOptionsNode(CardNode dialogNode)
    {
        _backButton.SetActive(dialogNode.IsBackAvailable());
        _okButton.ShowButton(false);

        _characterPortrait.sprite = dialogNode.CharacterPotrait;
        _characterName.text = dialogNode.CharacterName;
        // var text = dialogNode.DialogLine;
        // string b = Regex.Replace(text, "{.+}", match =>
        // {
        //     Debug.Log(match.Value);
        //     return $"Replaced with ${match.Value.Split(',')[1]}";
        // });
        // _sayingText.text = b;
        //
        // if (!string.IsNullOrEmpty(dialogNode.ItemsToAdd))
        // {
        //     Debug.Log("Add or remove this items " + dialogNode.ItemsToAdd);
        // }

        foreach (DialogAction action in dialogNode.actionsOnEnter)
        {
            Debug.Log("Invoking for " + dialogNode.DialogLine);
            action.Invoke("", 1);
        }


        _optionsHolder.CreateOptions(dialogNode._options, OptionSelected);
        GrowMessageBox(dialogNode.GetAllOptions().Count);
    }

    private void GrowMessageBox(int count)
    {
//		Vector2 size = GetComponent<RectTransform>().sizeDelta;
//		size.y += (count * _optionsHolder.CellHeight());
//		GetComponent<RectTransform>().sizeDelta = size;
    }

    private void OptionSelected(int optionSelected)
    {
        Debug.Log("Dialog node option selected " + optionSelected);
        eventUi.OptionSelected(_dialogId, optionSelected);
    }
}