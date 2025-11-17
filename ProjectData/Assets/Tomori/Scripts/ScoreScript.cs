using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    [SerializeField]
    GameController gameController;
    [SerializeField]
    AudioManager audioManager;
    [SerializeField]
    EnergyScript energyScript;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    ScoreManager scoreManager;
    [SerializeField]
    LeftConveyorScript leftConveyorScript;

    [SerializeField, Header("クローンオブジェクト")]
    GameObject[] clones;
    [SerializeField]
    GameObject player;
    [SerializeField, Header("ハンマーオブジェクト")]
    GameObject hammer;
    [SerializeField, Header("ハンマー追従オブジェクト")]
    GameObject follow;

    [SerializeField, Header("スコアUI")]
    Image[] scoreImage;
    [SerializeField, Header("数字UI")]
    Sprite[] numSprite;
    [SerializeField, Header("進捗スライダー")]
    Slider slider;

    public float maxTime = 10f;
    [SerializeField] float workTime = 0f;
    [SerializeField] float progress = 0;
    [SerializeField, Header("作業クールタイム")]
    float coolTime = 1.0f;
    //作業効率
    private float[] efficiency = new float[] { 2f, 3.5f, 5f, 7f, 10f };

    [SerializeField] int score = 0;
    public bool isWork = false;
    public bool isArea = false;
    bool isMove = false;

    [SerializeField] int workAreaNum = 0;

    float lookTime = 0;

    [SerializeField, Header("クローンのスタートZ座標")]
    float startPos;
    [SerializeField, Header("クローンの停止Z座標")]
    float stopPos;

    [SerializeField, Header("完成クローンのAnimator")]
    Animator animator;

    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        StartCoroutine(MoveMaterial());
    }

    void Update()
    {
        bool lookforward = Vector3.Angle(
            player.transform.forward,
             clones[0].transform.position - player.transform.position
            ) <= 60;

        if (lookforward && !isMove && isArea && !gameController.isFinish && !playerController.isStun)
        {
            if (playerController.haveBattery)
            {
                playerController.Drop();
            }
            //一瞬向いただけで仕事できないようにする
            lookTime += Time.deltaTime;
            if (lookTime >= 0.5f)
            {
                lookTime = 0;

                isWork = true;
                playerController.JobAnim(true);
            }
        }
        else
        {
            lookTime = 0;
            isWork = false;
            playerController.JobAnim(false);
            hammer.transform.localPosition = new Vector3(0, 0, 0);
            hammer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (isWork)
        {
            hammer.transform.position = follow.transform.position;
            hammer.transform.rotation = follow.transform.rotation;
        }
        
        if (isWork)
        {
            workTime += Time.deltaTime * efficiency[energyScript.level - 1];

            slider.gameObject.SetActive(true);
            slider.value = Mathf.Lerp(0, 1, workTime / maxTime);
        }      
        if (workTime >= maxTime / 2) //二段階
        {
            clones[0].SetActive(false);
            clones[1].SetActive(true);
        }
        if (workTime >= maxTime) //完成
        {
            clones[1].SetActive(false);
            clones[2].SetActive(true);
            //animator.SetBool("IsYell", true);

            isWork = false;
            slider.gameObject.SetActive(false);
            playerController.JobAnim(isWork);

            workTime = 0;
            score += 1;
            scoreManager.ChangeScore(playerController.playerNum);
            SetUI();

            StartCoroutine(MoveClone());

        }
        
        if (playerController.haveBattery)
        {
            isWork = false;
        }
    }
   
    public void ChangeIsWork(bool set)
    {
        isWork = set;
    }

    /// <summary>
    /// クローン材料の移動
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveMaterial()
    {
        isMove = true;

        clones[0].SetActive(true);

        float time = 0;
        while (time < coolTime)
        {
            time += Time.deltaTime;

            float currentPos = Mathf.Lerp(startPos, stopPos, time / coolTime);
            for (int i = 0; i < clones.Length; i++)
            {
                clones[i].transform.position = new Vector3(
                clones[i].transform.position.x,
                clones[i].transform.position.y,
                currentPos
                );
            }

            yield return null;
        }
        isMove = false;

        if (isArea)
        {
            isWork = true;
            playerController.JobAnim(isWork);
        }
    }
    /// <summary>
    /// 完成クローンの移動
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveClone()
    {
        isMove = true;
        yield return new WaitForSeconds(0.2f);

        float time = 0;
        while (time < coolTime)
        {
            time += Time.deltaTime;

            float currentPos = Mathf.Lerp(stopPos, startPos, time / coolTime);
            for (int i = 0; i < clones.Length; i++)
            {
                clones[i].transform.position = new Vector3(
                clones[i].transform.position.x,
                clones[i].transform.position.y,
                currentPos
                );
            }

            yield return null;
        }
        clones[2].SetActive(false);

        StartCoroutine(MoveMaterial());
        leftConveyorScript.AddList(playerController.playerNum);
    }

    void SetUI()
    {
        int ten = score / 10; //十の位
        int one = score - 10 * ten; //一の位

        //十の位がある時,十の位が表示されていないとき
        if (ten > 0 && !scoreImage[1].enabled)
        {
            Debug.Log("hoge");
            //一の位を右にずらす
            scoreImage[0].rectTransform.anchoredPosition = new Vector2(
                scoreImage[0].rectTransform.anchoredPosition.x + 20,
                scoreImage[0].rectTransform.anchoredPosition.y
                );
            //十の位を表示
            scoreImage[1].enabled = true;
        }

        scoreImage[0].sprite = numSprite[one];
        scoreImage[1].sprite = numSprite[ten];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag($"Player{workAreaNum}"))
        {
            isArea = false;
            isWork = false;
            playerController.JobAnim(isWork);

            hammer.transform.localPosition = new Vector3(0, 0, 0);
            hammer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
