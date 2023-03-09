using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public enum ResourceType
    {
        none,
        RedGem,
        YellowGem,
        GreenGem,
        BlueGem
    }
    public struct ResourceTypeCode
    {
        public const string RedGem = "RED";
        public const string YellowGem = "YELLOW";
        public const string GreenGem = "GREEN";
        public const string BlueGem = "BLUE";
    }
    public struct ResourceTypeField
    {
        public const string RedGem = "red";
        public const string YellowGem = "yellow";
        public const string GreenGem = "green";
        public const string BlueGem = "blue";
    }
    public class ResourceTypeParser
    {
        public static ResourceType[] GetAllTypes()
        {
            return new ResourceType[] {
                ResourceType.RedGem,
                ResourceType.YellowGem,
                ResourceType.GreenGem,
                ResourceType.BlueGem
            };
        }

        public static string[] GetAllFields()
        {
            return new string[] {
                ResourceTypeField.RedGem,
                ResourceTypeField.YellowGem,
                ResourceTypeField.GreenGem,
                ResourceTypeField.BlueGem
            };
        }

        public static string ToFieldName(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.RedGem: return ResourceTypeField.RedGem;
                case ResourceType.YellowGem: return ResourceTypeField.YellowGem;
                case ResourceType.GreenGem: return ResourceTypeField.GreenGem;
                case ResourceType.BlueGem: return ResourceTypeField.BlueGem;
            }
            return string.Empty;
        }

        public static ResourceType FromCode(string code)
        {
            switch (code)
            {
                case ResourceTypeCode.RedGem: return ResourceType.RedGem;
                case ResourceTypeCode.YellowGem: return ResourceType.YellowGem;
                case ResourceTypeCode.GreenGem: return ResourceType.GreenGem;
                case ResourceTypeCode.BlueGem: return ResourceType.BlueGem;
            }
            return ResourceType.none;
        }

        public static string ToCode(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.RedGem: return ResourceTypeCode.RedGem;
                case ResourceType.YellowGem: return ResourceTypeCode.YellowGem;
                case ResourceType.GreenGem: return ResourceTypeCode.GreenGem;
                case ResourceType.BlueGem: return ResourceTypeCode.BlueGem;
            }
            return string.Empty;
        }
    }
}
