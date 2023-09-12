using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {

	// This is the list of things that need data to be saved
	public CoinManager coinManager;
	public LevelManager levelManager;
	public CollectableManager collectableManager;

    // Objects for spawning new / saved coins
    public GameObject coinPlayField;
    public GameObject coinPreloadedTable;

    // Our saved coins that are searlized
    public Dictionary<string, CoinData> savedCoins;

    public void Awake()
    {
        // Init our dictionary of serialized data
        savedCoins = new Dictionary<string, CoinData>();

        // Used to continously save the coin data
        InvokeRepeating("saveCoinData", 1.0f, 5.0f);
    }

    /// <summary>
    /// Loads the data.
    /// </summary>
    public void loadData()
	{
		// Get data for the CoinManager
		if( PlayerPrefs.HasKey("currentCoinTotal") )
			coinManager.currentCoinTotal = PlayerPrefs.GetInt("currentCoinTotal");

		if( PlayerPrefs.HasKey("playerCash") )
			coinManager.playerCash = PlayerPrefs.GetInt("playerCash");

		// Get data for the LevelManager
		if( PlayerPrefs.HasKey("currentLevel") )
			levelManager.currentLevel = PlayerPrefs.GetFloat ("currentLevel");

		if( PlayerPrefs.HasKey("currentLevelAmount") )
			levelManager.currentLevelAmount = PlayerPrefs.GetFloat ("currentLevelAmount");

		// Get the data for the CollectableManager
		if (PlayerPrefs.HasKey ("collectables")) 
			collectableManager.inventory = PlayerPrefsSerialize<Dictionary<CoinEffect.Effect, int>>.Load ("collectables");
		else 
			collectableManager.inventory = new Dictionary<CoinEffect.Effect, int>();

    }

    /// <summary>
    /// Saves the data.
    /// </summary>
    public void saveData()
    {
        // Save data for the CoinManager
        PlayerPrefs.SetInt("currentCoinTotal", coinManager.currentCoinTotal);
        PlayerPrefs.SetInt("playerCash", coinManager.playerCash);

        // Save data for the LevelManager
        PlayerPrefs.SetFloat("currentLevel", levelManager.currentLevel);
        PlayerPrefs.SetFloat("currentLevelAmount", levelManager.currentLevelAmount);

        // Save data for the CollectableManager
        PlayerPrefsSerialize<Dictionary<CoinEffect.Effect, int>>.Save(collectableManager.collectableSaveName, collectableManager.inventory);

        saveCoinData();
    }

    /// <summary>
    /// Used for loading saved coin data
    /// </summary>
    public void loadSavedCoinData()
    {
        // Load any saved coins on the table
        // If we have save data, load it
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_CoinSaveData"))
        {
            // Disable the preloaded coin top
            coinPreloadedTable.SetActive(false);

            // Load the saved data
            savedCoins = PlayerPrefsSerialize<Dictionary<string, CoinData>>.Load(SceneManager.GetActiveScene().name + "_CoinSaveData");
            
            foreach (KeyValuePair<string, CoinData> coin in savedCoins)
            {
                // Get the object name we need to spawn
                // Look for the first blank whitespace and (
                int index = coin.Value.objectName.IndexOf(" (");

                // If we found enough of a string to parse
                if (index > 0)
                {
                    // Pull out everything UP to the " (" part
                    coin.Value.objectName = coin.Value.objectName.Substring(0, index);
                }

                // Now search and destroy for (Clone)
                index = coin.Value.objectName.IndexOf("(Clone)");

                // If we found enough of a string to parse
                if (index > 0)
                {
                    // Pull out everything UP to the "(Clone)" part
                    coin.Value.objectName = coin.Value.objectName.Substring(0, index);
                }

                // Loop through all available spawnables
                for (int i = 0; i < coinManager.allSpawnableCoinsItems.Length; i++)
                {                   
                    // If we find our gameobject name in the array of spawnable items / objects
                    if (String.Compare(coinManager.allSpawnableCoinsItems[i].name, coin.Value.objectName) == 0)
                    { 
                        // Save this spawnID index
                        coin.Value.spawnID = i;
                    }
                }

                // Spawn our new item
                GameObject go = Instantiate(coinManager.allSpawnableCoinsItems[coin.Value.spawnID]);

                // Reparent it
                go.transform.SetParent(coinPlayField.transform);

                // Adjust the position & rotation from the saved data
                go.transform.localPosition = new Vector3(coin.Value.posX, coin.Value.posY, coin.Value.posZ);
                go.transform.rotation = Quaternion.Euler(coin.Value.rotX, coin.Value.rotY, coin.Value.rotZ);
                
                // Update settings for this coin
                go.GetComponent<CoinEffect>().coinData.id = coin.Value.id;
                go.GetComponent<CoinEffect>().coinData.coinValue = coin.Value.coinValue;
                go.GetComponent<CoinEffect>().coinData.typeOfCoin = coin.Value.typeOfCoin;
                go.GetComponent<CoinEffect>().coinData.objectName = coin.Value.objectName;
            }
        }
        else
        {
            // Show the stock preloaded coin table top
            coinPreloadedTable.SetActive(true);
        }
    }

    /// <summary>
    /// Used for saving just coin data on the table
    /// </summary>
    public void saveCoinData()
    {
        // Save the seralized data
        PlayerPrefsSerialize<Dictionary<string, CoinData>>.Save(SceneManager.GetActiveScene().name + "_CoinSaveData", savedCoins);
    }

    public void addCoin(CoinData coinData)
    {
        // If we do not have this coin added already
        if (savedCoins.ContainsKey(coinData.id) == false)
        {
            // Let's add it
            savedCoins.Add(coinData.id, coinData);
        }
    }

    public void updateCoin(CoinData coinData)
    {
        // Make sure we have the coin to update
        if (savedCoins.ContainsKey(coinData.id) == true)
        {
            // Let's update this coin since it has changed
            savedCoins[coinData.id] = coinData;
        }
    }

	/// <summary>
	/// Checks for saved game.
	/// </summary>
	/// <returns><c>true</c>, if for saved game was checked, <c>false</c> otherwise.</returns>
	public bool checkForSavedGame()
	{
		// See what the return value is, if it is -1 we have no value
		return PlayerPrefs.GetInt("currentCoinTotal", -1) == -1 ? false : true;
	}

	/// <summary>
	/// Deletes all saved game data.
	/// </summary>
	public void deleteAllSavedGameData()
	{
		PlayerPrefs.DeleteAll ();
	}
}

