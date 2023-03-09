using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB.Game
{
    public interface IGameCaller
    {

    }
    public static class GameCallerExtension
    {
        public static CombatContainer GetCombatContainer(this IGameCaller caller) => CombatContainer.Instance;
    }
}