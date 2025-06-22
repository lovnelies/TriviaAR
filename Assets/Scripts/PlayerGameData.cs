[System.Serializable]
public class PlayerGameData
{
    public string nombre;
    public int puntaje;
    public float tiempo;
    public string fecha;
    
    public PlayerGameData(string nombre, int puntaje, float tiempo)
    {
        this.nombre = nombre;
        this.puntaje = puntaje;
        this.tiempo = tiempo;
        this.fecha = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
    }
}