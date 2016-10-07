using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class GooglePlayServices : MonoBehaviour {

	void Awake () {
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
	}

	public void activateLeaderBoards() {
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				Social.ShowLeaderboardUI();
			} 
		});
	}

	public void activatedAchievements() {
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				Social.ShowAchievementsUI();
			} 
		});
	}

	public void postScore (string level, int score) {
		if (level == LevelManagement.floorIt) {
			Social.ReportScore (score, FloorItResources.leaderboard_floor_it_highscore, (bool success) => {

			});
		} else if (level == LevelManagement.bowl) {
			Social.ReportScore (score, FloorItResources.leaderboard_bowl_highscore, (bool success) => {

			});
		} else if (level == LevelManagement.drive) {
			Social.ReportScore (score, FloorItResources.leaderboard_drive_highscore, (bool success) => {

			});
		}
	}

	public void postDistance (string level, int distance) {
		if (level == LevelManagement.floorIt) {
			Social.ReportScore (distance, FloorItResources.leaderboard_floor_it_farthest_distance, (bool success) => {

			});
		} else if (level == LevelManagement.drive) {
			Social.ReportScore (distance, FloorItResources.leaderboard_drive_farthest_distance, (bool success) => {

			});
		}
	}
}
