using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    [Serializable]
    public partial class ShipCombat
    {
        public string name;
        public int tier;
        public string partsCode;
        public string image;
        public int cannonDamage;
        public int cannonTurn;
    }
    public partial class ShipCombat
    {
        public static ShipCombat Decode(string raw)
        {
            var json = new JSONObject(raw);
            return Decode(json);

        }
        public static ShipCombat Decode(JSONObject json)
        {
            var data = new ShipCombat();

            data.name = json.ShouldGetString("name");
            data.tier = json.ShouldGetInt("tier");
            data.partsCode = json.ShouldGetString("partsCode");
            data.image = json.ShouldGetString("image");
            data.cannonDamage = json.ShouldGetInt("cannonDamage");
            data.cannonTurn = json.ShouldGetInt("cannonTurn");

            return data;
        }
    }
}