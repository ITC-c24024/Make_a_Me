using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]

public class GameController : MonoBehaviour
{
    [SerializeField]
    WaitScript waitScript;
    CountDownScript countDownScript;
    [SerializeField]
    AudioManager audioManager;
    TimerScript timerScript;
    [SerializeField]
    ShutterScript shutterScript;

    public bool isOpen = false;
    public bool isStart = false;
    public bool isFinish = false;

    void Start()
    {
        Application.targetFrameRate = 60;

        countDownScript = GetComponent<CountDownScript>();
        timerScript = this.GetComponent<TimerScript>();

        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(shutterScript.OpenShutter());

        yield return new WaitForSeconds(2.5f);
        isOpen = true;
        StartCoroutine(waitScript.SetUI());
    }
    /// <summary>
    /// タイマーを開始し、動けるようにする
    /// </summary>
    public IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1.6f);
        countDownScript.enabled = true;
        yield return new WaitForSeconds(3.5f);

        StartCoroutine(timerScript.Timer());
        isStart = true;
        audioManager.Main();
    }
    /// <summary>
    /// 警告音を鳴らし、BGMを加速
    /// </summary>
    public IEnumerator Notice()
    {
        audioManager.MainStop();

        audioManager.Warning();
        yield return new WaitForSeconds(3.0f);

        audioManager.MainSpeedUp();
        audioManager.Main();
    }
    /// <summary>
    /// BGMを止め、シャッターを閉じる
    /// </summary>
    public IEnumerator GameFinish()
    {
        isFinish = true;
        audioManager.MainStop();
        StartCoroutine(shutterScript.CloseShutter());

        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("ResultScene");
    }
}
