using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace MB.Game
{
    public class UnitComparer : IComparer<CharacterUnit>
    {
        public int Compare(CharacterUnit a, CharacterUnit b)
        {
            if(a.characterAbility.turnSpeed != b.characterAbility.turnSpeed)
            {
                return b.characterAbility.turnSpeed.CompareTo(a.characterAbility.turnSpeed);
            }
            else 
            {
                return randomUnit(a.characterAbility.luck, b.characterAbility.luck);
            }
        }
        public int randomUnit(int a, int b)
        {
            int chance = a + b;
            int randomNum = UnityEngine.Random.Range(0, chance);

            if (randomNum >= b)
            {
                return 1;
            }
            else
                return -1;
        }
    }

    [Serializable]
    public class TeamUnit
    {
        public List<CharacterUnit> charUnites;
    }

    public class BaseFormationHandler : MonoBehaviour
    {
        public string teamType = string.Empty;
        public Transform[] containers;
        public List<TeamUnit> teamUnites = new List<TeamUnit>();
        public ShipCombat shipInfo;
        public BaseFormationHandler targetFormationHandler;

        public void AddCharacterTeam(int charCount)
        {
            var team = new TeamUnit();
            team.charUnites = new List<CharacterUnit>(new CharacterUnit[charCount]);
            teamUnites.Add(team);
        }

        public void SetCharacter(int teamIndex, int charIndex, Transform character, CharacterCombat data)
        {
            var container = containers[charIndex];
            var charUnit = character.transform.GetComponent<CharacterUnit>();

            SetCharacterPoint(character, container);
            SetCharacterData(charUnit, data);
            teamUnites[teamIndex].charUnites[charIndex] = charUnit;
        }

        private void SetCharacterPoint(Transform character, Transform point)
        {
            character.SetParent(point);
            character.localPosition = Vector3.zero;
            character.localRotation = Quaternion.identity;
        }

        private void SetCharacterData(CharacterUnit unit, CharacterCombat data)
        {
            CharRuntime animator = unit.transform.GetComponent<CharRuntime>();
            unit.mineFormation = this;
            unit.ApplyInitValue(teamType, data, animator);
        }

        public void ClearCharacters()
        {
            foreach (var container in containers)
            {
                SceneObjectUtils.DestroyChildren(container);
            }
            teamUnites.Clear();
        }

        public bool IsUnitStillAlive(int wave)
        {
            for (int i = 0; i < teamUnites[wave].charUnites.Count; i++)
            {
                if (teamUnites[wave].charUnites[i].isAlive) { return true; }
            }
            return false;
        }

        public void CheckCharacterPosition(int wave)
        {
            for (int i = 0; i < teamUnites[wave].charUnites.Count; i++)
            {
                if (teamUnites[wave].charUnites[i].isAlive)
                {
                    if (teamUnites[wave].charUnites[i].transform.localPosition != Vector3.zero)
                    {
                        teamUnites[wave].charUnites[i].transform.localPosition = Vector3.zero;
                        teamUnites[wave].charUnites[i].transform.localEulerAngles = Vector3.zero;
                    }
                }
                else
                {
                    if(teamUnites[wave].charUnites[i].charAnimator.loopAction != CharLoopMotion.Die)
                    {
                        teamUnites[wave].charUnites[i].SetCharacterLoopAnimation(CharLoopMotion.Die);
                    }
                }
            }
        }

        public void CheckCharacterAlive(int wave)
        {
            for (var i = 0; i < teamUnites.Count; i++)
            {
                for (var j = 0; j < teamUnites[i].charUnites.Count; j++)
                {
                    teamUnites[i].charUnites[j].SetActive(teamUnites[i].charUnites[j].isAlive);
                }
            }
        }

        public void SetWaveEnemyActive(int wave)
        {
            for(var i = 0; i < teamUnites.Count; i++)
            {
                for(var j = 0; j < teamUnites[i].charUnites.Count; j++)
                {
                    teamUnites[i].charUnites[j].SetActive(i == wave);
                }
            }
        }

        public void SetCharacterEnergy(int wave)
        {
            for (int i = 0; i < teamUnites[wave].charUnites.Count; i++)
            {
                if (teamUnites[wave].charUnites[i].characterAbility.currentHp > 0 && teamUnites[wave].charUnites[i].isActiveAndEnabled)
                    teamUnites[wave].charUnites[i].EnergyRecovery();
            }
        }

        public void SetCharacterAnimation(int wave, CharLoopMotion motion)
        {
            for (int i = 0; i < teamUnites[wave].charUnites.Count; i++)
            {
                if (teamUnites[wave].charUnites[i].isActiveAndEnabled)
                    teamUnites[wave].charUnites[i].SetCharacterLoopAnimation(motion);
            }
        }

        public CharacterUnit GetCurrentLowestHPUnit(int wave)
        {
            int tempHP = 10000;
            CharacterUnit unit = null;

            for (int i = 0; i < teamUnites[wave].charUnites.Count; i++)
            {
                if(teamUnites[wave].charUnites[i].isAlive && teamUnites[wave].charUnites[i].characterAbility.currentHp < tempHP)
                {
                    tempHP = teamUnites[wave].charUnites[i].characterAbility.currentHp;
                    unit = teamUnites[wave].charUnites[i];
                }
            }
            return unit;
        }

        public List<CharacterUnit> GetCurrentUnits(int wave)
        {
            List<CharacterUnit> characters = new List<CharacterUnit>();
            for (int i = 0; i < teamUnites[wave].charUnites.Count; i++)
            {
                if (teamUnites[wave].charUnites[i].isAlive)
                {
                    characters.Add(teamUnites[wave].charUnites[i]);
                }
            }
            return characters;
        }
    }
}
