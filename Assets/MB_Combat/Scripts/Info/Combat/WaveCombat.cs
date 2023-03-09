using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    [Serializable]
    public partial class WaveCombat
    {
        public List<CharacterCombat> enemies;
    }
    public partial class WaveCombat
    {
        public static WaveCombat Decode(JSONObject json)
        {
            var data = new WaveCombat();
            var subJson = new JSONObject();

            subJson = json.GetField("enemies");
            if(subJson != null && subJson.IsArray)
            {
                data.enemies = new List<CharacterCombat>();
                foreach (var item in subJson.list)
                {
                    data.enemies.Add(CharacterCombat.Decode(item));
                }
            }

            return data;
        }
    }
}
