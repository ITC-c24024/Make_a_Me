using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_RuleUIManagerScript : UIManagerScript
{
    [SerializeField]
    AudioManager audioManager;

    [SerializeField, Header("TopicのImage")]
    Image[] topicImage;

    [SerializeField, Header("Topicの説明文")]
    Image[] sentenceImage;

    [SerializeField, Header("Topicの説明動画")]
    GameObject[] movieImage;

    [SerializeField, Header("ExitのImage")]
    Image exitImage;

    [SerializeField, Header("タイトルUIスクリプト")]
    TitleUIManagerScript titleUISC;

    //選択中のUIの番号
    int selectNum;
    void Start()
    {
        
    }

    void Update()
    {
        Vector2 stickMove = selectAction.ReadValue<Vector2>();

        if (!isCoolTime)
        {
            if (stickMove.y > 0.2f) ChangeSelect(-1); // 上
            if (stickMove.y < -0.2f) ChangeSelect(1); // 下
        }

        if (decisionAction.triggered && selectNum == topicImage.Length)
        {
            audioManager.Dicide();

            titleUISC.SelectOK();
            gameObject.SetActive(false);
        }
    }

    void ChangeSelect(int direction)
    {
        audioManager.Select();

        // 現在の選択をOFF
        if (selectNum < topicImage.Length)
        {
            topicImage[selectNum].enabled = false;
            sentenceImage[selectNum].enabled = false;
            movieImage[selectNum].SetActive(false);
        }
        else
        {
            exitImage.enabled = false;
        }

        // 移動
        selectNum += direction;

        if (selectNum < 0) selectNum = topicImage.Length;            
        if (selectNum > topicImage.Length) selectNum = 0;           

        // 新しい選択をON
        if (selectNum < topicImage.Length)
        {
            topicImage[selectNum].enabled = true;
            sentenceImage[selectNum].enabled = true;
            movieImage[selectNum].SetActive(true);
        }
        else
        {
            exitImage.enabled = true;
        }

        StartCoroutine(SelectCoolTime());
    }

}
