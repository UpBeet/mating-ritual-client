using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
class ServerMessage
{
    public string Action;
    public string Data;

    public static ServerMessage CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ServerMessage>(jsonString);
    }
}
