using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    [Serializable]
    public partial class CombatOption
    {
        public float bgmVolume;
        public float sfxVolume;
    }
    public partial class CombatOption
    {
        public static CombatOption Decode(JSONObject json)
        {
            var data = new CombatOption();

            data.bgmVolume = json.ShouldGetFloat("bgmVolume");
            data.sfxVolume = json.ShouldGetFloat("sfxVolume");

            return data;
        }
    }
}
