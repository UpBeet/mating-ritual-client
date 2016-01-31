using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BirbClient : MonoBehaviour {

    #region Data Properties

    public enum BirbMessageCode { CONNECT };

    public Dictionary<int, string> MessageStrings = new Dictionary<int, string>() {
            { (int)BirbMessageCode.CONNECT, "CONNECT" }
    };

    #endregion

    #region Private Properties

    /// <summary>
    /// The socket to connect to the birb server.
    /// </summary>
    WebSocket socket;

    #endregion

    #region Unity Engine Methods

    // Use this for initialization
    IEnumerator Start()
    {
        socket = new WebSocket(new Uri("ws://birb.herokuapp.com"));
        yield return StartCoroutine(socket.Connect());
        SendBirbMessage(BirbMessageCode.CONNECT, "");
        int i = 0;
        while (true)
        {
            string reply = socket.RecvString();
            if (reply != null)
            {
                Debug.Log("Received: " + reply);
                Process(reply);
            }
            if (socket.Error != null)
            {
                Debug.LogError("Error: " + socket.Error);
                break;
            }
            yield return 0;
        }
        socket.Close();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Send a message to the birb server.
    /// </summary>
    /// <param name="code">The code of the message to send through.</param>
    /// <param name="data">The data to send through.</param>
    public void SendBirbMessage(BirbMessageCode code, object data)
    {
        // TODO: Maybe the curlys around the data block will cause an issue...
        socket.SendString("\"Action:\" \"" + MessageStrings[(int)code] + "\", \"Data\": {\"" + JsonUtility.ToJson(data) + "\"}");
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Process a message coming in from the server.
    /// </summary>
    /// <param name="message">JSON string parameter coming in</param>
    private void Process(string message)
    {
        ServerMessage serverMessage = ServerMessage.CreateFromJSON(message);

    }

    #endregion
}
