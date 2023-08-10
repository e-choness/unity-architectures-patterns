using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TickingClock : MonoBehaviour
{
    private const float hoursToDegrees = 30.0f;
    private const float minutesToDegrees = 6.0f;
    private const float secondsToDegrees = 6.0f;
    private AudioSource audioSource;
    private Transform handHour, handMinute, handSecond;
    private DateTime lastTime, currentTime;
    

    // Start is called before the first frame update

    private void Awake()
    {
        currentTime = DateTime.Now;
        handHour = transform.GetChild(1);
        handMinute = transform.GetChild(2);
        handSecond = transform.GetChild(3);
        handHour.rotation = Quaternion.Euler(currentTime.Hour * hoursToDegrees + 90, 0.0f, -90.0f);
        handMinute.rotation = Quaternion.Euler(currentTime.Minute * minutesToDegrees + 90, 0.0f, -90.0f);
        handSecond.rotation = Quaternion.Euler(currentTime.Second * secondsToDegrees + 90, 0.0f, -90.0f);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateHandlesRotation();
    }
    
    private bool CheckHourChanged()
    {
        return currentTime.Hour != lastTime.Hour;
    }

    private bool CheckMinuteChanged()
    {
        return currentTime.Minute != lastTime.Minute;
    }

    private bool CheckSecondChanged()
    {
        return currentTime.Second != lastTime.Second;
    }

    void UpdateHandlesRotation()
    {
        currentTime = DateTime.Now;
        if (CheckHourChanged())
        {
            handHour.rotation = Quaternion.Euler(currentTime.Hour * hoursToDegrees + 90, 0.0f, -90.0f);
            audioSource.Play(); // A different audio clip can be played here
        }
        if (CheckMinuteChanged())
        {
            handMinute.rotation = Quaternion.Euler(currentTime.Minute * minutesToDegrees + 90, 0.0f, -90.0f);
            audioSource.Play();// A different audio clip can be played here
        }
        if (CheckSecondChanged())
        {
            handSecond.rotation = Quaternion.Euler(currentTime.Second * secondsToDegrees + 90, 0.0f, -90.0f);
            audioSource.Play();
        }

        StartCoroutine(WaitForASecond());
        lastTime = DateTime.Now;
    }

    IEnumerator WaitForASecond()
    {
        yield return new WaitForSeconds(1);
    }
}
