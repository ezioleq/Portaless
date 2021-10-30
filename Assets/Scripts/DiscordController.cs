using Discord;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour {
	private static DiscordController _instance;
	public static DiscordController Instance { get { return _instance; } }

	[SerializeField] private long _clientId;
	[SerializeField] private string _largeImage;
	[SerializeField] private string _largeText;
	[SerializeField] private string[] _randomStates;

	private Discord.Discord _discord;
	private Discord.ActivityManager _activityManager;
	private ActivityTimestamps _activityTimestamp;

	private ActivityAssets _activityAsset;

	private void Awake() {
		// Singleton
		if (_instance != null && _instance != this)
			Destroy(this.gameObject);
		else
			_instance = this;

		// Create new Discord instance
		_discord = new Discord.Discord(_clientId, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
		_activityManager = _discord.GetActivityManager();

		// Set default activity assets (this big picture on Discord's RPC)
		_activityAsset = new ActivityAssets {
			LargeImage = _largeImage,
			LargeText = _largeText
		};

		// Set start timestamp
		_activityTimestamp = new ActivityTimestamps {
			Start = System.DateTimeOffset.Now.ToUnixTimeSeconds()
		};

		// First activity
		var activity = new Activity {
			Assets = _activityAsset,
			Timestamps = _activityTimestamp
		};

		// Set first activity
		_activityManager.UpdateActivity(activity, (res) => {
			if (res == Discord.Result.Ok)
				Debug.Log("Initialized Discord RPC");
		});

		// Listen to current scene changes
		SceneManager.activeSceneChanged += OnSceneChange;
	}

	private void Update() {
		_discord.RunCallbacks();
	}

	private void OnSceneChange(Scene current, Scene next) {
		var activity = new Activity {
			Details = next.name,
			// Get random state, just for variety
			State = _randomStates[Random.Range(0, _randomStates.Length-1)],
			Assets = _activityAsset,
			Timestamps = _activityTimestamp
		};

		_activityManager.UpdateActivity(activity, (res) => {
			if (res == Discord.Result.Ok)
				Debug.Log("Scene changed, RPC Updated");
		});
	}

	private void OnApplicationQuit() {
		_discord.Dispose();
	}
}
