using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    // Start is called before the first frame update
    public Logger() { 
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }
}
