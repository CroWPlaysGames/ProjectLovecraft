using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private DeckManager deck_manager;

    void Start()
    {
        try
        {
            deck_manager = GameObject.Find("Deck Manager").GetComponent<DeckManager>();
            
        }

        catch
        {
            // Ignore
        }
    }

    public void AddCard()
    {
        deck_manager.AddCard(gameObject);
        LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector3(1.25f, 1.25f, 1.25f), 0.25f);
        LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector3(1.25f, 1.25f, 1.25f), 0.25f).setDelay(0.25f);
    }
}
