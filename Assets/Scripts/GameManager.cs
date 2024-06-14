using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Prefab du dragon
    [SerializeField]
    private GameObject dragonPrefab;

    // Référence au joueur
    private GameObject player;

    // Référence à un composant TMP_Text pour afficher le minuteur
    public TMP_Text timerText;

    // Minuteur
    private float timeRemaining = 60f;
    private bool timerIsRunning = false;

    // Référence au dragon
    private GameObject dragonInstance;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Démarre le minuteur
        timerIsRunning = true;
        Debug.Log("Minuteur démarré");

        // Trouve le joueur dans la scène
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("Joueur trouvé");
        }
        else
        {
            Debug.LogError("Joueur non trouvé");
        }

        // Instancie le dragon, mais le désactive pour l'instant
        if (dragonPrefab != null)
        {
            dragonInstance = Instantiate(dragonPrefab);
            dragonInstance.SetActive(false);
            Debug.Log("Dragon instancié et désactivé");
        }
        else
        {
            Debug.LogError("Prefab de dragon non assigné");
        }
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                Debug.Log("Temps écoulé, déclenchement du Game Over");
                GameOver();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // Met à jour l'affichage du minuteur
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("Time Remaining: {0:00}:{1:00}", minutes, seconds);
            Debug.Log($"Temps restant mis à jour : {minutes:00}:{seconds:00}");
        }
        else
        {
            Debug.LogError("Référence à timerText non assignée");
        }
    }

    public void OnFireCreated()
    {
        // Arrête le minuteur
        timerIsRunning = false;
        Debug.Log("Minuteur arrêté suite à la création du feu");

        // Réinitialise le minuteur à 0
        timeRemaining = 0;
        UpdateTimerDisplay();

        // Désactive le dragon
        if (dragonInstance != null)
        {
            dragonInstance.SetActive(false);
            Debug.Log("Dragon désactivé");
        }
        else
        {
            Debug.LogError("Instance de dragon non trouvée pour désactivation");
        }

        // Vérifie si le feu a été fait en moins d'une minute ou en une minute
        if (timeRemaining >= 0)
        {
            // Si le joueur a réussi en moins d'une minute ou en une minute, aller à la scène 2
            Debug.Log("Le feu a été fait dans le temps imparti, changement de scène");
            StartCoroutine(LoadSceneAfterDelay(2, 11f)); // Scène 2 avec un délai d'une seconde
        }
        else
        {
            // Sinon, afficher le message de Game Over
            GameOver();
        }
    }

    public void GameOver()
    {
        // Affiche le message de Game Over
        if (timerText != null)
        {
            timerText.text = "Game Over!";
            Debug.Log("Message de Game Over affiché");
        }
        else
        {
            Debug.LogError("Référence à timerText non assignée pour le message de Game Over");
        }

        // Revenir à l'écran de départ après un délai
        StartCoroutine(ReloadStartScreenAfterDelay(3f));
    }

    IEnumerator ReloadStartScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0); // Assurez-vous que la scène de départ est l'index 0 dans les paramètres de build
    }

    IEnumerator LoadSceneAfterDelay(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2); // Charger la scène spécifiée
    }
}
