using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class JsonExtension
{
    public static int ShouldGetInt(this JSONObject json, string name)
    {
        var field = json.GetField(name);
        if (field.IsNumber)
        {
            return (int)field.i;
        }
        if (field.IsString)
        {
            return int.Parse(field.str);
        }
        return 0;
    }

    public static float ShouldGetFloat(this JSONObject json, string name)
    {
        var field = json.GetField(name);
        if (field.IsNumber)
        {
            return field.n;
        }
        if (field.IsString)
        {
            return float.Parse(field.str);
        }
        return 0f;
    }

    public static string ShouldGetString(this JSONObject json, string name)
    {
        var field = json.GetField(name);
        if (field.IsString)
        {
            return field.str;
        }
        if (field.IsNumber)
        {
            return field.i.ToString();
        }
        return string.Empty;
    }
}
