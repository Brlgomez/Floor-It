using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class OnlineServices : MonoBehaviour {

	public Text errorText;

	void Awake () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			PlayGamesPlatform.DebugLogEnabled = false;
			PlayGamesPlatform.Activate ();
			logIn ();
		}
	}

	public void logIn() {
		if (!Social.localUser.authenticated) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					PlayerPrefs.SetInt (PlayerPrefManagement.usingOnlineServices, 0);
				} else {
					PlayerPrefs.SetInt (PlayerPrefManagement.usingOnlineServices, 1);
				}
			});
		}
	}

	public void activateLeaderBoards() {
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				PlayerPrefs.SetInt (PlayerPrefManagement.usingOnlineServices, 0);
				Social.ShowLeaderboardUI();
			} else {
				PlayerPrefs.SetInt (PlayerPrefManagement.usingOnlineServices, 1);
				errorMessage ();
			}
		});
	}

	public void activatedAchievements() {
		Social.localUser.Authenticate((bool success) => {
			if (success) {
				PlayerPrefs.SetInt (PlayerPrefManagement.usingOnlineServices, 0);
				Social.ShowAchievementsUI();
			} else {
				PlayerPrefs.SetInt (PlayerPrefManagement.usingOnlineServices, 1);
				errorMessage ();
			}
		});
	}

	public void postScore (string level, int score) {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			if (level == LevelManagement.floorIt) {
				Social.ReportScore (score, FloorItResources.leaderboard_floor_it_score, (bool success) => {
					if (!success) {
						errorMessage ();
					}
				});
			} else if (level == LevelManagement.bowl) {
				Social.ReportScore (score, FloorItResources.leaderboard_bowl_score, (bool success) => {
					if (!success) {
						errorMessage ();
					}
				});
			} else if (level == LevelManagement.drive) {
				Social.ReportScore (score, FloorItResources.leaderboard_drive_score, (bool success) => {
					if (!success) {
						errorMessage ();
					}
				});
			}
			revealScoreAchievements (level, score);
			revealLifetimeAchievements ();
		}
	}

	public void postDistance (string level, int distance) {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			if (level == LevelManagement.floorIt) {
				Social.ReportScore (distance, FloorItResources.leaderboard_floor_it_distance, (bool success) => {
				});
			} else if (level == LevelManagement.drive) {
				Social.ReportScore (distance, FloorItResources.leaderboard_drive_distance, (bool success) => {
				});
			}
			revealDistanceAchievements (level, distance);
		}
	}

	void revealScoreAchievements (string level, int score) {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			if (level == LevelManagement.floorIt) {
				if (score >= 100) {
					Social.ReportProgress (FloorItResources.achievement_floor_it, 100.0f, (bool success) => {
					});
					if (score >= 1000) {
						Social.ReportProgress (FloorItResources.achievement_build_a_road, 100.0f, (bool success) => {
						});
						if (score >= 2000) {
							Social.ReportProgress (FloorItResources.achievement_hit_the_road, 100.0f, (bool success) => {
							});
						}
					}
				}
			} else if (level == LevelManagement.bowl) {
				if (score >= 100) {
					Social.ReportProgress (FloorItResources.achievement_jim_j_bullock, 100.0f, (bool success) => {
					});
					if (score >= 1000) {
						Social.ReportProgress (FloorItResources.achievement_double, 100.0f, (bool success) => {
						});
						if (score >= 2000) {
							Social.ReportProgress (FloorItResources.achievement_turkey, 100.0f, (bool success) => {
							});
						}
					}
				}
			} else if (level == LevelManagement.drive) {
				if (score >= 100) {
					Social.ReportProgress (FloorItResources.achievement_self_control, 100.0f, (bool success) => {
					});
					if (score >= 1000) {
						Social.ReportProgress (FloorItResources.achievement_the_road, 100.0f, (bool success) => {
						});
						if (score >= 2000) {
							Social.ReportProgress (FloorItResources.achievement_endless_highway, 100.0f, (bool success) => {
							});
						}
					}
				}
			}
		}
	}

	void revealDistanceAchievements (string level, int distance) {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			if (level == LevelManagement.floorIt) {
				if (distance >= 20) {
					Social.ReportProgress (FloorItResources.achievement_move_on_up, 100.0f, (bool success) => {
					});
					if (distance >= 200) {
						Social.ReportProgress (FloorItResources.achievement_wanderer, 100.0f, (bool success) => {
						});
						if (distance >= 400) {
							Social.ReportProgress (FloorItResources.achievement_juggling_master, 100.0f, (bool success) => {
							});
						}
					}
				}
			} else if (level == LevelManagement.drive) {
				if (distance >= 20) {
					Social.ReportProgress (FloorItResources.achievement_dont_stop, 100.0f, (bool success) => {
					});
					if (distance >= 200) {
						Social.ReportProgress (FloorItResources.achievement_lets_go_away, 100.0f, (bool success) => {
						});
						if (distance >= 400) {
							Social.ReportProgress (FloorItResources.achievement_untraveled, 100.0f, (bool success) => {
							});
						}
					}
				}
			}
		}
	}

	public void revealUnlockAchievements () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			if (PlayerPrefs.GetInt (PlayerPrefManagement.limo, 0) == 1) {
				Social.ReportProgress (FloorItResources.achievement_nightjar, 100.0f, (bool success) => {
				});
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.truck, 0) == 1) {
				Social.ReportProgress (FloorItResources.achievement_joey, 100.0f, (bool success) => {
				});
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.sport, 0) == 1) {
				Social.ReportProgress (FloorItResources.achievement_wildflower, 100.0f, (bool success) => {
				});
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.monsterTruck, 0) == 1) {
				Social.ReportProgress (FloorItResources.achievement_baal, 100.0f, (bool success) => {
				});
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.cone, 0) == 1) {
				Social.ReportProgress (FloorItResources.achievement_conic, 100.0f, (bool success) => {
				});
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.bus, 0) == 1) {
				Social.ReportProgress (FloorItResources.achievement_hollyhock, 100.0f, (bool success) => {
				});
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.abstractCar, 0) == 1) {
				Social.ReportProgress (FloorItResources.achievement______, 100.0f, (bool success) => {
				});
			}
			revealAllUnlockAchievement ();
		}
	}

	void revealAllUnlockAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
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
			Social.ReportProgress (FloorItResources.achievement_master_of_none, 100.0f, (bool success) => {
			});
		}
	}

	void revealLifetimeAchievements () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalExp, 0) >= 100000) {
				Social.ReportProgress (FloorItResources.achievement_lump_sum, 100.0f, (bool success) => {
				});
				if (PlayerPrefs.GetInt (PlayerPrefManagement.totalExp, 0) >= 1000000) {
					Social.ReportProgress (FloorItResources.achievement_all_for_myself, 100.0f, (bool success) => {
					});
				}
			}
			if (PlayerPrefs.GetFloat (PlayerPrefManagement.totalDistance, 0) >= 10000) {
				Social.ReportProgress (FloorItResources.achievement_road_blocks, 100.0f, (bool success) => {
				});
				if (PlayerPrefs.GetFloat (PlayerPrefManagement.totalDistance, 0) >= 100000) {
					Social.ReportProgress (FloorItResources.achievement_highwayman, 100.0f, (bool success) => {
					});
				}
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalBlocksActivated, 0) >= 1000) {
				Social.ReportProgress (FloorItResources.achievement_im_not_done, 100.0f, (bool success) => {
				});
				if (PlayerPrefs.GetInt (PlayerPrefManagement.totalBlocksActivated, 0) >= 10000) {
					Social.ReportProgress (FloorItResources.achievement_the_elements, 100.0f, (bool success) => {
					});
				}
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalCarDeaths, 0) >= 500) {
				Social.ReportProgress (FloorItResources.achievement_cars_never_die, 100.0f, (bool success) => {
				});
				if (PlayerPrefs.GetInt (PlayerPrefManagement.totalCarDeaths, 0) >= 5000) {
					Social.ReportProgress (FloorItResources.achievement_who_will_fall_far_behind, 100.0f, (bool success) => {
					});
				}
			}
			if (PlayerPrefs.GetInt (PlayerPrefManagement.totalBombCarsBlownUp, 0) >= 100) {
				Social.ReportProgress (FloorItResources.achievement_if_i_had_a_heart, 100.0f, (bool success) => {
				});
				if (PlayerPrefs.GetInt (PlayerPrefManagement.totalBombCarsBlownUp, 0) >= 1000) {
					Social.ReportProgress (FloorItResources.achievement_quiet_horses, 100.0f, (bool success) => {
					});
				}
			}
		}
	}

	public void revealBombOnSpecialBlockAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			Social.ReportProgress (FloorItResources.achievement_thats_not_me, 100.0f, (bool success) => {
			});
		}
	}

	public void revealOnSpecialBlockAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			Social.ReportProgress (FloorItResources.achievement_every_super_hero_needs_their_theme_music, 100.0f, (bool success) => {
			});
		}
	}

	public void revealStrikeAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			Social.ReportProgress (FloorItResources.achievement_x, 100.0f, (bool success) => {
			});
		}
	}
		
	public void revealNoActivationAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			Social.ReportProgress (FloorItResources.achievement_triple_double_no_assist, 100.0f, (bool success) => {
			});
		}
	}

	public void revealNoActivationBowlAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			Social.ReportProgress (FloorItResources.achievement_chelseas_law, 100.0f, (bool success) => {
			});
		}
	}

	public void revealFiveFriendlyCarsAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			Social.ReportProgress (FloorItResources.achievement_by_the_kith_and_the_kin, 100.0f, (bool success) => {
			});
		}
	}

	public void revealBombCollisionAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			Social.ReportProgress (FloorItResources.achievement_friendly_fire, 100.0f, (bool success) => {
			});
		}
	}

	public void revealHordeAchievement () {
		if (PlayerPrefs.GetInt (PlayerPrefManagement.usingOnlineServices, 0) == 0) {
			Social.ReportProgress (FloorItResources.achievement_so_thats_what_happens, 100.0f, (bool success) => {
			});
		}
	}

	public void errorMessage () {
		errorText.text = "Not Signed In";
	}
}
