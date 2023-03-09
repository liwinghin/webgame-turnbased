using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    [Serializable]
    public partial class CombatStart
    {
        public CombatDetail combatDetail;
        public CombatOption option;
        public ShipCombat ship;
        public List<CharacterCombat> teammates;
        public List<WaveCombat> waves;
    }
    public partial class CombatStart
    {
        public static CombatStart Decode(string raw)
        {
            var json = new JSONObject(raw);
            return Decode(json);

        }
        public static CombatStart Decode(JSONObject json)
        {
            var data = new CombatStart();
            var subJson = new JSONObject();

            subJson = json.GetField("combat");
            if (subJson != null)
            {
                data.combatDetail = CombatDetail.Decode(subJson);
            }

            subJson = json.GetField("option");
            if (subJson != null)
            {
                data.option = CombatOption.Decode(subJson);
            }

            subJson = json.GetField("ship");
            if (subJson != null)
            {
                data.ship = ShipCombat.Decode(subJson);
            }

            subJson = json.GetField("teammates");
            if (subJson != null && subJson.IsArray)
            {
                data.teammates = new List<CharacterCombat>();
                foreach (var item in subJson.list)
                {
                    data.teammates.Add(CharacterCombat.Decode(item));
                }
            }

            subJson = json.GetField("waves");
            if (subJson != null && subJson.IsArray)
            {
                data.waves = new List<WaveCombat>();
                foreach (var item in subJson.list)
                {
                    data.waves.Add(WaveCombat.Decode(item));
                }
            }

            return data;
        }
    }
}
