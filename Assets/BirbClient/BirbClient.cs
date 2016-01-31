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
    public enum BirbMessageCode { CONNECT, CREATE_ROOM, JOIN, PLAYER_JOINED,
        BEGIN, GAME_STATE, SEND_PROMPT, GET_PROMPT, SEND_DANCE, DANCE_RECEVIED,
        PICK_WINNER, SEND_DANCES, ROUND_WINNER, GAME_WINNER, ROOM_KEY, INVALID_CODE };

    /// <summary>
    /// A collection of strings corresponding to birb message codes.
    /// </summary>
    public Dictionary<int, string> MessageStrings = new Dictionary<int, string>() {
            { (int)BirbMessageCode.CONNECT, "CONNECT" },
            { (int)BirbMessageCode.CREATE_ROOM, "CREATE_ROOM" },
            { (int)BirbMessageCode.JOIN, "JOIN" },
            { (int)BirbMessageCode.ROOM_KEY, "ROOM_KEY" },
            { (int)BirbMessageCode.PLAYER_JOINED, "PLAYER_JOINED" },
            { (int)BirbMessageCode.BEGIN, "BEGIN" },
            { (int)BirbMessageCode.GAME_STATE, "GAME_STATE" },
            { (int)BirbMessageCode.SEND_PROMPT, "SEND_PROMPT" },
            { (int)BirbMessageCode.GET_PROMPT, "GET_PROMPT" },
            { (int)BirbMessageCode.SEND_DANCE, "SEND_DANCE" },
            { (int)BirbMessageCode.DANCE_RECEVIED, "DANCE_RECEIVED" },
            { (int)BirbMessageCode.PICK_WINNER, "PICK_WINNER" },
            { (int)BirbMessageCode.SEND_DANCES, "SEND_DANCES" },
            { (int)BirbMessageCode.ROUND_WINNER, "ROUND_WINNER" },
            { (int)BirbMessageCode.GAME_WINNER, "GAME_WINNER" },
            { (int)BirbMessageCode.INVALID_CODE, "INVALID_CODE" },
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
        //72.230.134.30 :5000
        Uri server = new Uri("ws://birb.herokuapp.com");
        Uri braxton = new Uri("ws://72.230.134.30:5000");
        socket = new WebSocket(server);
        yield return StartCoroutine(socket.Connect());
        int i = 0;

        // Testing
        RunUnitTests();

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
        string fullMessage = "{\"action\": \"" + MessageStrings[(int)code] + "\", \"data\": " + JsonUtility.ToJson(data) + "}";

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
        ServerMessage serverMessage = null;
        try {
            serverMessage = ServerMessage.CreateFromJSON(message);
        } catch (ArgumentException e) {
            Debug.LogError("Received invalid birb message:\n" + e);
            return;
        }

        // Parse out the message code
        BirbMessageCode messageCode = BirbMessageCode.INVALID_CODE;
        foreach (BirbMessageCode code in MessageStrings.Keys)
            if (MessageStrings[(int)code] == serverMessage.Action)
                messageCode = code;

        switch (messageCode)
        {
            case (BirbMessageCode.CONNECT):
                Debug.Log("Received " + MessageStrings[(int)messageCode] + " message");
                break;
            case (BirbMessageCode.ROOM_KEY):
                Debug.Log("Received " + MessageStrings[(int)messageCode] + " message with key " + serverMessage.Data);
                break;
            case (BirbMessageCode.PLAYER_JOINED):
                Debug.Log("Received " + MessageStrings[(int)messageCode] + " message with userID " + serverMessage.Data);
                break;
            default:
                break;
        }
    }

    #endregion

    #region Unit Testing

    /// <summary>
    /// Run the unit test suite.
    /// </summary>
    private void RunUnitTests()
    {
        CreateRoom();
    }

    private void CreateRoom()
    {
        SendBirbMessage(BirbMessageCode.CREATE_ROOM, "");
    }

    #endregion

}
