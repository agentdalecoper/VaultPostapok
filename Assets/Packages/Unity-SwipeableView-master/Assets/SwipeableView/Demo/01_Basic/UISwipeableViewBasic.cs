using System.Collections.Generic;
using TMPro;

namespace SwipeableView
{
    public class UISwipeableViewBasic : UISwipeableView<BasicCardData>
    {
        private static UISwipeableViewBasic instance;
        public static UISwipeableViewBasic Instance => instance;

        public TextMeshProUGUI diceText;
        public DiceView diceView;

        public void Awake()
        {
            instance = this;
        }

        public void UpdateData(List<BasicCardData> data)
        {
            Initialize(data);
        }
    }
}