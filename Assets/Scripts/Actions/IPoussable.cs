using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IPoussable : IActionnable
{
    // Je prend l'exemple du prof! Si jamais il y a un autre objet qui serait poussable, ce dernier serait facilement implementé grace à cette interface.
    void Pousser(float vitesseTombe, Vector3 PlayerRight);
    void Tomber();
}
