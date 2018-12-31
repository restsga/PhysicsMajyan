using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float horizontalForceDivide = 1.0f;
    public float verticalForceDivide = 1.0f;
    public float rotationForceMultiple = 1.0f;

    static public GameObject selectingCard = null;

    private GameObject chosenIcon;
    
    // Use this for initialization
    void Start()
    {
        MultiTouch.Single= Horizontal;
        MultiTouch.Pinch=Vertical;
        MultiTouch.DoubleDrag = Rotation;

        chosenIcon = GameObject.Find("UIObjects/ChosenIcon");
    }

    // Update is called once per frame
    void Update()
    {
        if (selectingCard != null)
        {
            Vector3 cardPos = selectingCard.transform.position;
            chosenIcon.transform.position = new Vector3(cardPos.x, cardPos.y, cardPos.z);
        }
    }

    public void Horizontal(Vector2 vector)
    {
        if (selectingCard != null)
        {
            selectingCard.GetComponent<Rigidbody>().AddForce
                (vector.x/horizontalForceDivide, 0, vector.y / horizontalForceDivide);
        }
    }

    public void Vertical(float distance)
    {
        if (selectingCard != null)
        {
            selectingCard.GetComponent<Rigidbody>().AddForce
                (new Vector3(0, distance / verticalForceDivide*(-1f), 0));
        }
    }

    public void Rotation(Vector2 vector)
    {
        if (selectingCard != null)
        {
            selectingCard.GetComponent<Rigidbody>().AddTorque
                (new Vector3(vector.y*rotationForceMultiple,vector.x*rotationForceMultiple*(-1f),0));
        }
    }
}