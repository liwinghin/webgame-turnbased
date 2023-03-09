using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    [Serializable]
    public partial class SkillCombat
    {
        public string name;
        public int damage;
        public int energy;
    }
    public partial class SkillCombat
    {
        public static SkillCombat Decode(JSONObject json)
        {
            var data = new SkillCombat();

            data.name = json.ShouldGetString("name");
            data.damage = json.ShouldGetInt("damage");
            data.energy = json.ShouldGetInt("energy");

            return data;
        }
    }
}