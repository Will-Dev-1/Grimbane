using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    
public Slider healthSlider;
public Slider easeSlider;
public float maxHealth = 50f;
public float health;
private float lerpSpeed = 0.05f;

void Start()
{
    health = maxHealth;
        healthSlider.maxValue = maxHealth;
}

void Update()
{
    if(healthSlider.value != health)
    {
        healthSlider.value = health;
    }

    //Testing
    //if(Input.GetKeyDown(KeyCode.Space))
    //{
    //    takeDamage(10);
    //}

    if(healthSlider.value != easeSlider.value)
    {
        easeSlider.value = Mathf.Lerp(easeSlider.value, health, lerpSpeed);
    }
}

void takeDamage(float damage)
{
    health -= damage;
}
}
