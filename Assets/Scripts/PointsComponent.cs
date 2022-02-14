using System;

namespace Client
{
    [Serializable]
    public struct PointsComponent
    {
        public int money;
        public int food;
        public int hull;

        public override string ToString()
        {
            string result = "";
            if (money != 0)
            {
                result += " Money: " + money;
            }

            if (food != 0)
            {
                result += " Food: " + food;
            }

            if (hull != 0)
            {
                result += " Hull: " + hull;
            }

            return result;
        }
    }
}