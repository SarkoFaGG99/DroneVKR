using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    public static Stopwatch Instance;
    public float StartTime;
    public float StopTime;
    public bool TimerStarted = false;
    [SerializeField] TMP_Text TimerText;
    Coroutine TimerCoroutine;

    IEnumerator Timer()
    {
        while (true)
        {
            TimerText.text = "" + (int)(Time.time - StartTime);
            yield return null;
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    public void StartTimer()
    {
        if (TimerStarted)
        {
            return;
        }
        StartTime = Time.time;
        TimerStarted = true;
        TimerCoroutine = StartCoroutine(Timer());
    }
    public void StopTimer()
    {
        if (TimerCoroutine==null)
        {
            return;
        }
        StopTime = Time.time;
        StopCoroutine(TimerCoroutine);
        TimerCoroutine = null;
        TimerText.text = "" + (int)(StopTime - StartTime);
    }
    public void ResetTimer()
    {
        StopTimer();
        TimerText.text = "0";
        TimerStarted = false; 
        foreach (Ring r in FindObjectsOfType<Ring>())
        {
            r.CheckedRing = false;
        }
    }
}
