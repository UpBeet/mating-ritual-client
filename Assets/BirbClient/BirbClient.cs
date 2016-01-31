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
    public enum BirbMessageCode { CONNECT, CREATE_ROOM, JOIN_ROOM, JOINED_ROOM,
        BEGIN, GAME_STATE, SEND_PROMPT, GET_PROMPT, SEND_DANCE, DANCE_RECEVIED,
        PICK_WINNER, SEND_DANCES, ROUND_WINNER, GAME_WINNER, ROOM_KEY, START_JUDGING,
        INVALID_CODE };

    #endregion

    #region Private Properties

    /// <summary>
    /// The socket to connect to the birb server.
    /// </summary>
    WebSocket socket;

    /// <summary>
    /// The game state manager.
    /// </summary>
    GameStateManager gameStateManager;

    /// <summary>
    /// A generic callback to use for reponses.
    /// </summary>
    /// <param name="parameters"></param>
    public delegate void Callback(params object[] parameters);

    Callback currentCallback;

    #endregion

    #region Unity Engine Methods

    /// <summary>
    /// Start the Birb Client.
    /// </summary>
    /// <returns>Nothing right now.</returns>
    IEnumerator Start()
    {
        gameStateManager = GetComponent<GameStateManager>();
        Uri server = new Uri("ws://birb.herokuapp.com");
        Uri localhost = new Uri("ws://localhost:5000");
        socket = new WebSocket(localhost);
        yield return StartCoroutine(socket.Connect());
        int i = 0;

        // Testing
        //RunUnitTests();

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
    public void SendBirbMessage(BirbMessageCode code, object data, Callback callback)
    {
        string dataString = "";
        if (data is string && data.ToString() != String.Empty)
        {
            dataString = "\"" + data.ToString() + "\"";
        }
        else
        {
            dataString = JsonUtility.ToJson(data);
        }
        string fullMessage = "{\"action\": \"" + code.ToString() + "\", \"data\": " + dataString + ", \"roomKey\": " + dataString + "}";

        // Debugging
        Debug.Log("Sending birb message: " + fullMessage);
        currentCallback = callback;

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

        // Debugging
        debug("Got action: " + serverMessage.action);
        debug("Got data: " + serverMessage.data);

        // Parse out the message code
        BirbMessageCode messageCode = BirbMessageCode.INVALID_CODE;
        for(int i = 0; i < Enum.GetNames(typeof(BirbMessageCode)).Length; i++)
        {
            BirbMessageCode code = (BirbMessageCode)i;
            if (serverMessage.action == code.ToString())
                messageCode = code;
        }

        // Debugging
        debug("Got message code: " + messageCode.ToString());

        switch (messageCode)
        {
            case (BirbMessageCode.CONNECT):
                Debug.Log("Received " + messageCode.ToString() + " message");
                break;
            case (BirbMessageCode.ROOM_KEY):
                Debug.Log("Received " + messageCode.ToString() + " message with key " + serverMessage.data);
                // Write the JOIN, ROOM_KEY
                DataCache.RoomKey = serverMessage.data;
                if(currentCallback != null)
                {
                    currentCallback.Invoke(DataCache.RoomKey);
                }
                SendBirbMessage(BirbMessageCode.JOIN_ROOM, serverMessage.data, EmptyCallback);
                break;
            case (BirbMessageCode.JOINED_ROOM):
                Debug.Log("Received " + messageCode.ToString() + " message with userID " + serverMessage.data);
                DataCache.PlayerIndex = int.Parse(serverMessage.data);
                if(currentCallback != null)
                {
                    currentCallback.Invoke(DataCache.RoomKey);
                }
                break;
            case (BirbMessageCode.BEGIN):
                Debug.Log("Received " + messageCode.ToString() + " message with judge ID " + serverMessage.data);
                if(currentCallback != null)
                {
                    DataCache.JudgeIndex = int.Parse(serverMessage.data);
                    currentCallback.Invoke(DataCache.JudgeIndex);
                }
                break;
            case (BirbMessageCode.GAME_STATE):
                Debug.Log("Received " + messageCode.ToString() + " message with game state data " + serverMessage.data);
                // TODO: Handle that game state somehow
                break;
            case (BirbMessageCode.GET_PROMPT):
                Debug.Log("Received " + messageCode.ToString() + " message with prompt " + serverMessage.data);
                // TODO : Switch to the gameplay posing screen
                break;
            case (BirbMessageCode.DANCE_RECEVIED):
                Debug.Log("Received " + messageCode.ToString() + " message with boolean " + serverMessage.data);
                // TODO : Display the acknowledgement
                break;
            case (BirbMessageCode.START_JUDGING):
                Debug.Log("Received " + messageCode.ToString() + " message.");
                // TODO : Move the judge to the judging screen and the others
                // to the waiting screen
                break;
            default:
                break;
        }
    }

    private void EmptyCallback(params object[] parameters)
    { }

    #endregion

    #region Unit Testing

    /// <summary>
    /// Run the unit test suite.
    /// </summary>
    private void RunUnitTests()
    {
        CreateAndJoinRoom();
        // Wait until we get a user ID back
        //while(userData.UserId == -1L)
        //{ }
        //BeginGame();
    }

    private void CreateAndJoinRoom()
    {
        SendBirbMessage(BirbMessageCode.CREATE_ROOM, "", EmptyCallback);
    }

    private void BeginGame()
    {
        SendBirbMessage(BirbMessageCode.BEGIN, DataCache.RoomKey, EmptyCallback);
    }

    private void debug(string message)
    {
        Debug.Log(message);
    }

    #endregion
}
