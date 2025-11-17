using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]

public class PoseScript : MonoBehaviour
{
    [SerializeField]
    GameController gameController;
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    ShutterScript shutterScript;
    [SerializeField, Header("ポーズ画面")]
    GameObject poseImage;

    [SerializeField, Header("ポーズUI")]
    GameObject[] poseUI;

    [SerializeField, Header("スコアマネージャースクリプト")]
    ScoreManager scoreManaSC;

    //UI切り替え変数
    private int uiNum = 0;


    float maxScale = 1.1f;

    float maxTime = 1f;


    //ポーズ画面アクション
    private InputAction stickAction, decideAction, poseAction;

    private AnimationCurve animationCurve;

    private Coroutine currentCoroutine;

    void Start()
    {
        //プレイヤーのActionMapを取得
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;

        stickAction = actionMap["Move"];
        decideAction = actionMap["Throw"];
        poseAction = input.actions["Pose"];

        animationCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 0f));

        poseUI[uiNum].SetActive(true);
        //StartAnimationForScene();

        scoreManaSC = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    void Update()
    {
        //ポーズ画面に移動
        var poseAct = poseAction.triggered;

        if (poseAct && gameController.isOpen)
        {
            audioManager.Dicide();

            if (Time.timeScale == 1)
            {
                //動けなくする
                Time.timeScale = 0;
                poseImage.SetActive(true);
            }
            else
            {
                poseImage.SetActive(false);
                Time.timeScale = 1;
            }
            
        }


        //決定
        var selectAct = decideAction.triggered;

        if (selectAct && Time.timeScale == 0)
        {
            audioManager.Dicide();
            switch (uiNum)
            {
                case 0:
                    Invoke("DeletePanel", 0.3f);

                    //動けるようにする
                    Time.timeScale = 1;
                    break;
                case 1:
                    //動けるようにする
                    Time.timeScale = 1;
                    StartCoroutine(shutterScript.CloseShutter());
                    scoreManaSC.ResetScores();
                    Invoke("SelectTitle", 2.5f);
                    break;
            }
        }


        //UI切り替え
        var stickAct = stickAction.ReadValue<Vector2>().y;

        if (stickAct > 0 && Time.timeScale == 0 && uiNum != 0)
        {
            poseUI[uiNum].SetActive(false);

            uiNum = 0;
            audioManager.Select();

            StartCoroutine(BounceUI(poseUI[uiNum].transform, 0.3f));

            poseUI[uiNum].SetActive(true);
        }

        if (stickAct < 0 && Time.timeScale == 0 && uiNum != 1)
        {
            poseUI[uiNum].SetActive(false);

            uiNum = 1;
            audioManager.Select();

            StartCoroutine(BounceUI(poseUI[uiNum].transform, 0.3f));

            poseUI[uiNum].SetActive(true);
        }
    }

    private void DeletePanel()
    {
        poseImage.SetActive(false);
    }

    private void SelectTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator BounceUI(Transform target, float time)
    {
        float t = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / time;
            float s = 2f;
            float curved = 1f + s * Mathf.Pow(t - 1f, 3) + s * Mathf.Pow(t - 1f, 2);
            target.localScale = Vector3.LerpUnclamped(startScale, endScale, curved);
            yield return null;
        }
        target.localScale = Vector3.one;
    }
}
