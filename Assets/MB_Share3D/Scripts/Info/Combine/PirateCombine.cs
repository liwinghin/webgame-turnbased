using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    [Serializable]
    public class PirateCombine : IPartsCoder
    {
        private string[] m_mainPartValues = { };
        private List<string> m_leftHandValues = new List<string>();
        private List<string> m_rightHandValues = new List<string>();

        public string[] partValues { get => this.m_mainPartValues; set => this.m_mainPartValues = value; }

        public PirateCombine()
        {
            List<string> valueList = new List<string>();
            for (int i = 0; i < Enum.GetValues(typeof(PirateValue)).Length; i++)
            {
                valueList.Add(this.defaultValue);
            }
            this.partValues = valueList.ToArray();
        }

        public PirateCombine Clone()
        {
            return new PirateCombine
            {
                partValues = (string[])this.partValues.Clone()
            };
        }

        #region Const prop
        private string defaultValue => PirateConfig.DefaultValue;

        private string tier4Bg => "001";
        private string tier5Bg => "002";
        #endregion

        #region Main part prop
        public string body
        {
            get => this.GetValue(PirateValue.Body); set => this.SetValue(PirateValue.Body, value);
        }
        public string skin
        {
            get => this.GetValue(PirateValue.Skin); set => this.SetValue(PirateValue.Skin, value);
        }
        public string face
        {
            get => this.GetValue(PirateValue.Face); set => this.SetValue(PirateValue.Face, value);
        }
        public string hair
        {
            get => this.GetValue(PirateValue.Hair); set => this.SetValue(PirateValue.Hair, value);
        }
        public string earing
        {
            get => this.GetValue(PirateValue.Earing); set => this.SetValue(PirateValue.Earing, value);
        }
        public string beard
        {
            get => this.GetValue(PirateValue.Beard); set => this.SetValue(PirateValue.Beard, value);
        }
        public string eyewear
        {
            get => this.GetValue(PirateValue.Eyewear); set => this.SetValue(PirateValue.Eyewear, value);
        }
        public string hat
        {
            get => this.GetValue(PirateValue.Hat); set => this.SetValue(PirateValue.Hat, value);
        }
        public string background
        {
            get => this.GetValue(PirateValue.Background); set => this.SetValue(PirateValue.Background, value);
        }
        public string enegry
        {
            get => this.GetValue(PirateValue.Energy); set => this.SetValue(PirateValue.Energy, value);
        }
        public string arua
        {
            get => this.GetValue(PirateValue.Aura); set => this.SetValue(PirateValue.Aura, value);
        }

        public string bodyName => $"{this.body}";
        public string bodySkinName => $"{bodyName}{this.skin}";
        public string faceName => $"{bodyName}{this.face}";
        public string hairName => $"{bodyName}{this.hair}";
        public string earingName => $"{this.earing}";
        public string beardName => $"{this.beard}";
        public string eyewearName => $"{this.eyewear}";
        public string hatName => $"{this.hat}";
        public string productBgName => $"product{this.background}";
        public string detailBgName => $"detail{this.background}";
        public string enegrySkinName => $"{this.enegry}";
        public string auraName => $"{this.arua}";
        #endregion

        #region Hand part prop
        public List<string> leftHands { get => this.m_leftHandValues; set => this.m_leftHandValues = value; }
        public List<string> rightHands { get => this.m_rightHandValues; set => this.m_rightHandValues = value; }
        #endregion

        #region Value handle
        private string GetValue(PirateValue key)
        {
            var index = (int)key;
            return this.partValues[index];
        }

        private void SetValue(PirateValue key, string value)
        {
            var index = (int)key;
            this.partValues[index] = value;
        }

        public bool GetValid(PirateCate cate)
        {
            var tier = cate.GetTierLevel();
            return this.GetValid(tier);
        }

        private bool GetValid(int tierLevel)
        {
            // Tier 1
            foreach (string value in this.partValues)
            {
                if (value == string.Empty) return false;
            }

            // Tier 2
            if (tierLevel >= 1)
            {
                if (this.earing == this.defaultValue) return false;
            }
            else
            {
                if (this.earing != this.defaultValue) return false;
            }

            // Tier 3
            if (tierLevel >= 2)
            {
                if (this.eyewear == this.defaultValue) return false;
                if (this.hat == this.defaultValue) return false;
            }
            else
            {
                if (this.eyewear != this.defaultValue) return false;
                if (this.hat != this.defaultValue) return false;
            }

            // Tier 4
            if (tierLevel >= 3)
            {
                if (this.background == this.defaultValue) return false;
                if (this.enegry == this.defaultValue) return false;
            }
            else
            {
                if (this.background != this.defaultValue) return false;
                if (this.enegry != this.defaultValue) return false;
            }
            if (tierLevel == (int)PirateCate.Normal_4)
            {
                if (this.background != this.tier4Bg) return false;
            }

            // Tier 5
            if (tierLevel >= 4)
            {
                if (this.arua == this.defaultValue) return false;
            }
            else
            {
                if (this.arua != this.defaultValue) return false;
            }
            if (tierLevel == (int)PirateCate.Normal_5)
            {
                if (this.background != this.tier5Bg) return false;
            }

            return true;
        }
        #endregion
    }
}