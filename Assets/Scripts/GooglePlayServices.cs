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
		revealScoreAchievements (level, score);
		revealLifetimeAchievements ();
	}

	public void postDistance (string level, int distance) {
		if (level == LevelManagement.floorIt && distance != 0) {
			Social.ReportScore (distance, FloorItResources.leaderboard_floor_it_distance, (bool success) => {

			});
		} else if (level == LevelManagement.drive && distance != 0) {
			Social.ReportScore (distance, FloorItResources.leaderboard_drive_score, (bool success) => {

			});
		}
		revealDistanceAchievements (level, distance);
	}

	void revealScoreAchievements (string level, int score) {
		if (level == LevelManagement.floorIt) {
			if (score >= 100) {
				Social.ReportProgress (FloorItResources.achievement_floor_it, 100.0f, (bool success) => {
				
				});
			}
			if (score >= 1000) {
				Social.ReportProgress (FloorItResources.achievement_dropping_blocks, 100.0f, (bool success) => {

				});
			}
			if (score >= 2000) {
				Social.ReportProgress (FloorItResources.achievement_block_master, 100.0f, (bool success) => {

				});
			}
		} else if (level == LevelManagement.bowl) {
			if (score >= 100) {
				Social.ReportProgress (FloorItResources.achievement_strike, 100.0f, (bool success) => {
			
				});
			}
			if (score >= 1000) {
				Social.ReportProgress (FloorItResources.achievement_double, 100.0f, (bool success) => {

				});
			}
			if (score >= 2000) {
				Social.ReportProgress (FloorItResources.achievement_turkey, 100.0f, (bool success) => {

				});
			}
		} else if (level == LevelManagement.drive) {
			if (score >= 100) {
				Social.ReportProgress (FloorItResources.achievement_self_control, 100.0f, (bool success) => {
			
				});
			}
			if (score >= 1000) {
				Social.ReportProgress (FloorItResources.achievement_twisting_roads, 100.0f, (bool success) => {

				});
			}
			if (score >= 2000) {
				Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {

				});
			}
		}
	}

	void revealDistanceAchievements (string level, int distance) {
		if (level == LevelManagement.floorIt) {
			if (distance >= 20) {
				//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
				//
				//});
			}
			if (distance >= 200) {
				//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
				//
				//});
			}
			if (distance >= 400) {
				//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
				//
				//});
			}
		} else if (level == LevelManagement.drive) {
			if (distance >= 20) {
				//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
				//
				//});
			}
			if (distance >= 200) {
				//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
				//
				//});
			}
			if (distance >= 400) {
				//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
				//
				//});
			}
		}
	}

	public void revealUnlockAchievements (int carNum) {
		if (carNum == 1) {
			//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
			//
			//});
		} else if (carNum == 2) {
			//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
			//
			//});
		} else if (carNum == 3) {
			//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
			//
			//});
		} else if (carNum == 4) {
			//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
			//
			//});
		} else if (carNum == 5) {
			//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
			//
			//});
		} else if (carNum == 6) {
			//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
			//
			//});
		} else if (carNum == 7) {
			//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
			//
			//});
		}
		revealAllUnlockAchievement ();
	}

	void revealAllUnlockAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.limo, 0) == 0) {
			return;
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.truck, 0) == 0) {
			return;
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.sport, 0) == 0) {
			return;
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.monsterTruck, 0) == 0) {
			return;
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.bus, 0) == 0) {
			return;
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.abstractCar, 0) == 0) {
			return;
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.cone, 0) == 0) {
			return;
		}
		//Social.ReportProgress (FloorItResources.achievement_on_a_narrow_highway, 100.0f, (bool success) => {
		//
		//});
	}

	void revealLifetimeAchievements () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.totalExp, 0) >= 100000) {

			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalExp, 0) >= 1000000) {

			}
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.totalDistance, 0) >= 10000) {

			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalDistance, 0) >= 100000) {

			}
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.totalBlocksActivated, 0) >= 1000) {

			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalBlocksActivated, 0) >= 10000) {

			}
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.totalCarDeaths, 0) >= 500) {

			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalCarDeaths, 0) >= 5000) {

			}
		}
		if (PlayerPrefs.GetInt (PlayerPrefManagement.totalBombCarsBlownUp, 0) >= 100) {

			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalBombCarsBlownUp, 0) >= 1000) {

			}
		}
	}

	public void revealBombOnSpecialBlockAchievement () {

	}

	public void revealOnSpecialBlockAchievement () {

	}

	public void revealStrikeAchievement () {

	}
		
	public void revealNoActivationAchievement () {

	}

	public void revealNoActivationBowlAchievement () {

	}

	public void revealFiveFriendlyCarsAchievement () {

	}

	public void revealBombCollisionAchievement () {

	}

	public void revealHordeAchievement () {

	}

	public void errorMessage () {
		errorText.text = "Connection Failed";
	}
}
