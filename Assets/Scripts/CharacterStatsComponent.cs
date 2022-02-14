using System;
using System.Drawing;

namespace Client
{
    [Serializable]
    public struct CharacterStatsComponent
    {
        public int fighting;
        public int learning;
        public int stewardship;

        public override string ToString()
        {
            return $"{nameof(fighting)}: {fighting}, " +
                   $"{nameof(learning)}: {learning}, {nameof(stewardship)}: {stewardship}";
        }
    }

    [Serializable]
    public struct ToHireCharacterComponent
    {
        public PointsComponent cost;
    }
}