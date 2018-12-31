using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MultiTouch_Single(Vector2 move);
public delegate void MultiTouch_Pinch(float distanceChange);
public delegate void MultiTouch_DoubleDrag(Vector2 move);

public class TouchPos
{
    public Vector2 pos;
    
    public TouchPos(Vector2 pos)
    {
        this.pos = new Vector2(pos.x, pos.y);
    }
}

public class MultiTouch : MonoBehaviour
{
    public int needPinchOrDragCount = 5;
    public float dragBorder = 2.0f;
    public int maxHistoryCount = 60;

    private List<List<TouchPos>> touchHistories;

    static public MultiTouch_Single Single;
    static public MultiTouch_Pinch Pinch;
    static public MultiTouch_DoubleDrag DoubleDrag;

    // Use this for initialization
    void Start()
    {
        touchHistories = new List<List<TouchPos>>();
    }

    // Update is called once per frame
    void Update()
    {
        SaveTouchData();
        CheckAction();
    }

    private void SaveTouchData()
    {
        List<TouchPos> touches = new List<TouchPos>();

        for (int i = 0; i < Input.touchCount; i++)
        {
            touches.Add(new TouchPos(Input.touches[i].position));
        }

        touchHistories.Insert(0, touches);

        if (touchHistories.Count > maxHistoryCount)
        {
            touchHistories.RemoveRange(maxHistoryCount, touchHistories.Count - maxHistoryCount);
        }
    }

    private void CheckAction()
    {
        if (touchHistories.Count >= needPinchOrDragCount)
        {
            int targetIndex = needPinchOrDragCount - 1;
            if (touchHistories[0].Count == 2 && touchHistories[targetIndex].Count == 2)
            {
                float distance_after = 
                    Distance(touchHistories[0][0].pos, touchHistories[0][1].pos);
                float distance_before=
                    Distance(touchHistories[targetIndex][0].pos,
                    touchHistories[targetIndex][1].pos);
                float distanceChange = distance_after - distance_before;                    

                if (Mathf.Abs(distanceChange) > dragBorder)
                {
                    Pinch(distanceChange);
                }
                else
                {
                    DoubleDrag(MiddlePoint(touchHistories[0][0].pos, touchHistories[0][1].pos) -
                        MiddlePoint(touchHistories[targetIndex][0].pos, touchHistories[targetIndex][1].pos));
                }
            }
        }

        if (touchHistories.Count >= 2)
        {
            if (touchHistories[0].Count == 1 && touchHistories[1].Count == 1)
            {
                Single(touchHistories[0][0].pos - touchHistories[1][0].pos);
            }
        }
    }

    private float Distance(Vector2 point1,Vector2 point2)
    {
        return Mathf.Sqrt((point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y));
    }

    private Vector2 MiddlePoint(Vector2 point1,Vector2 point2)
    {
        return (point1 + point2) / 2;
    }
}