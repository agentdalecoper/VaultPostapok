using TMPro;
using UnityEngine;

public class SkillsUI : MonoBehaviour
{
    public TextMeshProUGUI fighting;
    public TextMeshProUGUI science;
    public TextMeshProUGUI mechanical;
    public TextMeshProUGUI survival;
    public TextMeshProUGUI charisma;

    public void Awake()
    {
        SkillsSystem.onSkillsChanged += (newSkills, playerResultSkills) =>
        {
            fighting.text = playerResultSkills.fighting.ToString();
            science.text = playerResultSkills.science.ToString();
            mechanical.text = playerResultSkills.mechanical.ToString();
            survival.text = playerResultSkills.survival.ToString();
            charisma.text = playerResultSkills.charisma.ToString();
        };
    }
}