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
                result += "+🪖fighting: " + fighting;
                result += " ";
            }

            if (science != 0)
            {
                result += "+🔬science: " + science;
                result += " ";
            }

            if (mechanical != 0)
            {
                result += "+🔧mechanical: " + mechanical;
                result += " ";
            }

            if (survival != 0)
            {
                result += "+🔥survival: " + survival;
                result += " ";
            }

            if (charisma != 0)
            {
                result += "+💬charisma: " + charisma;
                result += " ";
            }

            return result;
        }
    }

    [Serializable]
    public struct AddToPlayer
    {
    }
}