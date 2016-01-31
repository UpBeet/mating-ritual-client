using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class BirbUserData
{
    /// <summary>
    /// The user's unique ID inside of the room.
    /// </summary>
    private long userId;
    public long UserId { get; set; }

    /// <summary>
    /// The room code for the room that the user is in.
    /// </summary>
    private string roomCode;
    public string RoomKey { get; set; }

    public BirbUserData()
    {
        UserId = -1;
        RoomKey = "";
    }
}
