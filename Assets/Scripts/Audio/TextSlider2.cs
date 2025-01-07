using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextSlider2 : MonoBehaviour
{
    public TMP_Text numberText2;
    private Slider slider2;

    void Start() 
    {
        slider2 = GetComponent<Slider>();
        SetNumberText2(slider2.value);
    }

    public void SetNumberText2(float value) 
    {
        numberText2.text = value.ToString();
    }

}
