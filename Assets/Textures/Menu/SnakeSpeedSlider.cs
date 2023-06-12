using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SnakeSpeedSlider : MonoBehaviour
{
    public Slider speedSlider;
    public float minSpeed = 3f; // minimum speed of the snake
    public float maxSpeed = 15f; // maximum speed of the snake
    public Snake Snake; // reference to the SnakeController script

    void Start()
    {
        // set the initial value of the slider based on the current snake speed
        if (Snake != null) {
        float savedSpeed = PlayerPrefs.GetFloat("SnakeSpeed", Snake.SnakeSpeed);
            speedSlider.value = Mathf.InverseLerp(minSpeed, maxSpeed, savedSpeed);
            Snake.SnakeSpeed = savedSpeed;
    }
    }

    public void SetSpeed()
    {
        // set the new speed of the snake based on the value of the slider
        float newSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedSlider.value);
        Snake.SnakeSpeed = newSpeed;
        PlayerPrefs.SetFloat("SnakeSpeed", newSpeed);
    }
}
