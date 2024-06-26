using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool isGameplay;

    public SceneFader sceneFader;

    public string thisLevel;
    public string nextLevel;
    public string levelSelector = "LevelSelector";

    public int levelToUnlock = 2;

    public void Awake()
    {
        instance = this;
        isGameplay = true;
        Time.timeScale = 1.0f;
    }

    public void RestartLevel()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
        sceneFader.FadeTo(thisLevel);
    }

    public void NextLevel()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
        PlayerPrefs.SetInt("levelReached", levelToUnlock);
        sceneFader.FadeTo(nextLevel);
    }

    public void GoToLevelSelector()
    {
        AudioManager.Ins.PlaySFX(AudioManager.Ins.click);
        sceneFader.FadeTo(levelSelector);
    }


}
