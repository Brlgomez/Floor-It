﻿using UnityEngine;
using System.Collections;

public class DeadObjectCatcher : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag != TagManagement.pin) {
			Destroy (other.gameObject);
		}
	}
}
