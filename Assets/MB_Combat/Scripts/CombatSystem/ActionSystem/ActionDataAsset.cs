using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MB.Game
{
    public enum ActionType { Attack, Heal, None }
    public enum TargetType { SingleAttack, SingleHeal, GroupAttack, GroupHeal, None }
    public enum AnimationType { Sword, Gun, None }

    [Serializable]
    public class ActionData 
    {
        //Data
        [Header("Ability")]
        [SerializeField] private string m_actionName = string.Empty;
        [SerializeField] private ActionType m_actionType = ActionType.None;
        [SerializeField] private TargetType m_targetType = TargetType.None;
        [Header("Art")]
        [SerializeField] private AnimationType m_animationType = AnimationType.None;
        [SerializeField] private CharOnceMotion m_actionAnimation = CharOnceMotion.none;
        [SerializeField] private AudioType m_audioType = AudioType.none;
        [SerializeField] private Sprite m_actionIcon = null;
        [SerializeField] private string m_AttackVFX = null;
        [SerializeField] private string m_gotDamagedVFX = null;

        //Data
        public string actionName { get => m_actionName; set => m_actionName = value; }
        public ActionType actionType { get => m_actionType; set => m_actionType = value; }
        public TargetType targetType { get => m_targetType; set => m_targetType = value; }
        public Sprite actionIcon { get => m_actionIcon; set => m_actionIcon = value; }
        public AnimationType animationType { get => m_animationType; set => m_animationType = value; }
        public CharOnceMotion actionAnimation { get =>  m_actionAnimation; set =>  m_actionAnimation = value; }
        public AudioType audioType { get => m_audioType; set => m_audioType = value; }
        public string attackVFX { get => m_AttackVFX; set => m_AttackVFX = value; }
        public string gotDamagedVFX { get => m_gotDamagedVFX; set => m_gotDamagedVFX = value; }
    }

    [CreateAssetMenu(fileName = "New Action Data", menuName = "Action Data/Create Action Data Asset", order = 1)]
    public class ActionDataAsset : ScriptableObject
    {
        //public ActionData[] actionData;
        public ActionData actionData;

        public string DataName()
        {
            return actionData.actionName;
        }

        public ActionData LoadData()
        {
            return actionData;
        }
    }
}
