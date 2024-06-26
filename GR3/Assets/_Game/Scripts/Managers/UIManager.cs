using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //public static UIManager Instance
    //{
    //    get 
    //    {
    //        if (instance == null)
    //        {
    //            instance = FindObjectOfType<UIManager>();
    //        }

    //        return instance;
    //    }
    //}

    public GameObject menu, gameplay, finish, lose, timeOut, levelSelector;

    public TextMeshProUGUI timeRemainText;

    [SerializeField] TextMeshProUGUI coinText;

    private void Awake()
    {
        instance = this;
        OpenGameplay();
    }

    public void OpenMenu()
    {
        CloseAll();
        menu.SetActive(true);
        levelSelector.SetActive(true);
    //    GameManager.instance.isGameplay = true;
    }

    public void OpenGameplay()
    {
        CloseAll();
        gameplay.SetActive(true);
    }

    public void OpenFinish()
    {
        CloseAll();
        finish.SetActive(true);
        levelSelector.SetActive(true);
    }

    public void OpenLose()
    {
        CloseAll();
        lose.SetActive(true);
        levelSelector.SetActive(true);
    }

    public void OpenTimeOut()
    {
        CloseAll();
        timeOut.SetActive(true);
        levelSelector.SetActive(true);
    }

    private void CloseAll()
    {
        menu.SetActive(false);
        gameplay.SetActive(false);
        finish.SetActive(false);
        lose.SetActive(false);
        timeOut.SetActive(false);
        levelSelector.SetActive(false);
    }

    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString() + "/" + CoinManager.Ins.coinNumber;
    }

    public void SetTimeRemain(float timeRemain)
    {
        timeRemainText.text = Mathf.Abs(timeRemain).ToString() + "s";
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ReloadSceneByNumber(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
