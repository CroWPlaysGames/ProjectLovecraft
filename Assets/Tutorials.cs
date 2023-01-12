using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    public Options options_menu;
    public GameObject next_button;
    public GameObject close_button;
    public GameObject blur;
    
    [Header ("Multiplayer Tutorials")]
    public GameObject[] multiplayer_text;

    [Header("Deck Builder Tutorials")]
    public GameObject[] deck_builder_text;

    [Header("Card Manager Tutorials")]
    public GameObject[] card_manager_text;

    [Header("Hotseat Tutorials")]
    public GameObject[] hotseat_text;

    [HideInInspector] public int index = 0;
    private string current_tutorial = "";

    public void MultiplayerTutorials()
    {
        if (options_menu.multiplayer_tutorial)
        {
            blur.SetActive(true);
            current_tutorial = "Multiplayer";

            if (index.Equals(multiplayer_text.Length - 1))
            {
                next_button.SetActive(false);
                close_button.SetActive(true);
            }

            else
            {
                next_button.SetActive(true);
            }

            foreach (GameObject text in multiplayer_text)
            {
                text.SetActive(false);
            }

            multiplayer_text[index].SetActive(true);
        }
    }

    public void DeckBuilderTutorials()
    {
        if (options_menu.deck_builder_tutorial)
        {
            blur.SetActive(true);
            current_tutorial = "Deck Builder";

            if (index.Equals(deck_builder_text.Length - 1))
            {
                next_button.SetActive(false);
                close_button.SetActive(true);
            }

            else
            {
                next_button.SetActive(true);
            }

            foreach (GameObject text in deck_builder_text)
            {
                text.SetActive(false);
            }

            deck_builder_text[index].SetActive(true);
        }
    }

    public void CardManagerTutorials()
    {
        if (options_menu.card_manager_tutorial)
        {
            blur.SetActive(true);
            current_tutorial = "Card Manager";

            if (index.Equals(card_manager_text.Length - 1))
            {
                next_button.SetActive(false);
                close_button.SetActive(true);
            }

            else
            {
                next_button.SetActive(true);
            }

            foreach (GameObject text in card_manager_text)
            {
                text.SetActive(false);
            }

            card_manager_text[index].SetActive(true);
        }
    }

    public void HotseatTutorials()
    {
        if (options_menu.hotseat_tutorial)
        {
            blur.SetActive(true);
            current_tutorial = "Hotseat";

            if (index.Equals(hotseat_text.Length - 1))
            {
                next_button.SetActive(false);
                close_button.SetActive(true);
            }

            else
            {
                next_button.SetActive(true);
            }

            foreach (GameObject text in hotseat_text)
            {
                text.SetActive(false);
            }

            hotseat_text[index].SetActive(true);
        }
    }

    public void Next()
    {
        index++;

        switch(current_tutorial)
        {
            case "Multiplayer":
                MultiplayerTutorials();
                break;
            case "Deck Builder":
                DeckBuilderTutorials();
                break;
            case "Card Manager":
                CardManagerTutorials();
                break;
            case "Hotseat":
                HotseatTutorials();
                break;
        }
    }

    public void Close()
    {
        switch (current_tutorial)
        {
            case "Multiplayer":
                options_menu.multiplayer_tutorial = false;
                foreach (GameObject text in multiplayer_text)
                {
                    text.SetActive(false);
                }

                close_button.SetActive(false);
                break;
            case "Deck Builder":
                options_menu.deck_builder_tutorial = false;
                foreach (GameObject text in deck_builder_text)
                {
                    text.SetActive(false);
                }

                close_button.SetActive(false);
                break;
            case "Card Manager":
                options_menu.card_manager_tutorial = false;
                foreach (GameObject text in card_manager_text)
                {
                    text.SetActive(false);
                }

                close_button.SetActive(false);
                break;
            case "Hotseat":
                HotseatTutorials();
                break;
        }

        blur.SetActive(false); 
        options_menu.SaveSettings();
        index = 0;
    }
}
