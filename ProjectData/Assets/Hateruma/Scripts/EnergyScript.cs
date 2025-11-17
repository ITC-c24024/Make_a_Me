using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnergyScript : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;

    [SerializeField, Header("プレイヤー番号")]
    int playerNum;

    // レベル
    public int level = 1;

    // 最大レベル
    int maxLevel = 5;

    [SerializeField, Header("総取得エネルギー")]
    int allEnergyAmount;

    [SerializeField, Header("レベルごとの取得エネルギ―量")]
    int energyAmount;

    //レベルアップに必要なエネルギー量
    int[] requireEnergy = { 50, 75, 100, 125, 150 };

    //レベルに応じてのドロップ割合
    float[] dropRate = { 0.2f, 0.3f, 0.5f, 0.7f, 0.8f };

    [SerializeField, Header("チャージ中")]
    bool isCharge;

    // チャージ用タイマー
    float timer = 0;

    // ドロップマネージャースクリプト
    DropEnergyManagerScript dropManagerSC;

    [SerializeField, Header("エネルギー量表示用UI")]
    GameObject energyUIObj;

    [SerializeField, Header("エネルギー量表示用スライダー")]
    Slider[] energySlider;

    [SerializeField, Header("ハンマーイメージ")]
    Image hammerImage;

    [SerializeField, Header("ハンマーのスプライト")]
    Sprite[] hammerSprite;

    [SerializeField, Header("レベルのImage")]
    Image[] levelImage;

    [SerializeField, Header("レベルのSprite(非常時)")]
    Sprite[] levelSprite1;

    [SerializeField, Header("レベルのSprite(常時)")]
    Sprite[] levelSprite2;

    [SerializeField, Header("スライダー表示時間(秒)")]
    float sliderDisplayTime = 3f;

    // スライダー表示コルーチン用
    Coroutine showRoutine;

    // UIの座標
    Transform uiPos;

    // カメラ
    Camera mainCam;

    [SerializeField, Header("ハンマーオブジェクト")]
    GameObject[] hammerObj;

    [SerializeField, Header("ハンマーの親オブジェクト")]
    GameObject hammerMasterObj;

    private void Start()
    {
        dropManagerSC = gameObject.GetComponent<DropEnergyManagerScript>();

        energySlider[0].maxValue = requireEnergy[0];
        energySlider[1].maxValue = requireEnergy[0];

        uiPos = energyUIObj.transform;

        mainCam = Camera.main;
    }

    void Update()
    {
        if (isCharge)
        {
            // 1秒ごとに20ずつチャージされる
            timer += Time.deltaTime * 10;
            if (timer >= 1f)
            {
                ChargeEnergy(1);
                timer = 0f;
            }
        }

        if (uiPos != null)
        {
            // プレイヤーの頭上に追従
            Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position + new Vector3(0, 1f, 2));
            uiPos.position = screenPos;
        }
    }

    /// <summary>
    /// エネルギーのチャージ
    /// </summary>
    public void ChargeEnergy(int amount)
    {

        if (allEnergyAmount < 500)
        {
            allEnergyAmount += amount;
        }

        while (amount + energyAmount >= requireEnergy[level - 1])
        {
            amount -= requireEnergy[level - 1] - energyAmount;

            if (level != maxLevel)
            {
                LevelUp();
            }
            else
            {
                // レベル最大ならゲージを埋め切って終了
                energyAmount = requireEnergy[level - 1];
                energySlider[0].maxValue = requireEnergy[level - 1];
                energySlider[1].maxValue = requireEnergy[level - 1];
                energySlider[0].value = energyAmount;
                energySlider[1].value = energyAmount;

                amount = 0;
                break;
            }

        }


        if (amount > 0)
        {
            energyAmount += amount;
            energySlider[0].value = energyAmount;
            energySlider[1].value = energyAmount;
        }

        ShowSlider();
    }

    /// <summary>
    /// エネルギーのドロップ(総量の1/3)
    /// </summary>
    public void LostEnergy()
    {
        if (allEnergyAmount <= 0) return;

        // 総量のうち、レベルに応じてエネルギーをドロップ
        int amount = Mathf.CeilToInt(allEnergyAmount * dropRate[level-1]);
        dropManagerSC.Drop(amount);
        allEnergyAmount -= amount;

        while (amount > 0)
        {
            if (energyAmount >= amount)
            {
                // 今のレベルで減らしきれる
                energyAmount -= amount;
                amount = 0;
            }
            else
            {
                // 今のレベルのゲージを全部使い切る
                amount -= energyAmount;
                energyAmount = 0;

                // まだ減らす分が残っていて、レベル > 1 ならダウン
                if (level > 1)
                {
                    LevelDown();
                    // レベルダウンしたら次のゲージは満タンからスタート
                    energyAmount = requireEnergy[level - 1];
                }
                else
                {
                    // レベル1で削り切ったらゼロ
                    amount = 0;
                }
            }
        }

        // スライダー更新
        energySlider[0].maxValue = requireEnergy[Mathf.Clamp(level - 1, 0, requireEnergy.Length - 1)];
        energySlider[1].maxValue = energySlider[0].maxValue;
        energySlider[0].value = energyAmount;
        energySlider[1].value = energyAmount;

        ShowSlider();
    }



    /// <summary>
    /// レベルアップ用
    /// </summary>
    void LevelUp()
    {
        audioManager.LevelUp();

        level++;
        level = Mathf.Clamp(level, 1, maxLevel);

        hammerObj[level - 2].SetActive(false);
        hammerObj[level - 1].SetActive(true);
        hammerImage.sprite = hammerSprite[level - 1];

        StartCoroutine(BounceObject(hammerMasterObj.transform, 0.3f));
        StartCoroutine(BounceObject(levelImage[0].transform, 0.3f));

        energyAmount = 0;

        energySlider[0].maxValue = requireEnergy[level - 1];
        energySlider[1].maxValue = requireEnergy[level - 1];
        energySlider[0].value = 0;
        energySlider[1].value = 0;

        levelImage[0].sprite = levelSprite1[level - 1];
        levelImage[1].sprite = levelSprite2[level - 1];
    }

    /// <summary>
    /// レベルダウン用
    /// </summary>
    void LevelDown()
    {
        level--;
        level = Mathf.Clamp(level, 1, maxLevel);

        hammerObj[level].SetActive(false);
        hammerObj[level - 1].SetActive(true);
        hammerImage.sprite = hammerSprite[level - 1];

        StartCoroutine(BounceObject(hammerMasterObj.transform, 0.3f));
        StartCoroutine(BounceObject(levelImage[0].transform, 0.3f));

        energySlider[0].maxValue = requireEnergy[level - 1];
        energySlider[1].maxValue = requireEnergy[level - 1];
        energySlider[0].value = energyAmount;
        energySlider[1].value = energyAmount;

        levelImage[0].sprite = levelSprite1[level - 1];
        levelImage[1].sprite = levelSprite2[level - 1];
    }

    IEnumerator BounceObject(Transform target, float time)
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





    /// <summary>
    /// エネルギーチャージ中かどうかの切り替え用
    /// </summary>
    public void ChargeSwitch(bool charge)
    {
        isCharge = charge;
    }

    void ShowSlider()
    {
        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
            showRoutine = null;
        }

        showRoutine = StartCoroutine(ShowSliderRoutine());
    }

    IEnumerator ShowSliderRoutine()
    {
        energyUIObj.gameObject.SetActive(true);

        yield return new WaitForSeconds(sliderDisplayTime);

        energyUIObj.gameObject.SetActive(false);
        showRoutine = null;
    }
}

