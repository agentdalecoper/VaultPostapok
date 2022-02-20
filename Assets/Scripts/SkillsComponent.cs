using System;
using System.Drawing;

namespace Client
{
    [Serializable]
    public struct SkillsComponent
    {
        public int fighting;
        public int science;
        public int mechanical;
        public int survival;
        public int charisma;


        public override string ToString()
        {
            string result = "";
            if (fighting != 0)
            {
                result += " +🪖fighting: " + fighting;
            }

            if (science != 0)
            {
                result += " +🔬science: " + science;
            }

            if (mechanical != 0)
            {
                result += " +🔧mechanical: " + mechanical;
            }

            if (survival != 0)
            {
                result += " +🔥survival: " + survival;
            }

            if (charisma != 0)
            {
                result += " +💬charisma: " + charisma;
            }

            return result;
        }
    }

    [Serializable]
    public struct AddToPlayer
    {
    }
}