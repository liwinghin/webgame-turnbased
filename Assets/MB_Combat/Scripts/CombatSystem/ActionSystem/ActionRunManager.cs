using System.Collections.Generic;
using UnityEngine;

namespace MB.Game
{
    public class ActionRunManager : MonoBehaviour
    {
        private static ActionRunManager m_Instance = null;
        public static ActionRunManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<ActionRunManager>();
                return m_Instance;
            }
        }

        [Header("ActionData")]
        public List<ActionDataAsset> actionData = new List<ActionDataAsset>();

        [Header("AI")]
        [Range(0, 100)]
        public int distractChance;

        private bool isAttackSuccessful(CharacterUnit unit, CharacterUnit target)
        {
            int missChance = target.characterAbility.missChance;
            int randomNum = Random.Range(0, 100);
            print("Hit Chance: <" + (100 - missChance) + "%>");
            bool hit = (randomNum > missChance) ? true : false;
            return hit;
        }

        private bool isCriticalAttack(CharacterUnit unit)
        {
            int criticalChance = unit.characterAbility.criticalChance;
            int randomNum = Random.Range(0, 100);
            bool critical = (randomNum < criticalChance) ? true : false;
            return critical;
        }

        public void RandomEnemyAndSkill(CharacterUnit unit, BaseFormationHandler targetHandler, int wave)
        {
            CharacterUnit target = null;
            bool hit = false;
            bool isCritical = false;
            int randomDistract = Random.Range(0, 100);

            while (true)
            {
                if (distractChance > randomDistract)
                {
                    target = targetHandler.GetCurrentLowestHPUnit(wave);
                    //Random Skill
                    bool isAction = RandomSkill(unit, target, ref hit, ref isCritical);
                    if (isAction) break;
                }
                else
                {
                    int randomEnemy = Random.Range(0, targetHandler.teamUnites[wave].charUnites.Count);
                    target = targetHandler.teamUnites[wave].charUnites[randomEnemy];
                    bool isAlive = target.isAlive;
                    if (isAlive == true)
                    {
                        bool isAction = RandomSkill(unit, target, ref hit, ref isCritical);
                        if (isAction) break;
                    }
                }

            }
            unit.DoAction(target, hit, isCritical);
        }

        public bool RandomSkill(CharacterUnit unit, CharacterUnit target, ref bool hit, ref bool isCritical)
        {
            int randomSkill = UnityEngine.Random.Range(0, unit.actionSet.Count);
            unit.SetCurrentAction(randomSkill);
            bool actionAvailable = unit.ActionAvailable();
            if (actionAvailable)
            {
                hit = isAttackSuccessful(unit, target);
                isCritical = isCriticalAttack(unit);
                return true;
            }
            return false;
        }

        public void ShipAttacked(CharacterUnit unit, List<CharacterUnit> targetUnits, Transform vfxPos)
        {
            unit.DoShipAttack(targetUnits, true, false, vfxPos);
        }

        public bool ClickedTarget(CharacterUnit unit, CharacterUnit target)
        {
            bool isAttacked = false;
            //If Action Type is Attack
            if (unit.currentActionType == ActionType.Attack && !IsSameTeam(unit, target))
            {
                bool skillAvailable = unit.ActionAvailable();
                if (skillAvailable)
                {
                    bool hit = isAttackSuccessful(unit, target);
                    bool isCritical = isCriticalAttack(unit);
                    unit.DoAction(target, hit, isCritical);
                    isAttacked = true;
                }
            }
            return isAttacked;
        }

        public bool IsSameTeam(CharacterUnit unit, CharacterUnit target)
        {
            return (unit.transform.root == target.transform.root) ? true : false;
        }

        public int AttackDamage()
        {
            int damage = 0;

            return damage;
        }

        public ActionHandler SetActionHandler(string actionName, int actionValues, int actionEnergy)
        {
            ActionHandler handler;
            handler.actionName = actionName;
            handler.actionData = LoadingAction(actionName);
            handler.actionValues = actionValues;
            handler.actionEnergy = actionEnergy;
            return handler;
        }

        public ActionData LoadingAction(string actionName)
        {
            ActionData action = null;
            foreach (ActionDataAsset data in actionData)
            {
                if (data.DataName().Equals(actionName))
                {
                    action = data.LoadData();
                }
            }
            return action;
        }

        //public int CalActionValue(ActionData data, CharacterAbility ability)
        //{
        //    var resultFomula = data.valueFomula;
        //    string[] sArray = resultFomula.Split(new char[2] { '{', '}' });

        //    for (int i = 0; i < sArray.Length; i++)
        //    {
        //        foreach (FieldInfo info in ability.GetType().GetFields())
        //        {
        //            if (info.Name == sArray[i])
        //            {
        //                string targetStr = '{' + sArray[i] + '}';
        //                string replaceValue = info.GetValue(ability).ToString();
        //                resultFomula = resultFomula.Replace(targetStr, replaceValue);
        //            }
        //        }
        //    }
        //    var result = new DataTable().Compute(resultFomula, string.Empty).ToString();
        //    var resultInt = (int)float.Parse(result);
        //    if (sArray.Length > 2)
        //        return resultInt;
        //    else
        //        return int.Parse(data.valueFomula);
        //}
    }
}
