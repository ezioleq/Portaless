using Discord;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Portaless
{
	public class DiscordController : MonoBehaviour {
		private static DiscordController instance;
		public static DiscordController Instance { get { return instance; } }

		[SerializeField] private long clientId;
		[SerializeField] private string largeImage;
		[SerializeField] private string largeText;
		[SerializeField] private string[] randomStates;

		private Discord.Discord discord;
		private Discord.ActivityManager activityManager;
		private ActivityTimestamps activityTimestamp;

		private ActivityAssets activityAsset;

		private void Awake() {
			// Singleton
			if (instance != null && instance != this)
				Destroy(this.gameObject);
			else
				instance = this;

			// Create new Discord instance
			discord = new Discord.Discord(clientId, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
			activityManager = discord.GetActivityManager();

			// Set default activity assets (this big picture on Discord's RPC)
			activityAsset = new ActivityAssets {
				LargeImage = largeImage,
				LargeText = largeText
			};

			// Set start timestamp
			activityTimestamp = new ActivityTimestamps {
				Start = System.DateTimeOffset.Now.ToUnixTimeSeconds()
			};

			// First activity
			var activity = new Activity {
				Assets = activityAsset,
				Timestamps = activityTimestamp
			};

			// Set first activity
			activityManager.UpdateActivity(activity, (res) => {
				if (res == Discord.Result.Ok)
					Debug.Log("Initialized Discord RPC");
			});

			// Listen to current scene changes
			SceneManager.activeSceneChanged += OnSceneChange;
		}

		private void Update() {
			discord.RunCallbacks();
		}

		private void OnSceneChange(Scene current, Scene next) {
			var activity = new Activity {
				Details = next.name,
				// Get random state, just for variety
				State = GetRandomState(),
				Assets = activityAsset,
				Timestamps = activityTimestamp
			};

			activityManager.UpdateActivity(activity, (res) => {
				if (res == Discord.Result.Ok)
					Debug.Log("Scene changed, RPC Updated");
			});
		}

		private string GetRandomState() {
			return (randomStates.Length > 0) ? randomStates[Random.Range(0, randomStates.Length - 1)] : "";
		}

		private void OnApplicationQuit() {
			discord.Dispose();
		}
	}
}
