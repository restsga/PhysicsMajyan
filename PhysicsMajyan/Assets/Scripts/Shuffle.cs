using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCards()
    {
        //シャッフル
        List<uint> randomId = Enumerable.Range(0, 136*2).Select(i => XOR128.Random()).ToList();
        Cards.cards=Enumerable.Range(0,136*2).OrderBy(i => randomId[i]).Select(i => Cards.cards[i]).ToArray();


    }
}
