using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cards;

public class encyclopaedia : MonoBehaviour
{
    private List<CardClass> all_cards;
    private CardClass selected_card;
    public CardClass card_prefab;
    [HideInInspector] public int index = 0;
    private int index_temporary;

    public MenuManager menu_manager;

    new public TextMeshProUGUI name;
    public TextMeshProUGUI type;
    public TextMeshProUGUI ability_type;
    public TextMeshProUGUI mana_cost;
    public TextMeshProUGUI health;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI description;

    public Button previous_button;
    public Button next_button;


    // Start is called before the first frame update
    void Start()
    {
        all_cards = menu_manager.LoadFile();
        index_temporary = index;

        card_prefab.transform.Find("Artwork").GetComponent<Image>().sprite = Resources.Load<Sprite>("Card Artwork/000");
        card_prefab.transform.Find("Artwork").GetComponent<Image>().enabled = true;
        CheckDetails();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDetails();

        if (index.Equals(0))
        {
            previous_button.gameObject.SetActive(false);
        }

        else
        {
            previous_button.gameObject.SetActive(true);   
        }

        if (index.Equals(all_cards.Count - 1))
        {
            next_button.gameObject.SetActive(false);
        }

        else
        {
            next_button.gameObject.SetActive(true);
        }
    }

    private void CheckDetails()
    {
        selected_card = all_cards[index];

        if (!index_temporary.Equals(index))
        {
            card_prefab.ID = selected_card.ID;
            card_prefab.Name = selected_card.Name;
            card_prefab.Description = selected_card.Description;
            card_prefab.Type = selected_card.Type;
            card_prefab.Mana_Cost = selected_card.Mana_Cost;
            card_prefab.Health = selected_card.Health;
            card_prefab.Damage = selected_card.Damage;
            card_prefab.Speed = selected_card.Speed;
            card_prefab.Ability_Type = selected_card.Ability_Type;
            card_prefab.Ability_Modifier = selected_card.Ability_Modifier;

            card_prefab.DisplayData();

            name.text = selected_card.Name;
            type.text = selected_card.Type;
            ability_type.text = selected_card.Ability_Type;
            mana_cost.text = selected_card.Mana_Cost;
            health.text = selected_card.Health;
            damage.text = selected_card.Damage;
            speed.text = selected_card.Speed;
            description.text = selected_card.Description;

            index_temporary = index;
        }
    }

    public void PreviousCard()
    {
        if (index > 0)
        {
            index--;
        }
    }

    public void NextCard()
    {
        if (index < all_cards.Count - 1)
        {
            index++;
        }
    }
}
