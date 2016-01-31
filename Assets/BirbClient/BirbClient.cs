using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BirbClient : MonoBehaviour {

    #region Data Properties

    /// <summary>
    /// An enumeration of all of the message codes that can be sent
    /// to, or received from, the birb server.
    /// </summary>
    public enum BirbMessageCode { CONNECT, CREATE_ROOM, JOIN, ENTER_ROOM, JOINED,
        BEGIN, GAME_STATE, SEND_PROMPT, GET_PROMPT, SEND_DANCE, DANCE_RECEVIED,
        PICK_WINNER, SEND_DANCES, ROUND_WINNER, GAME_WINNER, INVALID_CODE };

    /// <summary>
    /// A collection of strings corresponding to birb message codes.
    /// </summary>
    public Dictionary<int, string> MessageStrings = new Dictionary<int, string>() {
            { (int)BirbMessageCode.CONNECT, "CONNECT" },
            { (int)BirbMessageCode.CREATE_ROOM, "CREATE_ROOM" },
            { (int)BirbMessageCode.JOIN, "JOIN" }
    };

    #endregion

    #region Private Properties

    /// <summary>
    /// The socket to connect to the birb server.
    /// </summary>
    WebSocket socket;

    #endregion

    #region Unity Engine Methods

    /// <summary>
    /// Start the Birb Client.
    /// </summary>
    /// <returns>Nothing right now.</returns>
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
        string fullMessage = "\"Action:\" \"" + MessageStrings[(int)code] + "\", \"Data\": " + JsonUtility.ToJson(data);

        // Debugging
        Debug.Log("Sending birb message: " + fullMessage);

        socket.SendString(fullMessage);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Process a message coming in from the server.
    /// </summary>
    /// <param name="message">JSON string parameter coming in</param>
    private void Process(string message)
    {
        try {
            ServerMessage serverMessage = ServerMessage.CreateFromJSON(message);
        } catch (ArgumentException e) {
            Debug.LogError("Received invalid birb message:\n" + e);
            return;
        }

        // Parse out the message code
        BirbMessageCode messageCode = BirbMessageCode.INVALID_CODE;
        foreach (BirbMessageCode code in MessageStrings.Keys)
            if (MessageStrings[(int)code] == message)
                messageCode = code;

        switch (messageCode)
        {
            case (BirbMessageCode.CONNECT):
                Debug.Log("Received CONNECT message");
                break;
            default:
                break;
        }
    }

    #endregion
}
