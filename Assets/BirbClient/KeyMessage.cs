using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
class KeyMessage : ServerMessage
{
    public string roomKey;

    public static KeyMessage CreateFromJSON(string jsonString)
    {
        KeyMessage message = JsonUtility.FromJson<KeyMessage>(jsonString);
        return message;
    }
}