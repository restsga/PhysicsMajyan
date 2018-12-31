using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : MonoBehaviour
{
    //最高高度
    public float maxHeight=0.1f;

    //初期高度
    private float firstHeight=0f;

    // Use this for initialization
    void Start()
    {
        firstHeight = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
