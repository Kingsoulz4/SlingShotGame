using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("Fire")]
    [SerializeField] AudioClip firingClip;
    [SerializeField] [Range(0f, 1f)] float volume = 1f;
    [SerializeField] AudioClip playerDamagedClip;
    [SerializeField] AudioClip enemyDamagedClip;
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] AudioClip clickedEffect;

    //Instance 
    public static AudioController sharedInstance;

    public AudioClip FiringClip
    {
        get { return firingClip; }
        set { firingClip = value; }
    }
    public AudioClip PlayerDamagedClip
    {
        get { return firingClip; }
        set { firingClip = value; }
    }
    public AudioClip EnemyDamagedClip
    {
        get { return enemyDamagedClip; }
        set { enemyDamagedClip = value; }
    }
    public AudioClip GameOverClip
    {
        get { return gameOverClip; }
        set { gameOverClip = value; }
    }
    public AudioClip ClickedEffect
    {
        get { return clickedEffect; }
        set { clickedEffect = value; }
    }

    void Awake()
    {
        // Check if SharedInstance is intialized and not equal to the current instance, then destroy it
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }


    /**
     * Play when player using weapon
     */
    public void PlayFiringSound()
    {
        if(firingClip != null)
        {
            AudioSource.PlayClipAtPoint(firingClip, Camera.main.transform.position, volume);
        }
    } 
    /**
     * Play when player being damaged
     */
    public void PlayDamagingPlayerSound()
    {
        if(playerDamagedClip != null)
        {
            AudioSource.PlayClipAtPoint(playerDamagedClip, Camera.main.transform.position, volume);
        }
    }
    /**
     * Play Clicked effect
     */
    public void PlayClickedEffect()
    {
        if(clickedEffect != null)
        {
            AudioSource.PlayClipAtPoint(clickedEffect, Camera.main.transform.position, volume);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
