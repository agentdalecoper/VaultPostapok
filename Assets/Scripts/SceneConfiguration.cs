using UnityEngine;

namespace Client
{
    [CreateAssetMenu]
    class SceneConfiguration : ScriptableObject
    {
        public PointsComponent startupPoints;
        public SkillsComponent startSkills;
        public PointsComponent hungerEndOfDayPoints;

        public Day[] days;
    }
}