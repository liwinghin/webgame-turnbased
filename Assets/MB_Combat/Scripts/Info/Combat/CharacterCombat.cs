using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    [Serializable]
    public partial class CharacterCombat
    {
        public string name;
        public int tier;
        public string partsCode;
        public string image;
        public CharacterType characterType;
        public int hp;
        public int energy;
        public int turnSpeed;
        public int energyRecover;
        public int normalDamage;
        public int missChance;
        public int criticalChance;
        public int luck;
        public List<SkillCombat> specials;
    }
    public partial class CharacterCombat
    {
        public static CharacterCombat Decode(JSONObject json)
        {
            var data = new CharacterCombat();
            var subJson = new JSONObject();

            data.name = json.ShouldGetString("name");
            data.tier = json.ShouldGetInt("tier");
            data.partsCode = json.ShouldGetString("partsCode");
            data.image = json.ShouldGetString("image");
            data.image = data.image.Replace("img1024", "img350");
            data.characterType = CharacterTypeParser.FromCode(json.ShouldGetString("charType"));

            data.hp = json.ShouldGetInt("hp");
            data.energy = json.ShouldGetInt("energy");
            data.turnSpeed = json.ShouldGetInt("turnSpeed");
            data.energyRecover = json.ShouldGetInt("energyRecover");
            data.normalDamage = json.ShouldGetInt("normalDamage");
            data.missChance = json.ShouldGetInt("missChance");
            data.criticalChance = json.ShouldGetInt("criticalChance");
            data.luck = json.ShouldGetInt("luck");

            subJson = json.GetField("specials");
            data.specials = new List<SkillCombat>();
            if (subJson != null && subJson.IsArray)
            {
                foreach (var item in subJson.list)
                {
                    data.specials.Add(SkillCombat.Decode(item));
                }
            }

            return data;
        }
    }
}
