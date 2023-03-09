using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public class CamRuntime : MonoBehaviour
    {
        [Header("Parts")]
        [SerializeField] private CamBackground m_nowBackground = null;

        public CamBackground nowBackground
        {
            get => this.m_nowBackground; set => this.m_nowBackground = value;
        }
    }
}