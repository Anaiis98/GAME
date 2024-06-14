using UnityEngine;
using System.Collections.Generic;

public class CounterManager : MonoBehaviour
{
    public Transform itemCounterContainer; // Le conteneur o� les compteurs seront affich�s
    public GameObject itemCounterPrefab; // Le prefab du compteur d'objet

    private Dictionary<string, CounterItemUI> itemCounters = new Dictionary<string, CounterItemUI>();

    public void AddItem(string itemName, Sprite itemSprite)
    {
        if (!itemCounters.ContainsKey(itemName))
        {
            // Cr�er un nouvel objet UI pour le compteur
            GameObject newItemCounter = Instantiate(itemCounterPrefab, itemCounterContainer);

            // Configurer le compteur avec l'image et le nom de l'objet
            CounterItemUI counterItemUI = newItemCounter.GetComponent<CounterItemUI>();
            counterItemUI.Initialize(itemSprite, itemName);

            // Ajouter le compteur au dictionnaire
            itemCounters[itemName] = counterItemUI;
        }

        // Incr�menter le compteur de l'objet
        itemCounters[itemName].IncrementCount();
    }
}
