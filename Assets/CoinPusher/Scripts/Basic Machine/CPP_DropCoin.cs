using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPP_DropCoin : MonoBehaviour
{
	[Header("Regular Coins")]
	// Our public array of coins we should spawn
	public Transform[] coins;

	[Header("Common Coins")]
	public Transform[] common;
	[Header("Rare Coins")]
	public Transform[] rare;
	[Header("Epic Coins")]
	public Transform[] epic;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			{
				// Make sure they clicked on the spawn area
				if (hit.collider.CompareTag("TouchClickArea"))
				{
					// Spawn a coin
					spawnCoin(hit.point);
				}
			}

		}
	}

	public void spawnCoin(Vector3 position)
	{
		Vector3 spawnLocation;

		// Spawn at click/tap location
		// x = z, y = y, z = x
		spawnLocation = new Vector3(position.x, transform.position.y, position.z);
		
		// Here is where we decide on what item to spawn, based on rarity.
		// To get a common item, it will be between: 1 - 5
		// To get a rare item, it wil be between: 1 - 15
		// To get an epic item, it will be between: 1 - 30
		int findItem = 4;
		int findCommon = Random.Range(1, 5);
		int findRare = Random.Range(1, 30);
		int findEpic = Random.Range(1, 45);

		if (findItem == findCommon)
		{
			if (common.Length != 0)
			{
				GameObject.Instantiate(common[Random.Range(0, common.Length)], spawnLocation, common[0].rotation);
			}
		}

		if (findItem == findRare)
		{
			if (rare.Length != 0)
			{
				GameObject.Instantiate(rare[Random.Range(0, rare.Length)], spawnLocation, rare[0].rotation);
			}
		}

		if (findItem == findEpic)
		{
			if (epic.Length != 0)
			{
				GameObject.Instantiate(epic[Random.Range(0, epic.Length)], spawnLocation, epic[0].rotation);
			}
		}

		if (findItem != findCommon && findItem != findRare && findItem != findEpic)
		{
			GameObject.Instantiate(coins[Random.Range(0, coins.Length)], spawnLocation, coins[0].rotation);
		}
	}
}
