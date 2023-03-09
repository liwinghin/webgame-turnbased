using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MB.Game
{
    public class UIResultPanel : MonoBehaviour
    {
        [SerializeField] private Button m_backBtn = null;

        public UnityEvent onBackClick => this.m_backBtn.onClick;
    }
}