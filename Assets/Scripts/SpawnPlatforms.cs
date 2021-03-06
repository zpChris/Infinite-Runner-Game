﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatforms : MonoBehaviour {

	public GameObject safePlatform; //a regular green platform
	private Vector3 spawnLocation; //spawn location

	private Vector2 sizeRange; //range of platform sizes that could spawn
	private float size; //size of latest platform, determined by random range of sizeRange

	private Vector2 xSpacingRange; //defined in the spawnPositionX(); range of spacing between platforms
	private float xSpawnPosition; //x spawn position of latest platform

	private float zSpawnPosition; //produces integer from -1 to 1, multiplied by 5 to give spawnPosition

	public GameObject collectable; //collectable
	private float colSpawnPos; //x spawn position of collectables on platform

	public GameObject spike; //spike
	private float spikeSpawnPos; //x spawn position of spikes on platform

	private float timeIncrease; //a value that increases as time progresses, used to scale platforms/spawning

	// Use this for initialization
	void Start () {
		GameplayManager.elapsedTime = 0.0f; //for some reason this class is "called" first so this is needed
		sizeRange = new Vector2 (20, 50);
		size = 30;
		timeIncrease = 0.0f;
		prewarmPlatforms ();
	}
	
	// Update is called once per frame
	void Update () {
		timeIncrease = Time.time / 5.0f;
		if (SafePlatformCharacteristics.spawnedNew) {
			SafePlatformCharacteristics.spawnedNew = false;
			spawnPlatform ();
			if (size > 35 && Random.Range (0, 3) == 0) {
				spawnCollectables ();
			}
			if (size > 40 && Random.Range (0, 1) == 0) {
				spawnSpikes ();
			}
		}
	}

	//spawns 8 extra platforms to look like they are preloaded
	void prewarmPlatforms() {
		for (int i = 0; i < 8; i++) {
			spawnPlatform();
			if (size > 35 && Random.Range (0, 2) == 0) {
				spawnCollectables ();
			}
			if (size > 40 && Random.Range (0, 1) == 0) {
				spawnSpikes ();
			}
		}
	}

	//makes platform random size based on sizeRange
	void setPlatformSizeX() {
		size = Random.Range (sizeRange.x, sizeRange.y) + timeIncrease;
		safePlatform.transform.localScale = new Vector3 (size, 1, 1);
	}

	float getSpawnPositionX() {
		xSpacingRange = new Vector2 (5 + GameplayManager.elapsedTime / 2.0f, 10 + GameplayManager.elapsedTime / 2.0f);
		xSpawnPosition += size/2; //add old size
		xSpawnPosition += Random.Range (xSpacingRange.x, xSpacingRange.y);
		setPlatformSizeX ();
		xSpawnPosition += size / 2; //add new size
		return xSpawnPosition;
	}

	float getSpawnPositionZ() {
		zSpawnPosition = Random.Range (-1, 2) * 5; //doesn't include last number
		//if the platform is on the left side, can't spawn on the right side, or vice versa (spawns in middle instead)
		if (zSpawnPosition == -spawnLocation.z) {
			zSpawnPosition = 0.0f;
		}
		return zSpawnPosition;
	}

	void spawnPlatform() {
		spawnLocation = new Vector3 (getSpawnPositionX(), 0.0f, getSpawnPositionZ());
		Instantiate (safePlatform, spawnLocation, Quaternion.identity);
	}

	void spawnCollectables() {
		colSpawnPos = Random.Range(-10, 10) + xSpawnPosition;
		for (int i = 0; i < Random.Range(1, 5); i++) {
			Instantiate (collectable, new Vector3 (colSpawnPos + (2 * i), 1.0f, zSpawnPosition), Quaternion.identity);
		}
	}

	void spawnSpikes() {
		spikeSpawnPos = Random.Range (-12.5f, 2.5f) + xSpawnPosition;
		//checks to make sure that the collectables are at least 6 units away from the spike or if collectables were not spawned
		if (Mathf.Abs(spikeSpawnPos - colSpawnPos) > 6) {
			for (int i = 0; i < Random.Range(1, 4); i++) {
				Instantiate(spike, new Vector3(spikeSpawnPos + (2 * i), 0.8f, zSpawnPosition), Quaternion.Euler(-90, 0, 0));
			}
		}

	}		
}
