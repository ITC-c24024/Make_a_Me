using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class DispencerScript : MonoBehaviour
{
    [SerializeField, Header("開始時間")]
    float startTime;

    [SerializeField, Header("発射オブジェクト")]
    GameObject shotObj;

    [SerializeField, Header("発射口")]
    GameObject launcherObj;

    [SerializeField, Header("プレイヤー番号")]
    int playerNum;

    [SerializeField] List<GameObject> cloneObj; // SerializeField を残す
    List<Rigidbody> cloneRB;

    [SerializeField, Header("クローンの数")]
    int cloneCount;

    //発射数
    int shotCount;

    [SerializeField, Header("カウントのイメージ")]
    Image[] countImage;

    [SerializeField, Header("数字のスプライト")]
    Sprite[] numSprite;

    //スコアマネージャースクリプト
    public ScoreManager scoreManaSC;

    //リザルトマネージャースクリプト
    public ResultManagerSC resultManaSC;

    [SerializeField, Header("オーディオマネージャースクリプト")]
    AudioManager audioManaSC;
    private void Awake()
    {
        
    }


    void Start()
    {
        if (cloneObj == null) cloneObj = new List<GameObject>();
        if (cloneRB == null) cloneRB = new List<Rigidbody>();

        cloneCount = scoreManaSC.players[playerNum - 1].score;

        GameObject[] clones = GameObject.FindGameObjectsWithTag($"ResultCloneP{playerNum}");
        cloneObj.AddRange(clones);

        foreach (var obj in cloneObj)
        {
            cloneRB.Add(obj.GetComponent<Rigidbody>());
        }
        StartCoroutine(ShotClone());
    }

    IEnumerator ShotClone()
    {
        var count = 0.25f;

        yield return new WaitForSeconds(startTime);

        while (cloneCount > 0)
        {
            shotObj.transform.localEulerAngles = new Vector3(
                Random.Range(60f, 120f),
                Random.Range(-30f, 30f)
            );

            var clone = cloneObj[0];
            var rb = cloneRB[0];

            clone.transform.position = shotObj.transform.position;

            StartCoroutine(BounceObj(launcherObj.transform, 0.3f));
            audioManaSC.Shoot();

            rb.isKinematic = false;
            rb.AddForce(shotObj.transform.forward * 10, ForceMode.Impulse);

            cloneObj.RemoveAt(0);
            cloneRB.RemoveAt(0);

            cloneCount--;
            shotCount++;

            if (count > 0.1f)
            {
                count -= 0.01f;
            }

            SetUI();

            yield return new WaitForSeconds(count);
        }

        StartCoroutine(resultManaSC.ShowResultAdd());

    }

    IEnumerator BounceObj(Transform target, float time)
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

    void SetUI()
    {
        int ten = shotCount / 10; //十の位
        int one = shotCount - 10 * ten; //一の位

        //十の位がある時,十の位が表示されていないとき
        if (ten > 0 && !countImage[1].enabled)
        {
            //一の位を右にずらす
            countImage[0].rectTransform.anchoredPosition = new Vector2(
                countImage[0].rectTransform.anchoredPosition.x + 20,
                countImage[0].rectTransform.anchoredPosition.y
                );
            //十の位を表示
            countImage[1].enabled = true;
        }

        countImage[0].sprite = numSprite[one];
        countImage[1].sprite = numSprite[ten];
    }
}
