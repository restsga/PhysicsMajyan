using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    public float offsetWidth = 0.233f;
    public float offsetHeight = -0.06f;
    public float offsetDepth = 0.230f;
    public float lineupWidth = 0.02f;
    public float lineupHeight = 0.02f;

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
        List<uint> randomNumbers = Enumerable.Range(0, 136*2).Select(i => XOR128.Random()).ToList();
        int[] randomIndexes=Enumerable.Range(0,136*2).OrderBy(i => randomNumbers[i]).ToArray();

        //配置
        int setCount = 0;
        GameObject targetCard;
        for(int i = 0; i < Cards.cards.Length; i++)
        {
            targetCard = Cards.cards[randomIndexes[i]];

            if (InvalidArea(targetCard.transform.position))
            {
                Vector3 pos = new Vector3();
                pos.x = -offsetWidth + lineupWidth * (setCount / 8);
                pos.y = offsetHeight + lineupHeight * ((setCount % 8) / 4);
                pos.z = -offsetDepth;
                targetCard.transform.position = RotateAroundY(pos,90*(setCount % 4));
                targetCard.transform.rotation = Quaternion.Euler(-90, -90 * (setCount % 4) + XOR128.Next(2) * 180, 0);

                setCount++;
            }
        }
    }

    private Vector3 RotateAroundY(Vector3 pos,float angle)
    {
        float x = pos.x,z=pos.z;
        float a = Mathf.PI * angle / 180.0f;
        pos.x = x * Mathf.Cos(a) - z * Mathf.Sin(a);
        pos.z = x * Mathf.Sin(a) + z * Mathf.Cos(a);

        return pos;
    }

    static public bool InvalidArea(Vector3 pos)
    {
        return Mathf.Abs(pos.x) <= 0.365f &&
                -0.5 <= pos.y && pos.y <= -0.25 &&
                Mathf.Abs(pos.z) <= 0.365f;
    }
}
