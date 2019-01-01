using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreStickSingle
{
    public GameObject obj;
    public int score;

    public ScoreStickSingle(GameObject obj,int score)
    {
        this.obj = obj;
        this.score = score;
    }
}

public class ScoreSticks : MonoBehaviour
{
    static public ScoreStickSingle[] scoreSticks = new ScoreStickSingle[13 * 4];

    private int[] ids = 
        {0, 0,  1,0,1,2,3,
        0,0,1,2,3,4};
    private int[] scores=
        {10000, 5000, 5000,1000 ,1000 ,1000 ,1000 ,
        500 ,100,100 ,100,100 ,100 };

    // Use this for initialization
    void Start()
    {
        for(int p = 0; p < 4; p++)
        {
            for(int i = 0; i < 13; i++)
            {
                scoreSticks[p * 13 + i] = new ScoreStickSingle(
                    AddEventTrigger(GameObject.Find("Table/StickBoxes/StickBox" + (p + 1) +
                    "/ScoreSticks/Score" + scores[i] + " (" + ids[i] + ")")), scores[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private GameObject AddEventTrigger(GameObject obj)
    {
        EventTrigger eventTrigger = obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((x) => OnClick(obj));

        eventTrigger.triggers.Add(entry);

        return obj;
    }

    public void OnClick(GameObject obj)
    {
        Hand.selectingObject = obj;
    }
}
