using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public float rotationForce=1.0f;
    public float rotationHeight = 0.05f;
    public float attractionForce = 1.0f;
    public float buttonForce_xz = 1.0f;
    public float buttonForce_y = 1.0f;
    public float buttonForcePointHeight = -0.3f;
    public float stopBorder = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();

        if (rigidbody.velocity.magnitude > stopBorder)
        {
            Vector3 force = -this.transform.position;
            force.y += buttonForcePointHeight;
            rigidbody.AddForce(force * attractionForce);
        }
    }

    public void Button2Click()
    {
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();

        int force_n = 1000;
        float force_x = (float)(XOR128.Next(force_n * 2+1)- force_n) / (float)force_n;
        float force_y = 1f;
        float force_z = (float)(XOR128.Next(force_n * 2 + 1) - force_n) / (float)force_n;
        rigidbody.AddForce(force_x * buttonForce_xz, force_y * buttonForce_y, force_z * buttonForce_xz);

        int torque_n = 1000;
        float torque_x = (float)XOR128.Next(torque_n) / (float)torque_n + 1f;
        float torque_y = (float)XOR128.Next(torque_n) / (float)torque_n + 1f;
        float torque_z = (float)XOR128.Next(torque_n) / (float)torque_n + 1f;

        rigidbody.AddTorque(new Vector3(torque_x, torque_y, torque_z) * rotationForce, ForceMode.Impulse);
    }
}
