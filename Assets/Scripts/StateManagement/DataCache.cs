using UnityEngine;

// Cached game data.
public static class DataCache {

	// Active room key.
	public static string RoomKey { get; set; }

	// If set to true, this user is the game host.
	public static bool IsHost { get; set; }

	// This player's index.
	public static int PlayerIndex { get; set; }

	// Player index of the current judge.
	public static int JudgeIndex { get; set; }

	// Current scores.
	public static int[] Scores { get; set; }

	// Initialize the data cache.
	static DataCache () {
		RoomKey = string.Empty;
		IsHost = false;
		PlayerIndex = 0;
		JudgeIndex = -1;
		Scores = new int[8];
		for (int i = 0; i < Scores.Length; i++) {
			Scores [i] = -1;
		}
	}

	// Checks if the player is the current judge.
	public static bool PlayerIsJudge () {
		return PlayerIndex == JudgeIndex;
	}
}
