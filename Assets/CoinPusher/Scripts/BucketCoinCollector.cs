using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BucketCoinCollector : MonoBehaviour {

	// The master coin manager object
	public CoinManager coinManager;

	// The level manager
	public LevelManager levelManager;

	public void Start()
	{
		if (!levelManager)
			levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>(); 
		
		if (!coinManager)
			coinManager = GameObject.FindGameObjectWithTag("CoinManager").GetComponent<CoinManager>();
	}

	// This is called when a coin/object falls into the bucket
	void OnTriggerEnter(Collider other) 
	{
		int value = other.gameObject.GetComponent<CoinEffect>().coinValue;
		other.gameObject.GetComponent<CoinEffect>().effect();

		// Send this off to the score manager to add the value the coin they got is worth
		coinManager.addCoin(value, false);

		// Now add a some value to their level
		levelManager.updateLevel(value);
	}
}