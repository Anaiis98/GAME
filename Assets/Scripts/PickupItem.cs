using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // R�f�rence � un composant TMP_Text pour afficher le compteur
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

    // Bool�en pour v�rifier si le feu a �t� cr��
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

        // Initialise le compteur pour chaque groupe d'objets s'il n'est pas d�j� initialis�
        InitializeCounter(stoneGroupName);
        InitializeCounter(axeGroupName);
        InitializeCounter(branchGroupName);

        // Initialise l'affichage du compteur pour tous les objets collect�s jusqu'� pr�sent
        UpdateCounterDisplay();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // R�cup�re le nom de l'objet
            string itemName = gameObject.name;

            // V�rifie si la limite pour cet objet n'a pas �t� atteinte
            if (itemCounters.ContainsKey(itemName) && itemCounters[itemName] < itemLimits[itemName])
            {
                // Incr�mente le compteur correspondant au groupe d'objets
                IncrementCounter(itemName);

                // V�rifie si la limite a �t� atteinte pour cet objet
                if (itemCounters[itemName] == itemLimits[itemName])
                {
                    // Change la couleur du texte pour cet objet
                    counterText.text = itemName + " collected: " + itemCounters[itemName] + "/" + itemLimits[itemName];
                    counterText.color = itemColors[itemName];
                }
                else
                {
                    // Met � jour l'affichage du compteur
                    UpdateCounterDisplay();
                }
            }

            // D�truit l'objet de la sc�ne
            Destroy(gameObject);

            // V�rifie si les conditions pour allumer un feu sont remplies
            if (itemCounters[stoneGroupName] >= itemLimits[stoneGroupName] &&
                itemCounters[axeGroupName] >= itemLimits[axeGroupName] &&
                itemCounters[branchGroupName] >= itemLimits[branchGroupName])
            {
                // Cr�e le feu
                CreateFire();
            }
        }
    }

    // M�thode pour initialiser le compteur pour un groupe d'objets s'il n'est pas d�j� initialis�
    void InitializeCounter(string groupName)
    {
        if (!itemCounters.ContainsKey(groupName))
        {
            itemCounters[groupName] = 0;
        }
    }

    // M�thode pour incr�menter le compteur pour un groupe d'objets
    void IncrementCounter(string groupName)
    {
        itemCounters[groupName]++;
    }

    // M�thode pour mettre � jour l'affichage du compteur pour tous les groupes d'objets
    void UpdateCounterDisplay()
    {
        if (counterText != null)
        {
            // Construit le texte � afficher avec les compteurs de tous les groupes d'objets
            string displayText = "";

            foreach (KeyValuePair<string, int> itemCounter in itemCounters)
            {
                displayText += itemCounter.Key + " collected: " + itemCounter.Value + "/" + itemLimits[itemCounter.Key] + "\n";
            }

            // Affiche le texte
            counterText.text = displayText;
        }
    }

    // M�thode pour cr�er un feu dans le jeu
    void CreateFire()
    {
        // Instancie un objet repr�sentant le feu dans le jeu � la position actuelle du joueur ou d'un endroit sp�cifique
        Instantiate(firePrefab, transform.position, Quaternion.identity);
        isFireCreated = true;

        // R�initialise les compteurs d'objets utilis�s pour le feu
        itemCounters[stoneGroupName] = 0;
        itemCounters[axeGroupName] = 0;
        itemCounters[branchGroupName] = 0;

        // Met � jour l'affichage du compteur apr�s avoir r�initialis� les compteurs
        UpdateCounterDisplay();

        // Informe le GameManager que le feu a �t� cr��
        GameManager.Instance.OnFireCreated();
    }
}