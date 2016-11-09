﻿using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

	int speed = 0;

	void Start () {
		speed = Random.Range (10, 100);
		if (speed % 2 == 0) {
			speed *= -1;
		}
	}

	void Update () {
		transform.Rotate (Time.deltaTime * speed, 0 , 0);
	}
}
