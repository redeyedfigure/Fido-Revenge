using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider healthFillSlider;
    public Slider healthInterpSlider;
    public Slider armorFillSlider;
    public Image armorFillImage;
    private int armorOpacity, armorTextOpacity;
    public PlayerAttributes playerAttributes;
    public TMP_Text healthCounter, armorCounter;



    // Start is called before the first frame update
    void Start()
    {
        healthFillSlider.maxValue = (float)playerAttributes.health;
        healthInterpSlider.maxValue = (float)playerAttributes.health;
        armorFillSlider.maxValue = (float)playerAttributes.armor;
    }

    // Update is called once per frame
    void Update()
    {
        healthFillSlider.value = ToSingle(playerAttributes.health);
        healthInterpSlider.value -= (healthInterpSlider.value - healthFillSlider.value ) * Time.deltaTime * 2;
        armorFillSlider.value = playerAttributes.armor;
        
        armorOpacity = (int)(armorFillSlider.value * 8);
        armorTextOpacity = 255 - armorOpacity;

        armorFillImage.color = new Color32(100,200,255,(byte)armorOpacity);
        armorCounter.color = new Color32(100,200,255,(byte)armorTextOpacity);

        healthCounter.text = playerAttributes.healthCount;
        armorCounter.text = playerAttributes.armorCount;
        
    }

    public static float ToSingle(double value)
    {
        return (float) value;
    }
}