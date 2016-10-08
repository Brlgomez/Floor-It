using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class GooglePlayServices : MonoBehaviour {

	void Awake () {
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = false;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
		Social.localUser.Authenticate ((bool success) => {});
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
		if (level == LevelManagement.floorIt && score != 0) {
			Social.ReportScore (score, FloorItResources.leaderboard_floor_it_highscore, (bool success) => {

			});
		} else if (level == LevelManagement.bowl && score != 0) {
			Social.ReportScore (score, FloorItResources.leaderboard_bowl_highscore, (bool success) => {

			});
		} else if (level == LevelManagement.drive && score != 0) {
			Social.ReportScore (score, FloorItResources.leaderboard_drive_highscore, (bool success) => {

			});
		}
	}

	public void postDistance (string level, int distance) {
		if (level == LevelManagement.floorIt && distance != 0) {
			Social.ReportScore (distance, FloorItResources.leaderboard_floor_it_farthest_distance, (bool success) => {

			});
		} else if (level == LevelManagement.drive && distance != 0) {
			Social.ReportScore (distance, FloorItResources.leaderboard_drive_farthest_distance, (bool success) => {

			});
		}
	}

	public void revealScoreAchievements (string level, int score) {
		if (level == LevelManagement.floorIt) {
			if (score >= 100) {
				Social.ReportProgress (FloorItResources.achievement_floor_it, 100.0f, (bool success) => {
				
				});
			}
			if (score >= 500) {
				Social.ReportProgress (FloorItResources.achievement_dropping_blocks, 100.0f, (bool success) => {

				});
			}
			if (score >= 1000) {
				Social.ReportProgress (FloorItResources.achievement_block_master, 100.0f, (bool success) => {

				});
			}
		} else if (level == LevelManagement.bowl) {
			if (score >= 100) {
				Social.ReportProgress (FloorItResources.achievement_strike, 100.0f, (bool success) => {
			
				});
			}
			if (score >= 500) {
				Social.ReportProgress (FloorItResources.achievement_double, 100.0f, (bool success) => {

				});
			}
			if (score >= 1000) {
				Social.ReportProgress (FloorItResources.achievement_turkey, 100.0f, (bool success) => {

				});
			}
		} else if (level == LevelManagement.drive) {
			if (score >= 100) {
				Social.ReportProgress (FloorItResources.achievement_self_control, 100.0f, (bool success) => {
			
				});
			}
			if (score >= 500) {
				Social.ReportProgress (FloorItResources.achievement_twisting_roads, 100.0f, (bool success) => {

				});
			}
			if (score >= 1000) {
				Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {

				});
			}
		}
	}
}
