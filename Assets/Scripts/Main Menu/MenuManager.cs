using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;
using Cards;

public class MenuManager : MonoBehaviour
{
    private List<Deck> decks = new List<Deck>();
    public Button play_button;
    public GameObject[] errors;
    public GameObject hotseat_menu;
    private string saveFilePath;
    private string decksFilePath;
    private string settingsFilePath;
    public int scene_index;


    void Awake()
    {
        saveFilePath = Application.persistentDataPath + "save.data";
        decksFilePath = Application.persistentDataPath + "decks.data";
        settingsFilePath = Application.persistentDataPath + "settings.data";
    }

    void Start()
    {        
        ApplicationModel.AllCardsDeck = LoadFile();
    }

    public void CreateFile(List<CardClass> all_cards)
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        using (var stream = File.Open(saveFilePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream))
            {
                foreach (CardClass card in all_cards)
                {
                    writer.WriteLine(card.ID);
                    writer.WriteLine(card.Name);
                    writer.WriteLine(card.Description);
                    writer.WriteLine(card.Health);
                    writer.WriteLine(card.Damage);
                    writer.WriteLine(card.Mana_Cost);
                    writer.WriteLine(card.Speed);
                    writer.WriteLine(card.Type);
                    writer.WriteLine(card.Ability_Type);
                    writer.WriteLine(card.Ability_Modifier);
                }
            }
        }
    }

    public List<CardClass> LoadFile()
    {
        if (File.Exists(saveFilePath))
        {
            List<CardClass> local_cards = new List<CardClass>();

            using (var stream = File.Open(saveFilePath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        CardClass card = new CardClass
                        (
                            reader.ReadLine(),
                            reader.ReadLine(),
                            reader.ReadLine(),
                            reader.ReadLine(),
                            reader.ReadLine(),
                            reader.ReadLine(),
                            reader.ReadLine(),
                            reader.ReadLine(),
                            reader.ReadLine(),
                            reader.ReadLine()
                        );

                        local_cards.Add(card);
                    }
                }
            }
            
            return local_cards;
        }

        else
        {
            return null;
        }
    }

    public void SetLevelIndex(int input)
    {
        scene_index = input;
    }

    public void OpenGameLobby()
    {
        hotseat_menu.GetComponent<HotseatLobby>().UpdateDecks();
        hotseat_menu.SetActive(true);
    }

    public void LoadLevel()
    {
        foreach (CardClass card in GameObject.Find("Player Decks").GetComponent<PlayerDecks>().player_1_deck) 
        {
            ApplicationModel.Player_1_Deck.Add(card);
        }
        foreach (CardClass card in GameObject.Find("Player Decks").GetComponent<PlayerDecks>().player_2_deck)
        {
            ApplicationModel.Player_2_Deck.Add(card);
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);

        GameObject.Find("Audio Manager").GetComponent<AudioManager>().StopAudio();
    }

    public void UpdateDecks()
    {
        foreach (GameObject error in errors)
        {
            error.SetActive(false);
        }

        decks.Clear();
        bool full_deck = false;

        foreach(Transform deck in GameObject.Find("Deck Manager").transform)
        {
            decks.Add(deck.gameObject.GetComponent<Deck>());
        }

        if (decks.Count.Equals(0))
        {
            play_button.interactable = false;
            play_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
            errors[2].SetActive(true);
        }

        else
        {
            foreach (Deck deck in decks)
            {
                if (deck.deck.Count.Equals(25))
                {
                    full_deck = true;
                    break;
                }
            }

            if (!full_deck)
            {
                play_button.interactable = false;
                play_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
                errors[0].SetActive(true);
            }

            else
            {
                play_button.interactable = true;
                play_button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
            }
        }
    }
}