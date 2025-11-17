using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUIScript : UIManagerScript
{

    [SerializeField, Header("タイトルのボタンImage")]
    Image[] buttonImage;

    [SerializeField]
    ShutterScript shutterScript;

    int selectNum;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField, Header("スコアマネージャースクリプト")]
    ScoreManager scoreManaSC;
    void Start()
    {
        scoreManaSC = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    void Update()
    {
        Vector2 stickMove = selectAction.ReadValue<Vector2>();

        if (!isCoolTime)
        {
            if (stickMove.x > 0.2f) ChangeSelect(-1); // 上
            if (stickMove.x < -0.2f) ChangeSelect(1); // 下

            if (decisionAction.triggered)
            {
                Decision();
            }
        }
    }

    void ChangeSelect(int direction)
    {
        audioManager.Select();

        // 現在の選択をOFF
        if (selectNum < buttonImage.Length)
        {
            buttonImage[selectNum].enabled = false;
        }

        // 移動
        selectNum += direction;

        if (selectNum < 0) selectNum = buttonImage.Length - 1;
        if (selectNum > buttonImage.Length - 1) selectNum = 0;

        // 新しい選択をON
        if (selectNum < buttonImage.Length)
        {
            buttonImage[selectNum].enabled = true;
            StartCoroutine(BounceUI(buttonImage[selectNum].transform, 0.3f));
        }

        StartCoroutine(SelectCoolTime());
    }

    IEnumerator BounceUI(Transform target, float time)
    {
        float t = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (t < 1f)
        {
            t += Time.deltaTime / time;

            float s = 2f;
            float curved = 1f + s * Mathf.Pow(t - 1f, 3) + s * Mathf.Pow(t - 1f, 2);

            target.localScale = Vector3.LerpUnclamped(startScale, endScale, curved);
            yield return null;
        }

        target.localScale = Vector3.one;
    }

    void Decision()
    {
        audioManager.Dicide();

        switch (selectNum)
        {
            case 0:
                audioManager.ResultStop();
                StartCoroutine(shutterScript.CloseShutter());
                // スコアをリセット
                ScoreManager.Instance.ResetScores();
                Invoke(nameof(MainScene), 2.5f);
                break;

            case 1:
                audioManager.ResultStop();
                StartCoroutine(shutterScript.CloseShutter());
                // スコアをリセット
                ScoreManager.Instance.ResetScores();
                Invoke(nameof(TitleScene), 2.5f);
                break;
        }
    }

    void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    void TitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
