using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Client;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{
    private static TradeUI instance;
    public static TradeUI Instance => instance;

    public SlotUI[] slotUis;
    public SlotUI hullSlotUi;
    public SlotUI foodSlotUi;

    public event Action<SkillsComponent, int> onBoughtSkills;
    public event Action<PointsComponent, int> onBoughtPoints;

    public Button okButton;

    private IEnumerable<SlotUI> slots;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);

        slots = slotUis.Union(new List<SlotUI> {foodSlotUi, hullSlotUi});
    }

    public void ShowCard(Trade trade)
    {
        Recalculate(trade);
        foreach (SlotUI slotUI in slots)
        {
            slotUI.gameObject.SetActive(true);
        }
    }

    private void Recalculate(Trade trade)
    {
        foreach (SlotUI slotUI in slots)
        {
            slotUI.button.onClick.RemoveAllListeners();
            slotUI.button.onClick.AddListener(delegate { Recalculate(trade); });
        }

        SetSlotData(slotUis[0], trade.skillsComponents[0], trade.skillComponentsCosts[0]);
        SetSlotData(slotUis[1], trade.skillsComponents[1], trade.skillComponentsCosts[1]);
        SetSlotData(slotUis[2], trade.skillsComponents[2], trade.skillComponentsCosts[2]);
        SetSlotData(foodSlotUi, trade.pointsComponents[0], trade.pointsComponentsCosts[0]);
        SetSlotData(hullSlotUi, trade.pointsComponents[1], trade.pointsComponentsCosts[1]);

        foreach (SlotUI slotUI in slots)
        {
            slotUI.button.onClick.AddListener(delegate { Recalculate(trade); });
        }
    }

    private void SetSlotData(SlotUI slotUi, SkillsComponent skillsComponent, int cost)
    {
        CheckIfNotEnoughMoney(slotUi, cost);

        slotUi.text.text = skillsComponent.ToString();
        slotUi.costText.text = cost.ToString();
        slotUi.button.onClick.AddListener(delegate { onBoughtSkills?.Invoke(skillsComponent, cost); });
        slotUi.button.onClick.AddListener(delegate { slotUi.gameObject.SetActive(false); });
    }

    private void SetSlotData(SlotUI slotUi, PointsComponent pointsComponent, int cost)
    {
        CheckIfNotEnoughMoney(slotUi, cost);

        slotUi.button.onClick.AddListener(delegate { onBoughtPoints?.Invoke(pointsComponent, cost); });
        slotUi.text.text = pointsComponent.ToString();
        slotUi.costText.text = cost.ToString();
    }


    private void CheckIfNotEnoughMoney(SlotUI slotUi, int cost)
    {
        if (!PointsSystem.EnoughPointsCost(new PointsComponent {money = cost}))
        {
            slotUi.button.interactable = false;
        }
    }
}