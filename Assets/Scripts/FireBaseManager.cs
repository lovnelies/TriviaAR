using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    
    private DatabaseReference databaseReference;
    private bool isFirebaseReady = false;
    private string playerName = "";
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
     void InitializeFirebase()
{
    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        var dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available)
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            app.Options.DatabaseUrl = new System.Uri("https://triviar-63a1c-default-rtdb.firebaseio.com/");
            
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            isFirebaseReady = true;
            Debug.Log("Firebase inicializado correctamente");
        }
        else
        {
            Debug.LogError($"Error al inicializar Firebase: {dependencyStatus}");
        }
    });
}
    
    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log("Nombre del jugador guardado: " + playerName);
    }
    
    public string GetPlayerName()
    {
        return playerName;
    }
    
    public void SavePlayerData(int score, float time)
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase no está listo");
            return;
        }
        
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("El nombre del jugador no está configurado");
            return;
        }
        
        PlayerGameData playerData = new PlayerGameData(playerName, score, time);
        string playerId = System.Guid.NewGuid().ToString();
        
        databaseReference.Child("rankings").Child(playerId).SetRawJsonValueAsync(JsonUtility.ToJson(playerData))
            .ContinueWithOnMainThread(task => {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log("Datos del jugador guardados correctamente en Firebase");
                }
                else
                {
                    Debug.LogError("Error al guardar datos: " + task.Exception);
                }
            });
    }
    
    public void LoadRankings(System.Action<List<PlayerGameData>> callback)
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase no está listo");
            return;
        }
        
        databaseReference.Child("rankings").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully)
            {
                DataSnapshot snapshot = task.Result;
                List<PlayerGameData> rankings = new List<PlayerGameData>();
                
                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    string json = childSnapshot.GetRawJsonValue();
                    if (!string.IsNullOrEmpty(json))
                    {
                        PlayerGameData player = JsonUtility.FromJson<PlayerGameData>(json);
                        rankings.Add(player);
                    }
                }
                
                // Ordenar por puntaje descendente, luego por tiempo ascendente
                rankings = rankings.OrderBy(p => p.tiempo)
                   .ThenByDescending(p => p.puntaje)
                   .Take(10)
                   .ToList();

                
                callback?.Invoke(rankings);
            }
            else
            {
                Debug.LogError("Error al cargar ranking: " + task.Exception);
                callback?.Invoke(new List<PlayerGameData>());
            }
        });
    }
    public void SavePlayerNameToFirebase(string playerName, System.Action<bool> callback)
{
    if (!isFirebaseReady)
    {
        Debug.LogError("Firebase no está listo");
        callback?.Invoke(false);
        return;
    }

    string playerId = System.Guid.NewGuid().ToString();

    // Guardamos solo el nombre en la ruta playersNames/{playerId}
    databaseReference.Child("playersNames").Child(playerId).SetValueAsync(playerName)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                Debug.Log("Nombre guardado correctamente en Firebase");
                // Guardamos localmente también para seguir usando
                SetPlayerName(playerName);
                callback?.Invoke(true);
            }
            else
            {
                Debug.LogError("Error al guardar el nombre: " + task.Exception);
                callback?.Invoke(false);
            }
        });
}
}

