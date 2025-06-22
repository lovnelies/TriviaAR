using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneController : MonoBehaviour
{
    // Referencia pública a quizstuff para que puedas asignarla en el inspector
    public QuizStuff quizStuff;

    // Para asegurarnos de que el objeto SceneController no se destruya al cambiar de escena
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

