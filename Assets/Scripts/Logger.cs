using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    // Classe pour pouvoir faire des Debug.Log � partir des classes non-monobehaviour (les classe �tats)
    public Logger() { 
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }
}
