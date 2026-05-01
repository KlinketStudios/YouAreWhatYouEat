using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    public int currentDay;

    private void LateUpdate()
    {
        if (SaveSystem.instance.gameData.currentDay != currentDay)
        {
            SaveSystem.instance.gameData.currentDay = currentDay;
        }
    }
}
