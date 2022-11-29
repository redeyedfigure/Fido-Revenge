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
    private int armorOpacity;
    public PlayerAttributes playerAttributes;
    public TMP_Text healthCounter;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthFillSlider.value = ToSingle(playerAttributes.health);
        healthInterpSlider.value -= (healthInterpSlider.value - healthFillSlider.value ) * Time.deltaTime * 2;
        armorFillSlider.value = playerAttributes.armor;
        armorOpacity = (int)(armorFillSlider.value * 8);
        armorFillImage.color = new Color32(255,255,255,(byte)armorOpacity);

        healthCounter.text = playerAttributes.healthCount;
    }

    public static float ToSingle(double value)
    {
        return (float) value;
    }
}