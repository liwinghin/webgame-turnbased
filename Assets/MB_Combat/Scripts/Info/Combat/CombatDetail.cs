using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    [Serializable]
    public partial class CombatDetail
    {
        public string id;
        public CombatType combatType;
        public string stage;
    }
    public partial class CombatDetail
    {
        public static CombatDetail Decode(JSONObject json)
        {
            var data = new CombatDetail();

            data.id = json.ShouldGetString("id");
            data.combatType = CombatTypeParser.FromCode(json.ShouldGetString("combatType"));
            data.stage = json.ShouldGetString("stage");

            return data;
        }
    }
}