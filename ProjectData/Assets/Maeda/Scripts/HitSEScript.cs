using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSEScript : MonoBehaviour
{
    [SerializeField]
    AudioManager audioManager;

    void HitSE()
    {
        audioManager.Work();
    }
}
