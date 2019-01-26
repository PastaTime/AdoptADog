using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    // Update is called once per frame
    void Update()
    {
        float test = Input.GetAxis("Vertical");
        float testHorizontal = Input.GetAxis("Horizontal");
        Debug.Log("v " + test + "h " + testHorizontal);
        float translation = Input.GetAxis("Vertical") * speed;
        //Debug.Log("translation " + Input.GetAxis("Vertical"));
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if(Input.GetButton("FireA")) {
            Debug.Log("fire A");
        }
    }
}
