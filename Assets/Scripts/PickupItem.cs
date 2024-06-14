using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // Référence à un composant TMP_Text pour afficher le compteur
    public TMP_Text counterText;

    // Noms globaux pour les groupes d'objets
    private static string stoneGroupName = "Stones";
    private static string axeGroupName = "Axes";
    private static string branchGroupName = "Branches";

    // Dictionnaire pour stocker les compteurs pour chaque type d'objet
    private static Dictionary<string, int> itemCounters = new Dictionary<string, int>();

    // Dictionnaire pour stocker les limites pour chaque type d'objet
    private static Dictionary<string, int> itemLimits = new Dictionary<string, int>();

    // Dictionnaire pour stocker les couleurs pour chaque type d'objet
    private static Dictionary<string, Color> itemColors = new Dictionary<string, Color>();

    // Prefab du feu
    [SerializeField]
    private GameObject firePrefab;

    // Booléen pour vérifier si le feu a été créé
    private bool isFireCreated = false;

    void Start()
    {
        // Initialise les limites pour chaque type d'objet
        itemLimits[stoneGroupName] = 3;
        itemLimits[axeGroupName] = 1;
        itemLimits[branchGroupName] = 2;

        // Initialise les couleurs pour chaque type d'objet
        itemColors[stoneGroupName] = Color.red;
        itemColors[axeGroupName] = Color.blue;
        itemColors[branchGroupName] = Color.green;

        // Initialise le compteur pour chaque groupe d'objets s'il n'est pas déjà initialisé
        InitializeCounter(stoneGroupName);
        InitializeCounter(axeGroupName);
        InitializeCounter(branchGroupName);

        // Initialise l'affichage du compteur pour tous les objets collectés jusqu'à présent
        UpdateCounterDisplay();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Récupère le nom de l'objet
            string itemName = gameObject.name;

            // Vérifie si la limite pour cet objet n'a pas été atteinte
            if (itemCounters.ContainsKey(itemName) && itemCounters[itemName] < itemLimits[itemName])
            {
                // Incrémente le compteur correspondant au groupe d'objets
                IncrementCounter(itemName);

                // Vérifie si la limite a été atteinte pour cet objet
                if (itemCounters[itemName] == itemLimits[itemName])
                {
                    // Change la couleur du texte pour cet objet
                    counterText.text = itemName + " collected: " + itemCounters[itemName] + "/" + itemLimits[itemName];
                    counterText.color = itemColors[itemName];
                }
                else
                {
                    // Met à jour l'affichage du compteur
                    UpdateCounterDisplay();
                }
            }

            // Détruit l'objet de la scène
            Destroy(gameObject);

            // Vérifie si les conditions pour allumer un feu sont remplies
            if (itemCounters[stoneGroupName] >= itemLimits[stoneGroupName] &&
                itemCounters[axeGroupName] >= itemLimits[axeGroupName] &&
                itemCounters[branchGroupName] >= itemLimits[branchGroupName])
            {
                // Crée le feu
                CreateFire();
            }
        }
    }

    // Méthode pour initialiser le compteur pour un groupe d'objets s'il n'est pas déjà initialisé
    void InitializeCounter(string groupName)
    {
        if (!itemCounters.ContainsKey(groupName))
        {
            itemCounters[groupName] = 0;
        }
    }

    // Méthode pour incrémenter le compteur pour un groupe d'objets
    void IncrementCounter(string groupName)
    {
        itemCounters[groupName]++;
    }

    // Méthode pour mettre à jour l'affichage du compteur pour tous les groupes d'objets
    void UpdateCounterDisplay()
    {
        if (counterText != null)
        {
            // Construit le texte à afficher avec les compteurs de tous les groupes d'objets
            string displayText = "";

            foreach (KeyValuePair<string, int> itemCounter in itemCounters)
            {
                displayText += itemCounter.Key + " collected: " + itemCounter.Value + "/" + itemLimits[itemCounter.Key] + "\n";
            }

            // Affiche le texte
            counterText.text = displayText;
        }
    }

    // Méthode pour créer un feu dans le jeu
    void CreateFire()
    {
        // Instancie un objet représentant le feu dans le jeu à la position actuelle du joueur ou d'un endroit spécifique
        Instantiate(firePrefab, transform.position, Quaternion.identity);
        isFireCreated = true;

        // Réinitialise les compteurs d'objets utilisés pour le feu
        itemCounters[stoneGroupName] = 0;
        itemCounters[axeGroupName] = 0;
        itemCounters[branchGroupName] = 0;

        // Met à jour l'affichage du compteur après avoir réinitialisé les compteurs
        UpdateCounterDisplay();

        // Informe le GameManager que le feu a été créé
        GameManager.Instance.OnFireCreated();
    }
}