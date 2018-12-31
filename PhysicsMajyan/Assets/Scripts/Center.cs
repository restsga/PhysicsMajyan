using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Center : MonoBehaviour
{
    //中央の移動速度
    public float speed_center= 0.01f;
    //中央の最高高度
    public float maxHeight_center=0.1f;
    //中央の初期高度
    private float firstHeight_center;
    //中央の跳ね用高度(空中停止防止)
    public float jumpHeight_center = 0.02f;

    //吸い込みの力
    public float holeForce = 0.1f;

    //エレベーターの移動速度
    public float speed_elevator = 0.01f;
    //エレベーターの最低高度
    public float minHeight_elevator = -0.1f;
    //エレベーターの初期高度
    public float firstHeight_elevator;
    //エレベーターの緩やか停止係数
    public float minSpeed_elevator = 1.1f;
    //エレベーターの上昇速度係数
    public float speedMaltiple_elevator = 1.1f;
    //エレベーターの停止時間
    public float stopTime_elevator = 0.25f;

    //エレベーター
    private GameObject[] elevators=new GameObject[4];
    //エレベーターの停止経過時間
    private float timer_elevator=-0.1f;

    //次の牌を既定の位置に移動するための時間
    public float waitTime = 5f;
    //次の牌を既定の位置に移動するタイマー
    private float waitTimer = -0.1f;

    //状態
    private const int NULL = -1, CLOSE = 0,
        CENTER_UP = 1, CENTER_HOLE = 2,CENTER_JUMP=3, CENTER_DOWN = 4,
        ELEVATOR_DOWN = 12, ELEVATOR_SETCARDS = 13, ELEVATOR_UP = 14,
        WAIT=21;
    private int condition;

    //処理の分割量
    public int processDivide = 10;
    //処理分割用カウンタ
    private int processCounter = 0;

    // Use this for initialization
    void Start()
    {
        firstHeight_center = this.transform.position.y;
        Enumerable.Range(0,4).ToList().ForEach(i=>elevators[i] = GameObject.Find("Table/TopBoards/Elevator/Cube"+(i+1)));
        firstHeight_elevator = elevators[0].transform.position.y;
        condition = CLOSE;
    }

    // Update is called once per frame
    void Update()
    {
        float height_center = this.transform.position.y;
        float height_elevator = elevators[0].transform.position.y;

        switch (condition)
        {
            case CENTER_UP:
                if (height_center >= maxHeight_center)
                {
                    condition = CENTER_HOLE;
                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                {
                    this.GetComponent<Rigidbody>().velocity = Vector3.up * speed_center;
                }
                break;

            case CENTER_HOLE:
                int allCardsCount = Cards.cards.Length;
                GameObject targetCard;
                Vector3 targetCardPos;
                for (int i = processCounter;
                    i < Math.Min(processCounter+ (int)(allCardsCount/processDivide),allCardsCount);
                    i++)
                {
                    targetCard = Cards.cards[i];
                    targetCardPos = targetCard.transform.position;

                    if (Shuffle.InvalidArea(targetCardPos) == false)
                    {
                        Vector3 direction = -new Vector3(targetCardPos.x, 0, targetCardPos.z);

                        targetCard.GetComponent<Rigidbody>().AddForce(direction * holeForce);
                    }
                }
                processCounter += allCardsCount / processDivide;
                if (processCounter >= allCardsCount)
                {
                    processCounter = 0;
                }
                break;
            case CENTER_JUMP:
                if (height_center >= maxHeight_center + jumpHeight_center)
                {
                    condition = CENTER_DOWN;
                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                {
                    this.GetComponent<Rigidbody>().velocity = Vector3.up * speed_center;
                }
                break;

            case CENTER_DOWN:
                if (height_center <= firstHeight_center)
                {
                    condition = ELEVATOR_DOWN;
                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
                else
                {
                    this.GetComponent<Rigidbody>().velocity = Vector3.down * speed_center;
                }
                break;

            case ELEVATOR_DOWN:
                if (height_elevator <= minHeight_elevator)
                {
                    condition = ELEVATOR_SETCARDS;
                    timer_elevator= stopTime_elevator;
                    Enumerable.Range(0,4).ToList().ForEach(
                        i=>elevators[i].GetComponent<Rigidbody>().velocity = Vector3.zero);
                }
                else
                {
                    Enumerable.Range(0, 4).ToList().ForEach(
                        i=>elevators[i].GetComponent<Rigidbody>().velocity = Vector3.down * speed_elevator);
                }
                break;

            case ELEVATOR_SETCARDS:
                if (timer_elevator<=0f)
                {
                    GameObject.Find("Table").GetComponent<Shuffle>().SetCards();

                    condition = ELEVATOR_UP;
                }
                else
                {
                    timer_elevator -= Time.deltaTime;
                }
                break;

            case ELEVATOR_UP:
                if (height_elevator >= firstHeight_elevator)
                {
                    condition = WAIT;
                    Enumerable.Range(0, 4).ToList().ForEach(
                        i=>elevators[i].GetComponent<Rigidbody>().velocity = Vector3.zero);

                    waitTimer = waitTime;
                    //11:OnlyClick(Layer)
                    GameObject.Find("Table/Box/Bottom1").layer = 11;
                }
                else
                {
                    Enumerable.Range(0, 4).ToList().ForEach(
                        i=>elevators[i].GetComponent<Rigidbody>().velocity =
                        Vector3.up * 
                        Mathf.Max(speed_elevator * (firstHeight_elevator - height_elevator)*speedMaltiple_elevator,
                        minSpeed_elevator));
                }
                break;

            case WAIT:
                if (waitTimer <= 0f)
                {
                    condition = CLOSE;

                    //9:Box(Layer)
                    GameObject.Find("Table/Box/Bottom1").layer = 9;
                }
                else
                {
                    waitTimer -= Time.deltaTime;
                }
                break;
        }

    }

    public void NextButton()
    {
        switch (condition)
        {
            case CLOSE:
                condition = CENTER_UP;
                break;
            case CENTER_HOLE:
                condition = CENTER_JUMP;
                break;
        }
    }
}
