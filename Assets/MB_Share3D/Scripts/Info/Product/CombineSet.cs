using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    [Serializable]
    public class CombineSet
    {
        public string valueName;
        public int total;
        public int numPad;

        private string format => new string('0', this.numPad);

        public List<string> GetEnableValues(string header = "")
        {
            var fmt = this.format;
            var result = new List<string>();

            for (int i = 0; i < this.total; i++)
            {
                result.Add(header + i.ToString(fmt));
            }
            return result;
        }
    }
}