using System;
using UnityEngine;
using System.Collections.Generic;

public class LissajousAnimationTask4 : MonoBehaviour
{
    [Header("X coordinates")]

    [SerializeField] protected float amplitudeX = 1f;
    [SerializeField] protected float frequancyX = 1f;
    [SerializeField] protected float shift = 0f;

    [Header("Y coordinates")]
    [SerializeField] protected float amplitudeY = 1f;
    [SerializeField] protected float frequancyY = 1f;

    [Header("Time")]
    [SerializeField] protected float timeScale = 1f;
    [SerializeField] protected float currentTime = 0f;

    [Header("Other")]
    public Vector3 startPosition;

    protected void Update()
    {
        currentTime += Time.deltaTime * timeScale;
        UpdatePosition(currentTime);
    }

    private void UpdatePosition(float t)
    {
        // Updated position based on Lissajous result
        Vector2 offset = GetParametricPosition(t);
        transform.position = new Vector3(
            startPosition.x + offset.x,
            startPosition.y + offset.y,
            startPosition.z
        );
    }

    public Vector2 GetParametricPosition(float t)
    {
        // Implmenting Lissajous formula of x = Asin(at + g), y = Bsin(bt)
        float x = amplitudeX * Mathf.Sin(frequancyX * t + shift);
        float y = amplitudeY * Mathf.Sin(frequancyY * t);

        return new Vector2(x, y);
    }


    /// <summary>
    /// A quick and easy way to set a range 
    /// </summary>
    /// <param name="lissajousValue"></param>
    /// <param name="minimumValue"></param>
    /// <param name="maximumValue"></param>
    public void RandomiseLissajousValue(LissajousValues lissajousValue, float minimumValue, float maximumValue) {

        // Dictionary to get store the correct values
        var fieldMap = new Dictionary<LissajousValues, Action<float>> {
            
            { LissajousValues.amplitudeX, value => amplitudeX = value },
            { LissajousValues.amplitudeY, value => amplitudeY = value },
            { LissajousValues.frequancyX, value => frequancyX = value },
            { LissajousValues.frequancyY, value => frequancyY = value },
            { LissajousValues.Shift, value => shift = value },
            { LissajousValues.timeScale, value => timeScale = value },
            { LissajousValues.currentTime, value => currentTime = value }
        };

        if (fieldMap.TryGetValue(lissajousValue, out var setter))
        {
            setter(UnityEngine.Random.Range(minimumValue, maximumValue));
        }
    }
}