using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IPoussable : IActionnable
{
    void Pousser(float vitesseTombe, Vector3 PlayerRight);
    void Tomber();
}
