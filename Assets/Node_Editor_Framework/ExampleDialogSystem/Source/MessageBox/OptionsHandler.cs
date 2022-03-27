using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    [SerializeField] private GameObject _optionPrefab;

    private List<OptionsButtonHandler> _buttonHandlers = new List<OptionsButtonHandler>();
    private Action<int> _callback;

    public void CreateOptions(List<DialogOption> allOptions, Action<int> callBack)
    {
        _callback = callBack;
        for (int x = 0; x < allOptions.Count; x++)
        {
            DialogOption dialogOption = allOptions[x];
            if (ConditionFalse(dialogOption)) continue;

            OptionsButtonHandler buttonHandler = Instantiate(_optionPrefab).GetComponent<OptionsButtonHandler>();
            buttonHandler.transform.SetParent(transform, false);

            string text = dialogOption.text;
            string[] splits = text.Split('#');
            string t = splits[0];

            // bool activeButton = true;
            // if (splits.Length > 1)
            // {
            //     for (int i = 1; i < splits.Length; i++)
            //     {
            //         string split = splits[i];
            //         Debug.Log("Split " + split);
            //         string[] item = split.Split(',');
            //         
            //         string id = item[0];
            //         string value = item[1];
            //
            //         if (id.StartsWith("item"))
            //         {
            //             Debug.Log("It is an item!! and  " + id + "=" + value);
            //             if (!PlayerService.Instance.itemsMap.ContainsKey(id) || PlayerService.Instance.itemsMap[id] < int.Parse(value))
            //             {
            //                 Debug.Log("Not enough of the item" + value + item);
            //                 activeButton = false;
            //             }
            //         }
            //     }
            // }


            buttonHandler.SetText(t);
            
            Debug.Log("set value and button callback " + x);
            buttonHandler.SetValueAndButtonCallBack(x, ButtonCallBack);
            // buttonHandler._button.interactable = activeButton;
            _buttonHandlers.Add(buttonHandler);
        }
    }

    private static bool ConditionFalse(DialogOption dialogOption)
    {
        foreach (Condition condition in dialogOption.conditions)
        {
            if (!condition.Invoke(1))
            {
                Debug.Log("Option checks are not for, not drawing " + dialogOption.text);
                return true;
            }
        }

        return false;
    }

    void ButtonCallBack(int value)
    {
        _callback(value);
    }

    public float CellHeight()
    {
        return GetComponent<GridLayoutGroup>().cellSize.y;
    }

    public void ClearList()
    {
        foreach (OptionsButtonHandler handler in _buttonHandlers)
        {
            Destroy(handler.gameObject);
        }

        _buttonHandlers.Clear();
    }
}