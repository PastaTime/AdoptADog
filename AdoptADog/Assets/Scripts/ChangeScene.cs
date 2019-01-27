using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < 5; i++)
        {
            if (Controller.getSingleton().getX(i)) 
            {
            //switch scene
            }
        }
    }
}
