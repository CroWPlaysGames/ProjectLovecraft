using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Cards;

public class DeckManager : MonoBehaviour
{
    [Header ("Deck Objects")]
    public GameObject deck_prefab;
    public GameObject selected_deck;
    private string selected_deck_name;
    private GameObject temporary_deck;
    [HideInInspector] public GameObject temporary_deck_cell;

    [Header ("Menus")]
    public MenuManager menu_manager;
    public GameObject creation_menu;
    public GameObject deck_list_menu;
    public TMP_InputField created_deck_name;
    public TMP_InputField renamed_deck_name;
    public GameObject deck_cell;
    public Transform deck_cell_list;
    public GameObject card_cell;
    public Transform card_cell_list;
    public GameObject card_cell_manager;
    public GameObject filter_button;
    public GameObject encyclopaedia_button;
    public GameObject rename_menu;
    public Button create_new_deck;
    public GameObject no_decks_found;

    public GameObject delete_menu;
    public Button confirm_delete_button;
    public TextMeshProUGUI confirm_delete_button_text;

    public GameObject[] errors;

    public GameObject cards_menu;
    public GameObject manage_decks_menu;
    private string saveFilePath;
    private string decksFilePath;
    private string settingsFilePath;
    [Header ("Deck Counter")]
    public TextMeshProUGUI deck_count;
    public GameObject deck_viewer;

    public GameObject deck_menu;
    public GameObject card_manager_menu;
    public Scrollbar card_list;
    public GameObject blur;
    [Header("Deck Settings")]
    public int MaxCardCount;
    void Awake()
    {
        saveFilePath = Application.persistentDataPath + "save.data";
        decksFilePath = Application.persistentDataPath + "decks.data";
        settingsFilePath = Application.persistentDataPath + "settings.data";
    }
    
    void Start()
    {
        LoadDecks();
        RefreshDeckList();
    }

    void Update()
    {
        try
        {
            deck_count.text = selected_deck.GetComponent<Deck>().deck.Count.ToString();
        }

        catch
        {
            deck_count.text = "X";
        }

        if (transform.childCount.Equals(0))
        {
            no_decks_found.SetActive(true);
        }

        else
        {
            no_decks_found.SetActive(false);
        }
    }

    public void CreateNewDeck()
    {
        ClearErrors();

        if (created_deck_name.text.Equals(""))
        {
            errors[0].SetActive(true);
        }

        else
        {
            bool duplicate_name_found = false;

            foreach (Transform deck in transform)
            {
                if (deck.gameObject.name.Equals(created_deck_name.text))
                {
                    duplicate_name_found = true;
                    break;
                }
            }

            if (duplicate_name_found)
            {
                errors[1].SetActive(true);
            }

            else
            {
                GameObject new_deck = Instantiate(deck_prefab, transform);
                new_deck.name = created_deck_name.text;
                new_deck.transform.localPosition = Vector3.zero;
                new_deck.GetComponent<Deck>().name = created_deck_name.text;

                SetDeck(new_deck);
                RefreshDeckList();

                creation_menu.SetActive(false);
                create_new_deck.enabled = true;

                created_deck_name.text = "";
            }
        }

        SaveDecks();
    }

    public void RenameDeck()
    {
        ClearErrors();

        if (renamed_deck_name.text.Equals(""))
        {
            errors[2].SetActive(true);
        }

        else
        {
            bool duplicate_name_found = false;

            foreach (Transform deck in transform)
            {
                if (deck.gameObject.name.Equals(renamed_deck_name.text))
                {
                    duplicate_name_found = true;
                    break;
                }
            }

            if (duplicate_name_found)
            {
                errors[3].SetActive(true);
            }

            else
            {
                temporary_deck_cell.name = renamed_deck_name.text;
                temporary_deck_cell.GetComponent<DeckCell>().deck.name = renamed_deck_name.text;
                temporary_deck_cell.GetComponent<DeckCell>().deck.GetComponent<Deck>().name = renamed_deck_name.text;

                RefreshDeckList();

                rename_menu.SetActive(false);

                renamed_deck_name.text = "";
            }
        }

        SaveDecks();
    }

    public void DeleteDeck()
    {
        Destroy(temporary_deck_cell.GetComponent<DeckCell>().deck);
        Destroy(temporary_deck_cell);
        delete_menu.SetActive(false);

        SaveDecks();
    }

    private void SetDeck(GameObject picked_deck)
    {
        selected_deck = picked_deck;

        RefreshCardList();
        SaveDecks();
    }

    public void RefreshDeckList()
    {
        foreach (Transform deck_cell in deck_cell_list)
        {
            Destroy(deck_cell.gameObject);
        }

        foreach (Transform deck in transform)
        {
            GameObject new_deck_cell = Instantiate(deck_cell, deck_cell_list);
            new_deck_cell.GetComponent<DeckCell>().deck = deck.gameObject;
            new_deck_cell.name = new_deck_cell.GetComponent<DeckCell>().deck.name;
        }
    }

