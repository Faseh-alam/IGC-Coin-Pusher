using UnityEngine;
using System.Collections;

[System.Serializable]
public class CoinData
{
    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;

    public int coinValue;

    // Let's define the type of effect this coin is
    public enum Effect
    {
        RegularCoin,
        BumperWallCoin,
        BullseyeCoin,
        CashCoin,
        GiftCoin,
        QuakeShakeCoin,
        StopCoin,
        CollectableDonut,
        CollectableCocoDonut,
        CollectableDice,
        CollectableOrangeDice,
        CollectableGoldBar
    }
    public Effect typeOfCoin;

    public string id;

    // This is the number of this spawnable coin (which prefab) in the CoinManager array: allSpawnableCoinsItems
    public int spawnID;

    // This is the spawnable name (GameObject name) of the item
    public string objectName;
   
}

public class CoinEffect : MonoBehaviour {

	// This is the value this coin is worth
	public int coinValue = 1;

	// Our EffectsManager
	private EffectsManager effectsManager;

    // Our SaveManager
    private SaveManager saveManager;

	// Let's define the type of effect this coin is
	public enum Effect {
		RegularCoin,
		BumperWallCoin,
		BullseyeCoin,
		CashCoin,
		GiftCoin,
		QuakeShakeCoin,
		StopCoin,
		CollectableDonut,
		CollectableCocoDonut,
		CollectableDice,
		CollectableOrangeDice,
		CollectableGoldBar
	}
	public Effect typeOfCoin;

	// This is used to prevent the coin SFX from playing when the coins start on the play field
	public bool alreadyOnPlayField;

	// This is the SFX of when the coin is dropped
	public AudioClip droppedSound;

	// Did it land, set this to true so we stop collisions later
	private bool didLand = false;

	// This is the sound effect when the coin is dropped and not collected
	public AudioClip destroyedSound;

	[Header("Collectable and Coin Shop Settings")]

	// Prize only related things, if it can be sold on the collectable screen and in the coin shop for buying
	public Sprite prizeImage;

	// The price of this item in the coin shop only
	public int coinShopItemPrice = 1;

    public int spawnableID = 0;

    public CoinData coinData;

	void Start()
	{
        // If we start with a coinData.id that is valid
        if ( coinData.id.Length > 0 )
        {
            // Load our previous coin settings
            this.coinValue = coinData.coinValue;
            this.typeOfCoin = (CoinEffect.Effect)coinData.typeOfCoin;
            this.name = coinData.objectName;
        }
        else
        {
            // Configure basic starting values for this new coin
            coinData.coinValue = this.coinValue;
            coinData.typeOfCoin = (CoinData.Effect)this.typeOfCoin;

            // When we spawn a new coin, make sure to save its name
            coinData.objectName = this.name;

            // Make a new ID for this coin since it's a fresh start
            coinData.id = System.Guid.NewGuid().ToString();
        }
        
        // Grab the object
        effectsManager = GameObject.FindWithTag("EffectManager").GetComponent<EffectsManager>();

        // Grab the save manager
        saveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();

        // Add this to our save manager
        if (saveManager != null)
        {
            saveManager.addCoin(coinData);
        }

		// Reparent this new coin
		this.gameObject.transform.parent = GameObject.FindWithTag("CoinsPlayField").transform;
    }

    public void Update()
    {
        updateCoinData();
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The value.</returns>
    public int getValue()
	{
		return coinValue;
	} 

    /// <summary>
    /// This is called from any area that needs to trigger the effect which is passed on to the effects manager
    /// </summary>
    public void effect()
	{
        if( effectsManager != null )
		    effectsManager.runEffect(typeOfCoin, coinValue);

		// Remove the coin
		removeCoin();
	}

	/// <summary>
	/// This is called externally, in the coin destroyer, when it drops and is not collected.
	/// </summary>
	public void playDestroyedSFX()
	{
		// Make sure the variable is not null, if not, play it
		if( destroyedSound != null )
			Camera.main.GetComponent<AudioSource>().PlayOneShot(destroyedSound);
	}

	// Remove this object when done
	public void removeCoin()
	{
        Destroy(this.gameObject, 0.1f);
	}

	void OnCollisionEnter (Collision col)
	{
        // If we did not land
        if ( !didLand )
		{
			// If we were not already on the play field (aka a coin in the beginnning)
			if( !alreadyOnPlayField )
			{
				// If we hit the push bar, floor
				if( col.gameObject.CompareTag("PushBar") ||  col.gameObject.CompareTag("Floor") )
				{
					// Make sure there is a sound effect to play
					if( droppedSound != null )
						Camera.main.GetComponent<AudioSource>().PlayOneShot(droppedSound);

					// Mark that we landed already
					didLand = true;
                }
			}
		}     
	}
    

    public void updateCoinData()
    {
        coinData.posX = this.gameObject.transform.localPosition.x;
        coinData.posY = this.gameObject.transform.localPosition.y;
        coinData.posZ = this.gameObject.transform.localPosition.z;

        coinData.rotX = this.gameObject.transform.rotation.x;
        coinData.rotY = this.gameObject.transform.rotation.y;
        coinData.rotZ = this.gameObject.transform.rotation.z;
        coinData.rotW = this.gameObject.transform.rotation.w;

        saveManager.updateCoin(coinData);
    }
}