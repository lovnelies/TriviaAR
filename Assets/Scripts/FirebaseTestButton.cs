using UnityEngine;
using UnityEngine.UI;

public class FirebaseTestButton : MonoBehaviour
{
    public Button testButton;

    void Start()
{
    //Debug.Log("FirebaseTestButton activo");

    if (testButton != null)
    {
        //Debug.Log("Botón asignado correctamente");
        testButton.onClick.AddListener(SaveTestData);
    }
    else
    {
        //Debug.LogError("No se asignó el botón de prueba.");
    }
}


    public void SaveTestData()
{
    Debug.Log("Botón de prueba presionado");

    if (FirebaseManager.Instance != null)
    {
        string nombre = "TestPlayer";
        int puntaje = Random.Range(10, 100);
        float tiempo = Random.Range(10f, 60f);

        FirebaseManager.Instance.SetPlayerName(nombre);
        FirebaseManager.Instance.SavePlayerData(puntaje, tiempo);

        //Debug.Log($"Intentando guardar datos de prueba: {nombre}, Score: {puntaje}, Tiempo: {tiempo}");
    }
    else
    {
        //Debug.LogError("FirebaseManager no está disponible.");
    }
}

}