    private void ClearErrors()
    {
        foreach (GameObject error in errors)
        {
            error.SetActive(false);
        }
    }

    public void RefreshCardList()
    {
        foreach (Transform card_cell in card_cell_list)
        {
            Destroy(card_cell.gameObject);
        }

        StartCoroutine(WaitForDeletedCardCells());
    }

    IEnumerator WaitForDeletedCardCells()
    {
        yield return new WaitForFixedUpdate();

        foreach (CardClass card in selected_deck.GetComponent<Deck>().deck)
        {
            bool needs_to_be_created = true;
            bool found_card_cell = false;

            foreach (Transform card_cell in card_cell_list)
            {
                if (card_cell.name.Equals(card.Name) && !found_card_cell)
                {
                    card_cell.GetComponent<CardCell>().count.text = (int.Parse(card_cell.GetComponent<CardCell>().count.text) + 1).ToString();
                    needs_to_be_created = false;
                    found_card_cell = true;
                    break;
                }
            }

            if (needs_to_be_created)
            {
                GameObject new_card_cell = Instantiate(card_cell, card_cell_list);
                new_card_cell.GetComponent<CardCell>().count.text = "1";
                new_card_cell.GetComponent<CardCell>().card = card;
                new_card_cell.name = card.Name;
            }
        }
    }

    public void AddCard(GameObject card)
    {
        if (!selected_deck.GetComponent<Deck>().deck.Count.Equals(25))
        {
            int i = 0;
            foreach(CardClass find_card in selected_deck.GetComponent<Deck>().deck)
            {
                if (find_card.Equals(card.GetComponent<CardClass>()))
                {
                    i++;
                }
            }

            if (i < MaxCardCount)
            {
                selected_deck.GetComponent<Deck>().deck.Add(card.GetComponent<CardClass>());

                GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Add Card");

                RefreshCardList();
                SaveDecks();

                card_cell_list.gameObject.GetComponent<GridManager>().Layout();
                
            }
        }
    }

    public void DeleteCard(CardCell card_cell)
    {
        selected_deck.GetComponent<Deck>().deck.Remove(card_cell.card);

        card_cell.count.text = (int.Parse(card_cell.count.text) - 1).ToString();

        if (card_cell.count.text.Equals(0))
        {
            Destroy(card_cell);
        }

        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Remove Card");

        RefreshCardList();
        SaveDecks();

        card_cell_list.gameObject.GetComponent<GridManager>().Layout();
    }

    public void SaveDecks()
    {
        if (File.Exists(decksFilePath))
        {
            File.Delete(decksFilePath);
        }

        using (var stream = File.Open(decksFilePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream))
            {
                foreach (Transform deck in transform)                
                {
                    writer.WriteLine(deck.GetComponent<Deck>().name);

                    foreach (CardClass card in deck.GetComponent<Deck>().deck)
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

                    // Add a break to split decks when importing
                    writer.WriteLine("");
                }
            }
        }

        menu_manager.UpdateDecks();
    }

    public void LoadDecks()
    {
        if (File.Exists(decksFilePath))
        {
            List<Deck> local_decks = new List<Deck>();

            using (var stream = File.Open(decksFilePath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    bool create_next_deck = true;
                    
                    while (!reader.EndOfStream)
                    {
                        if (create_next_deck)
                        {
                            GameObject deck = Instantiate(deck_prefab, transform);
                            deck.GetComponent<Deck>().name = reader.ReadLine();
                            deck.name = deck.GetComponent<Deck>().name;

                            temporary_deck = deck;

                            create_next_deck = false;
                        }

                        string line = reader.ReadLine();

                        if(line == null)
                        {
                            continue;
                        }

                        if (!line.Equals(""))
                        {
                            CardClass card = new CardClass
                            (
                                line,
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

                            temporary_deck.GetComponent<Deck>().deck.Add(card);
                        }

                        else
                        {
                            create_next_deck = true;
                        }
                    }
                }
            }
        }

        else
        {
            Debug.Log("No decks locally found");
        }
    }

    public void EditDeck(GameObject deck)
    {
        SetDeck(deck);
        deck_menu.SetActive(false);
        card_manager_menu.SetActive(true);
        card_manager_menu.GetComponent<CardViewController>().DisplayCards();
        card_list.value = 1;
        GameObject.Find("Tutorials").GetComponent<Tutorials>().CardManagerTutorials();
    }

    public void RenameDeckButton()
    {
        rename_menu.SetActive(true);
        blur.SetActive(true);
    }

    public void DeleteDeckButton()
    {
        delete_menu.SetActive(true);
        blur.SetActive(true);
    }
}
