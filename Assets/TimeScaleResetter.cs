using UnityEngine;

public class TimeScaleResetter : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
    }
}