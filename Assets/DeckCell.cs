using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckCell : MonoBehaviour
{
    public GameObject deck;
    public TextMeshProUGUI deck_name;
    public TextMeshProUGUI deck_count;

    public Button select_button; 
    public TextMeshProUGUI select_button_text;

    public GameObject rename_menu;
    public GameObject delete_menu;

    private DeckManager deck_manager;
    private AudioManager audio_manager;

    void Start()
    {
        try
        {
            deck_manager = GameObject.Find("Deck Manager").GetComponent<DeckManager>();
            rename_menu = deck_manager.rename_menu;
            delete_menu = deck_manager.delete_menu;

            audio_manager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        }

        catch
        {
            // Unable to find deck manager
        }
    }

    void Update()
    {
        try
        {
            deck_name.text = deck.GetComponent<Deck>().name;
            deck_count.text = deck.GetComponent<Deck>().deck.Count.ToString();
        }
        
        catch
        {
            // Ignore
        }
    }

    public void Edit()
    {
        deck_manager.EditDeck(deck);
        audio_manager.Play("Button Click");        
    }

    public void Rename()
    {
        deck_manager.temporary_deck_cell = this.gameObject;
        deck_manager.RenameDeckButton();
        audio_manager.Play("Button Click");
    }

    public void Delete()
    {
        deck_manager.temporary_deck_cell = this.gameObject;
        deck_manager.DeleteDeckButton();
        audio_manager.Play("Button Click");
    }
}
