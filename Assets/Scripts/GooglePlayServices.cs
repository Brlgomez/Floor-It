using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class GooglePlayServices : MonoBehaviour {

	public Text errorText;

	void Awake () {
		PlayGamesPlatform.DebugLogEnabled = false;
		PlayGamesPlatform.Activate ();
		logIn ();
	}

	public void logIn() {
		if (!Social.localUser.authenticated) {
			// Activate the Google Play Games platform
			Social.localUser.Authenticate ((bool success) => {

			});
		}
	}

	public void activateLeaderBoards() {
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				Social.ShowLeaderboardUI();
			} else {
				errorMessage ();
			}
		});
	}

	public void activatedAchievements() {
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				Social.ShowAchievementsUI();
			} else {
				errorMessage ();
			}
		});
	}

	public void postScore (string level, int score) {
		if (level == LevelManagement.floorIt && score != 0) {
			Social.ReportScore (score, FloorItResources.leaderboard_floor_it_score, (bool success) => {
				if (!success) {
					errorMessage ();
				}
			});
		} else if (level == LevelManagement.bowl && score != 0) {
			Social.ReportScore (score, FloorItResources.leaderboard_bowl_score, (bool success) => {
				if (!success) {
					errorMessage ();
				}
			});
		} else if (level == LevelManagement.drive && score != 0) {
			Social.ReportScore (score, FloorItResources.leaderboard_drive_score, (bool success) => {
				if (!success) {
					errorMessage ();
				}
			});
		}
	}

	public void postDistance (string level, int distance) {
		if (level == LevelManagement.floorIt && distance != 0) {
			Social.ReportScore (distance, FloorItResources.leaderboard_floor_it_distance, (bool success) => {

			});
		} else if (level == LevelManagement.drive && distance != 0) {
			Social.ReportScore (distance, FloorItResources.leaderboard_drive_score, (bool success) => {

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

	public void errorMessage () {
		errorText.text = "Connection Failed";
	}
}
