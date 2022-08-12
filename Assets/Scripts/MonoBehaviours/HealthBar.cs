using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public HitPoints hitPoints;
    [HideInInspector]
    public Player character;
    public Image meterImage;
    public Text hpText;
    float maxHitPoints;
    void Start()
    {
        maxHitPoints = character.maxHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        if(character!=null)
        {
            meterImage.fillAmount = hitPoints.value/maxHitPoints;
            hpText.text = "HP:" + (meterImage.fillAmount*100);
        }
    }
}
