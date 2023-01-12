using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotseatLobby : MonoBehaviour
{
    public TMP_InputField player_1_name;
    public TMP_InputField player_2_name;

    public TMP_Dropdown player_1_dropdown;
    public TMP_Dropdown player_2_dropdown;

    public GameObject decks;
    public PlayerDecks player_decks;

    public MenuManager menu_manager;

    public void UpdateDecks()
    {
        player_1_dropdown.ClearOptions();
        player_2_dropdown.ClearOptions();

        List<string> deck_names = new List<string>();

        foreach (Transform deck in decks.transform)
        {
            if (deck.GetComponent<Deck>().deck.Count.Equals(25))
            {
                deck_names.Add(deck.name);
            }
        }

        player_1_dropdown.AddOptions(deck_names);
        player_2_dropdown.AddOptions(deck_names);
    }

    public void PlayGame()
    {
        player_decks.player_1_name = player_1_name.text;
        player_decks.player_2_name = player_2_name.text;
        ApplicationModel.Player_2_name = player_2_name.text;
        ApplicationModel.Player_1_name = player_1_name.text;
        player_decks.player_1_deck = GameObject.Find(player_1_dropdown.captionText.text).GetComponent<Deck>().deck;
        player_decks.player_2_deck = GameObject.Find(player_2_dropdown.captionText.text).GetComponent<Deck>().deck;


        menu_manager.LoadLevel();
    }
}
