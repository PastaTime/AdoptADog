using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public float speed = 10.0F;
    public float rotationSpeed = 10.0F;
    // Update is called once per frame
    void Update()
    {
        float test = Input.GetAxis("Vertical");
        float testHorizontal = Input.GetAxis("Horizontal");
        Debug.Log("v " + test + "h " + testHorizontal);
        float translationY = Input.GetAxis("Vertical") * speed;
        //Debug.Log("translation " + Input.GetAxis("Vertical"));
        float translateX = Input.GetAxis("Horizontal") * rotationSpeed;
        translationY *= Time.deltaTime;
        translateX *= Time.deltaTime;
        transform.Translate(0, 0, translationY);
        transform.Translate(translateX, 0, 0);

        if(Input.GetButton("FireA")) {
            Debug.Log("fire A");
        }
    }
}
