using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftConveyorScript : SideConveyorScript
{
    List<ScrollObjScript>[] cloneNum_SC = new List<ScrollObjScript>[4];

    List<ScrollObjScript> usedCloneSC = new List<ScrollObjScript>();
    
    List<int> orderList = new();

    void Awake()
    {
        for (int i = 0; i < cloneNum_SC.Length; i++)
        {
            cloneNum_SC[i] = new List<ScrollObjScript>();
        }
    }

    void Start()
    {
        foreach (var sc in cloneSC)
        {
            sc.conveyorSC = this;
            cloneNum_SC[sc.playerNum - 1].Add(sc);
        }

        StartCoroutine(Scroll());
    }

    public void AddList(int playerNum)
    {
        orderList.Add(playerNum);
    }

    IEnumerator Scroll()
    {
        while (true)
        {
            if (orderList.Count > 0)
            {
                var playerNum = orderList[0];
                orderList.RemoveAt(0);

                if (cloneNum_SC[playerNum - 1].Count > 0)
                {
                    var clone = cloneNum_SC[playerNum - 1][0];
                    cloneNum_SC[playerNum - 1].Remove(clone);
                    usedCloneSC.Add(clone);

                    StartCoroutine(clone.Move(startPos, finishPos, scrollSpeed));
                }

                yield return new WaitForSeconds(1);
            }
            yield return null;
        }
    }

    public override void AddClone(ScrollObjScript scrollObj, int playerNum)
    {
        cloneNum_SC[playerNum - 1].Add(scrollObj);
        usedCloneSC.Remove(scrollObj);
    }
}

