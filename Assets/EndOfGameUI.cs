using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfGameUI : MonoBehaviour
{
    private static EndOfGameUI instance;
    public static EndOfGameUI Instance => instance;

    public TextMeshProUGUI endOfDayText;
    public Button button;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void SetData(EndOfGame endOfGame)
    {
        if (endOfGame.win)
        {
            endOfDayText.color = Color.green;
            endOfDayText.text = $"End of the game. You won!";
        }
        else
        {
            endOfDayText.color = Color.red;
            endOfDayText.text = $"End of game. You lost!";
        }
    }
}