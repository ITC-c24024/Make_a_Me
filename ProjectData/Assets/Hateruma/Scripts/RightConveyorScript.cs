using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightConveyorScript : SideConveyorScript
{
    List<ScrollObjScript> unUsedCloneSC = new List<ScrollObjScript>(12);

    List<ScrollObjScript> usedCloneSC = new List<ScrollObjScript>();

    bool isScroll;

    void Start()
    {
        foreach (var sc in cloneSC)
        {
            unUsedCloneSC.Add(sc);
        }

        foreach (var sc in unUsedCloneSC)
        {
            sc.conveyorSC = this;
        }

        isScroll = true;
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        while (isScroll)
        {
            var clone = unUsedCloneSC[Random.Range(0, unUsedCloneSC.Count)];

            unUsedCloneSC.Remove(clone);
            usedCloneSC.Add(clone);

            StartCoroutine(clone.Move(startPos, finishPos, scrollSpeed));

            yield return new WaitForSeconds(1f);
        }
    }

    public override void AddClone(ScrollObjScript scrollObj,int playerNum)
    {
        unUsedCloneSC.Add(scrollObj);
        usedCloneSC.Remove(scrollObj);
    }
}
