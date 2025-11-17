using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    GameController gameController;
    CountDownScript countDownScript;

    [SerializeField,Header("タイマーUIの親")]
    GameObject timerObj;
    [SerializeField, Header("中心オブジェクト")]
    GameObject center;
    [SerializeField, Header("歯車外側")]
    Image gearOut;
    [SerializeField, Header("歯車内側")]
    Image gearIn;
    [SerializeField, Header("経過時間スライダー")]
    Slider slider;

    [SerializeField, Header("ゲーム時間(秒)")]
    float limitTime = 180;

    bool isNotice = false;
    bool isCount = false;

    void Start()
    {
        gameController = GetComponent<GameController>();
        countDownScript = GetComponent<CountDownScript>();
    }

    /// <summary>
    /// スライダー制御
    /// </summary>
    /// <returns></returns>
    public IEnumerator Timer()
    {
        float currentTime = 0;
        while (currentTime < limitTime)
        {
            currentTime += Time.deltaTime;
            if (currentTime > limitTime * 5 / 6 && !isNotice)
            {
                isNotice = true;
                StartCoroutine(Notice());
                StartCoroutine(gameController.Notice());
            }
            else if (currentTime >= limitTime - 3 && !isCount)
            {
                isCount = true;
                countDownScript.CountSatrt();
            }

            gearOut.rectTransform.localEulerAngles += new Vector3(0, 0, -1);
            gearIn.rectTransform.localEulerAngles += new Vector3(0, 0, 4);

            //針を回転
            float rotationZ = Mathf.Lerp(0, -360, currentTime / limitTime);
            center.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            //経過時間スライダーを進める
            float value= Mathf.Lerp(0, 1, currentTime / limitTime);
            slider.value = value;

            yield return null;
        }
        isNotice = false;
        StartCoroutine(gameController.GameFinish());
    }
    /// <summary>
    /// タイマーUIをもわんもわんさせる
    /// </summary>
    /// <returns></returns>
    IEnumerator Notice()
    {
        Vector3 startScale = timerObj.transform.localScale;
        float changeT = 0.5f;
        while (isNotice)
        {
            float time = 0;
            while (time < changeT)
            {
                time += Time.deltaTime;

                float rate = Mathf.Lerp(0, 3.14f, time / changeT);
                Vector3 currentScale = startScale * (1 + Mathf.Abs(Mathf.Sin(rate)) * 0.2f);
                timerObj.transform.localScale = currentScale;

                yield return null;
            }
        }  
    }
}
