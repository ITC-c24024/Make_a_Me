using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider seSlider;

    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    AudioMixerGroup bgmGrp;
    [SerializeField]
    AudioMixerGroup seGrp;

    [SerializeField,Header("タイトルBGM")]
    AudioSource titleSource;
    [SerializeField, Header("メインBGM")]
    AudioSource mainSource;
    [SerializeField, Header("シャッターSE")]
    AudioSource shutterSource;
    [SerializeField, Header("選択SE")]
    AudioSource selectSource;
    [SerializeField, Header("決定SE")]
    AudioSource dicideSource;
    [SerializeField, Header("作業SE")]
    AudioSource workSource;
    [SerializeField, Header("放電SE")]
    AudioSource dischargeSource;
    [SerializeField, Header("レベルアップSE")]
    AudioSource levelUpSource;
    [SerializeField, Header("レベルダウンSE")]
    AudioSource levelDownSource;
    [SerializeField, Header("警告SE")]
    AudioSource warningSource;
    [SerializeField, Header("発射SE")]
    AudioSource shootSource;
    [SerializeField, Header("リザルトBGM")]
    AudioSource resultSource;
    [SerializeField, Header("順位表示SE")]
    AudioSource rankSource;

    [SerializeField, Header("タイトルBGM")]
    AudioClip titleClip;
    [SerializeField, Header("メインBGM")]
    AudioClip mainClip;
    [SerializeField, Header("シャッターSE")]
    AudioClip shutterClip;
    [SerializeField, Header("選択SE")]
    AudioClip selectClip;
    [SerializeField, Header("決定SE")]
    AudioClip dicideClip;
    [SerializeField, Header("作業SE")]
    AudioClip workClip;
    [SerializeField, Header("放電SE")]
    AudioClip dischargeClip;
    [SerializeField, Header("レベルアップSE")]
    AudioClip levelUpClip;
    [SerializeField, Header("レベルダウンSE")]
    AudioClip levelDownClip;
    [SerializeField, Header("警告SE")]
    AudioClip warningClip;
    [SerializeField, Header("発射SE")]
    AudioClip shootClip;
    [SerializeField, Header("リザルトBGM")]
    AudioClip resultClip;
    [SerializeField, Header("順位表示SE")]
    AudioClip rankClip;

    void Start()
    {
        titleSource.outputAudioMixerGroup = bgmGrp;
        mainSource.outputAudioMixerGroup = bgmGrp;
        shutterSource.outputAudioMixerGroup = seGrp;
        selectSource.outputAudioMixerGroup = seGrp;
        dicideSource.outputAudioMixerGroup = seGrp;
        workSource.outputAudioMixerGroup = seGrp;
        dischargeSource.outputAudioMixerGroup = seGrp;
        levelUpSource.outputAudioMixerGroup = seGrp;
        levelDownSource.outputAudioMixerGroup = seGrp;
        warningSource.outputAudioMixerGroup = seGrp;
        shootSource.outputAudioMixerGroup = seGrp;
        resultSource.outputAudioMixerGroup = bgmGrp;
        rankSource.outputAudioMixerGroup = seGrp;
    }

    void Update()
    {       
        if (bgmSlider != null && seSlider != null)
        {
            var bgmvalue = bgmSlider.value;
            var seValue = seSlider.value;
            audioMixer.SetFloat("Param_BGM", bgmvalue);
            audioMixer.SetFloat("Param_SE", seValue);
        }
    }

    public void Title()
    {
        titleSource.PlayOneShot(titleClip);
    }
    public void TitleStop()
    {
        titleSource.Stop();
    } 
    public void Main()
    {
        mainSource.PlayOneShot(mainClip);
    }
    public void MainStop()
    {
        mainSource.Stop();
    }
    public void MainSpeedUp()
    {
        mainSource.pitch = 1.25f;
    }
    public void Shutter()
    {
        shutterSource.PlayOneShot(shutterClip);
    }
    public void Select()
    {
        selectSource.PlayOneShot(selectClip);
    }
    public void Dicide()
    {
        dicideSource.PlayOneShot(dicideClip);
    }
    public void Work()
    {
        workSource.PlayOneShot(workClip);
    }
    public void Discharge()
    {
        dischargeSource.PlayOneShot(dischargeClip);
    }
    public void LevelUp()
    {
        levelUpSource.PlayOneShot(levelUpClip);
    }
    public void LevelDown()
    {
        levelDownSource.PlayOneShot(levelDownClip);
    }
    public void Warning()
    {
        warningSource.PlayOneShot(warningClip);
    }
    public void Shoot()
    {
        shootSource.PlayOneShot(shootClip);
    }
    public void Result()
    {
        resultSource.PlayOneShot(resultClip);
    }
    public void ResultStop()
    {
        resultSource.Stop();
    }

    public void Rank()
    {
        rankSource.PlayOneShot(rankClip);
    }
}