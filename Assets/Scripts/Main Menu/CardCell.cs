using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cards;

public class CardCell : MonoBehaviour
{
    public CardClass card;

    public TextMeshProUGUI count;

    private DeckManager deck_manager;
    private AudioManager audio_manager;


    void Start()
    {
        deck_manager = GameObject.Find("Deck Manager").GetComponent<DeckManager>();
        audio_manager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();

        transform.Find("Name").GetComponent<TextMeshProUGUI>().text = card.Name;
        transform.Find("Mana Cost").Find("Value").GetComponent<TextMeshProUGUI>().text = card.Mana_Cost;
        transform.Find("Health").Find("Value").GetComponent<TextMeshProUGUI>().text = card.Health;
        transform.Find("Damage").Find("Value").GetComponent<TextMeshProUGUI>().text = card.Damage;
        transform.Find("Speed").Find("Value").GetComponent<TextMeshProUGUI>().text = card.Speed;

        foreach (Transform outline in transform.Find("Name Outline"))
        {
            outline.GetComponent<TextMeshProUGUI>().text = card.Name;
        }

        foreach (Transform outline in transform.Find("Mana Cost").Find("Mana Cost Outline"))
        {
            outline.GetComponent<TextMeshProUGUI>().text = card.Mana_Cost;
        }

        foreach (Transform outline in transform.Find("Health").Find("Health Outline"))
        {
            outline.GetComponent<TextMeshProUGUI>().text = card.Health;
        }

        foreach (Transform outline in transform.Find("Damage").Find("Damage Outline"))
        {
            outline.GetComponent<TextMeshProUGUI>().text = card.Damage;
        }

        foreach (Transform outline in transform.Find("Speed").Find("Speed Outline"))
        {
            outline.GetComponent<TextMeshProUGUI>().text = card.Speed;
        }

        foreach (Transform outline in transform.Find("Count Outline"))
        {
            outline.GetComponent<TextMeshProUGUI>().text = count.text;
        }
    }

    public void RemoveCard()
    {
        deck_manager.DeleteCard(this);
    }
}
