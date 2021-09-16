using System;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour {
	public Discord.Discord discord;

	void Awake() {
		DontDestroyOnLoad(this);
		if (FindObjectsOfType(GetType()).Length > 1)
			Destroy(this);

		discord = new Discord.Discord(855876148675346442, (UInt64)Discord.CreateFlags.Default);
		var activityManager = discord.GetActivityManager();
		var activity = new Activity {
			State = "Doing tests",
			Details = "Chamber 01",
			Assets = new ActivityAssets {
				LargeImage = "icon",
				LargeText = "I'm drowning, help"
			},
			Timestamps = new ActivityTimestamps {
				Start = DateTimeOffset.Now.ToUnixTimeSeconds()
			}
		};
		activityManager.UpdateActivity(activity, (res) => {
			if (res == Discord.Result.Ok)
				Debug.Log("Discord RPC OK");
		});
	}

	void Update() {
		discord.RunCallbacks();
	}

	private void OnApplicationQuit() {
		discord.Dispose();
	}
}
