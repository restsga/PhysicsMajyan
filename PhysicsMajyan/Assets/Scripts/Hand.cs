using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float horizontalForceDivide = 1.0f;
    public float verticalForceDivide = 1.0f;
    public float rotationForceMultiple = 1.0f;
    
    static public GameObject selectingObject = null;
    
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
        if (selectingObject != null)
        {
            Vector3 cardPos = selectingObject.transform.position;
            chosenIcon.transform.position = new Vector3(cardPos.x, cardPos.y, cardPos.z);
        }
    }

    public void Horizontal(Vector2 vector)
    {
        if (selectingObject != null)
        {
            Vector3 force = new Vector3(vector.x / horizontalForceDivide, 0, vector.y / horizontalForceDivide);
            selectingObject.GetComponent<Rigidbody>().AddForce(force);
        }
    }

    public void Vertical(float distance)
    {
        if (selectingObject != null)
        {
            Vector3 force = new Vector3(0, distance / verticalForceDivide * (-1f), 0);
            selectingObject.GetComponent<Rigidbody>().AddForce(force);
        }
    }

    public void Rotation(Vector2 vector)
    {
        if (selectingObject != null)
        {
            Vector3 force = new Vector3(vector.y * rotationForceMultiple, vector.x * rotationForceMultiple * (-1f), 0);
            selectingObject.GetComponent<Rigidbody>().AddTorque(force);
        }
    }
}