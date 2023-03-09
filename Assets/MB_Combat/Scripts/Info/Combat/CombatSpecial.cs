using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    [Serializable]
    public partial class CombatSpecial
    {
        public string name;
    }
    public partial class CombatSpecial
    {
        public static CombatSpecial Decode(string raw)
        {
            var json = new JSONObject(raw);
            return Decode(json);

        }
        public static CombatSpecial Decode(JSONObject json)
        {
            var data = new CombatSpecial();

            data.name = json.ShouldGetString("name");

            return data;
        }
    }
}