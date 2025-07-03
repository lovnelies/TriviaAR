using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacionAutomatica : MonoBehaviour 
{
    public float velocidadRotacion = 30f;
    
    void Update() 
    {
        // Rotar solo en el eje Y (horizontal)
        transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0);
    }
}