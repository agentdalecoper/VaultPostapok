using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SwipeableView
{
    public class BasicScene : MonoBehaviour
    {
        [SerializeField]
        private UISwipeableViewBasic swipeableView = default;

        [SerializeField]
        private Sprite[] sprites;

        void Start()
        {
            var randomSprites = sprites.OrderBy(x => Random.Range(float.MinValue, float.MaxValue)).ToList();
            List<BasicCardData> data = Enumerable.Range(0, 40)
                .Select(i => new BasicCardData
                {
                    color = new Color(1f, 1f, 1f, 1.0f)
                    , sprite = randomSprites[i % sprites.Length]
                })
                .ToList();
        }

        public void OnClickLike()
        {
            if (swipeableView.IsAutoSwiping) return;
            swipeableView.AutoSwipe(SwipeDirection.Right);
        }

        public void OnClickNope()
        {
            if (swipeableView.IsAutoSwiping) return;
            swipeableView.AutoSwipe(SwipeDirection.Left);
        }
    }
}