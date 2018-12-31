using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Call_Direction(Vector2 move);
public delegate void Call_Distance(float distance);

public class TouchInput : MonoBehaviour
{
    public int needHistoryCount = 5;
    public float doubleDragSpeed = 2.0f;

    static public Call_Direction Drag, DoubleDrag;
    static public Call_Distance Pinch;
    
    private List<Vector2>[] fingersPos = new List<Vector2>[2];
    private List<float>[] deltaTimes = new List<float>[2];
    private int[] fingersId = new int[2];

    // Use this for initialization
    void Start()
    {
        Enumerable.Range(0, 2).ToList().ForEach(i => fingersPos[i] = new List<Vector2>());
        Enumerable.Range(0, 2).ToList().ForEach(i => deltaTimes[i] = new List<float>());
        Enumerable.Range(0, 2).ToList().ForEach(i => fingersId[i] = -1);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < fingersPos.Length; i++)
        {
            if (fingersId[i] < 0)
            {
                LogText.SetLogMessage("fi" + Input.touchCount);

                int newTouchIndex =
                    Enumerable.Range(0, Input.touchCount).ToList().FindIndex
                    (index => fingersId.ToList().FindIndex(id => id == Input.touches[index].fingerId) < 0);
                
                LogText.AddLogMessage(""+newTouchIndex+"fids0"+fingersId[0]+"fids1"+fingersId[1]);

                if (newTouchIndex >= 0)
                {
                    fingersId[i] =
                        Input.touches[newTouchIndex].fingerId;

                    LogText.AddLogMessage("Log1");
                }
            }

            LogText.SetLogMessage("tc" + Input.touchCount);

            int touchIndex = Enumerable.Range(0, Input.touchCount).ToList().
                FindIndex(index => Input.touches[index].fingerId == fingersId[i]);
            LogText.AddLogMessage("tind" + touchIndex);

            if (touchIndex >= 0)
            {
                Vector2 pos = Input.touches[touchIndex].position;
                fingersPos[i].Insert(0, new Vector2(pos.x, pos.y));
                deltaTimes[i].Insert(0, Time.deltaTime);

                LogText.AddLogMessage("Log2");
            }
            else
            {
                fingersPos[i].Clear();
                deltaTimes[i].Clear();
                fingersId[i] = -1;

                LogText.AddLogMessage("Log3");
            }
        }
    }

    private void CheckAndCallAction()
    {
        if (fingersId.ToList().Where(id => id >= 0).Count() >= 2)
        { 
            LogText.AddLogMessage("Log4");

        if (fingersPos.ToList().Any(list => list.Count < needHistoryCount) == false)
            {
                LogText.AddLogMessage("Log5");

                float distance_before =
                    Distance_Light(fingersPos[0][needHistoryCount - 1], fingersPos[1][needHistoryCount - 1]);
                float distance_after =
                    Distance_Light(fingersPos[0][0], fingersPos[1][0]);

                float distance = distance_after - distance_before;

                if (Math.Abs(distance) <= doubleDragSpeed)
                {
                    LogText.AddLogMessage("Log6");

                    Vector2 move =
                        (fingersPos[0][0] - fingersPos[1][0]) / 2 -
                        (fingersPos[0][needHistoryCount - 1] - fingersPos[1][needHistoryCount - 1]) / 2;

                    DoubleDrag(move);
                    return;
                }
                else
                {
                    LogText.AddLogMessage("Log7");

                    Pinch(distance);
                    return;
                }
            }
        }
        else
        {
            int index = fingersPos.ToList().FindIndex(list => list.Count >= 2);

            if (index >= 0)
            {
                LogText.AddLogMessage("Log8");

                Vector2 move = fingersPos[index][0] - fingersPos[index][1];
                Drag(move);
            }
        }
    }

    private float Distance_Light(Vector2 point1,Vector2 point2)
    {
        return (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y);
    }
}
