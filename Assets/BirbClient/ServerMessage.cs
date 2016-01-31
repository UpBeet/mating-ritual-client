using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
class ServerMessage
{
    public string action;
    public string data;

    public static ServerMessage CreateFromJSON(string jsonString)
    {
        ServerMessage message = JsonUtility.FromJson<ServerMessage>(jsonString);
        return message;
    }
}
