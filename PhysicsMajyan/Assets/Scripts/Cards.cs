using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cards : MonoBehaviour {

    //牌
    static public GameObject[] cards = new GameObject[136 * 2];

    // Use this for initialization
    void Start()
    {
        //牌のGameObjectを取得しつつEventTriggerを登録
        string[] color_str = { "Blue", "Orange" };
        Enumerable.Range(0, 136 * 2).ToList().
            ForEach(i => cards[i] =AddEventTrigger(GameObject.Find
            ("CardsManager/Cards" + color_str[i / 136] + "/Card (" + (i % 136) + ")")));
    }

    // Update is called once per frame
    void Update () {
		
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
