using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    [Serializable]
    public class ShipCombine : IPartsCoder
    {
        private string[] m_partValues = { };

        public string[] partValues { get => this.m_partValues; set => this.m_partValues = value; }

        public ShipCombine()
        {
            List<string> valueList = new List<string>();
            for (int i = 0; i < Enum.GetValues(typeof(ShipValue)).Length; i++)
            {
                valueList.Add(this.defaultValue);
            }
            this.partValues = valueList.ToArray();
        }

        public ShipCombine Clone()
        {
            return new ShipCombine
            {
                partValues = (string[])this.partValues.Clone()
            };
        }

        #region Const prop
        private string defaultValue => ShipConfig.DefaultValue;

        public string normal1Body => ShipConfig.GetCateBody(ShipCate.Normal_1);
        public string normal2Body => ShipConfig.GetCateBody(ShipCate.Normal_2);
        public string cursed1Body => ShipConfig.GetCateBody(ShipCate.Cursed_1);
        public string cursed2Body => ShipConfig.GetCateBody(ShipCate.Cursed_2);

        public string normal1Background => ShipConfig.GetCateBackground(ShipCate.Normal_1);
        public string normal2Background => ShipConfig.GetCateBackground(ShipCate.Normal_2);
        public string cursed1Background => ShipConfig.GetCateBackground(ShipCate.Cursed_1);
        public string cursed2Background => ShipConfig.GetCateBackground(ShipCate.Cursed_2);
        #endregion

        #region Value prop
        public string body
        {
            get => this.GetValue(ShipValue.Body); set => this.SetValue(ShipValue.Body, value);
        }
        public string skin
        {
            get => this.GetValue(ShipValue.Skin); set => this.SetValue(ShipValue.Skin, value);
        }
        public string sail
        {
            get => this.GetValue(ShipValue.Sail); set => this.SetValue(ShipValue.Sail, value);
        }
        public string background
        {
            get => this.GetValue(ShipValue.Background); set => this.SetValue(ShipValue.Background, value);
        }

        public string bodyName => $"{this.body}";
        public string bodySkinName => $"{bodyName}{this.skin}";
        public string sailSkinName => $"{bodyName}{this.sail}";
        public string productBgName => $"product{this.background}";
        public string detailBgName => $"detail{this.background}";
        #endregion

        #region Value handle
        public bool GetValid(ShipCate cate)
        {
            foreach (string value in this.partValues)
            {
                if (value == string.Empty) return false;
            }

            // Body
            if (this.body != ShipConfig.GetCateBody(cate))
            {
                return false;
            }

            // Background
            if (this.background != ShipConfig.GetCateBackground(cate))
            {
                return false;
            }

            return true;
        }

        private string GetValue(ShipValue key)
        {
            var index = (int)key;
            return this.partValues[index];
        }

        private void SetValue(ShipValue key, string value)
        {
            var index = (int)key;
            this.partValues[index] = value;
        }
        #endregion
    }
}