using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Cards;

public class CardViewController : MonoBehaviour
{
    [SerializeField] private GameObject card_prefab;
    [SerializeField] private Transform parent;
    private List<CardClass> all_cards;
    public GridManager card_list;
    public GridManager card_cell_list;

    public TMP_InputField card_name;
    public TMP_Dropdown card_type;
    public TMP_Dropdown mana_cost;
    public TMP_Dropdown health;
    public TMP_Dropdown damage;
    public TMP_Dropdown speed;
    public TMP_Dropdown ability_phase;

    private string[] goblins = {"000", "001", "002", "003", "004", "005", "006", "007"};
    private string[] undead = { "008", "009", "010", "011", "012", "013", "014" };
    private string[] summoner = { "015", "016", "017", "018", "019", "020", "021", "022" };

    private bool updating = false;

    private void Start()
    {
        all_cards = GameObject.Find("Menu").GetComponent<MenuManager>().LoadFile();
    }
    
    public void DisplayCards()
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }

        StartCoroutine(CreateCards());
    }

    IEnumerator CreateCards()
    {
        yield return new WaitForEndOfFrame();

        List<CardClass> cards_to_display = new List<CardClass>();

        foreach (CardClass card in all_cards)
        {
            if (!card_name.text.Equals("") && !card.Name.Contains(card_name.text))
            {
                continue;
            }

            switch (card_type.captionText.text)
            {
                case "All":
                    break;
                case "Goblins":
                    if (!goblins.Any(card.ID.Contains))
                    {
                        continue;
                    }
                    break;
                case "Undead":
                    if (!undead.Any(card.ID.Contains))
                    {
                        continue;
                    }
                    break;
                case "Summoner":
                    if (!summoner.Any(card.ID.Contains))
                    {
                        continue;
                    }
                    break;
            }

            switch (ability_phase.captionText.text)
            {
                case "All":
                    break;
                case "Pre-Battle":
                    if (!card.Ability_Type.Equals("Pre Battle"))
                    {
                        continue;
                    }
                    break;
                case "Battle":
                    if (!card.Ability_Type.Equals("None"))
                    {
                        continue;
                    }
                    break;
                case "Passive":
                    if (!card.Ability_Type.Equals("Passive") && !card.Ability_Type.Equals("Active"))
                    {
                        continue;
                    }
                    break;
                case "Post-Battle":
                    if (!card.Ability_Type.Equals("Post Battle") && !card.Ability_Type.Equals("Place"))
                    {
                        continue;
                    }
                    break;
            }

            cards_to_display.Add(card);
        }

        switch (mana_cost.captionText.text)
        {
            case "Low to High":
                cards_to_display = cards_to_display.OrderBy(card => int.Parse(card.Mana_Cost)).ToList();
                break;
            case "High to Low":
                cards_to_display = cards_to_display.OrderByDescending(card => int.Parse(card.Mana_Cost)).ToList();
                break;
        }

        switch (health.captionText.text)
        {
            case "Low to High":
                cards_to_display = cards_to_display.OrderBy(card => int.Parse(card.Health)).ToList();
                break;
            case "High to Low":
                cards_to_display = cards_to_display.OrderByDescending(card => int.Parse(card.Health)).ToList();
                break;
        }

        switch (damage.captionText.text)
        {
            case "Low to High":
                cards_to_display = cards_to_display.OrderBy(card => int.Parse(card.Damage)).ToList();
                break;
            case "High to Low":
                cards_to_display = cards_to_display.OrderByDescending(card => int.Parse(card.Damage)).ToList();
                break;
        }

        switch (speed.captionText.text)
        {
            case "Low to High":
                cards_to_display = cards_to_display.OrderBy(card => int.Parse(card.Speed)).ToList();
                mana_cost.value = 0;
                health.value = 0;
                damage.value = 0;
                break;
            case "High to Low":
                cards_to_display = cards_to_display.OrderByDescending(card => int.Parse(card.Speed)).ToList();
                mana_cost.value = 0;
                health.value = 0;
                damage.value = 0;
                break;
        }

        foreach (CardClass card in cards_to_display)
        {
            GameObject new_card = Instantiate(card_prefab, parent) as GameObject;

            new_card.GetComponent<CardClass>().ID = card.ID;
            new_card.GetComponent<CardClass>().Name = card.Name;
            new_card.GetComponent<CardClass>().Description = card.Description;
            new_card.GetComponent<CardClass>().Health = card.Health;
            new_card.GetComponent<CardClass>().Speed = card.Speed;
            new_card.GetComponent<CardClass>().Damage = card.Damage;
            new_card.GetComponent<CardClass>().Mana_Cost = card.Mana_Cost;
            new_card.GetComponent<CardClass>().Type = card.Type;
            new_card.GetComponent<CardClass>().Ability_Type = card.Ability_Type;
            new_card.GetComponent<CardClass>().Ability_Modifier = card.Ability_Modifier;
            new_card.GetComponent<CardClass>().InDeckView = true;
            new_card.name = card.Name;

            new_card.transform.localScale *= 1.25f;
        }

        updating = false;

        UpdateLayout();
    }

    public void ResetFilter()
    {
        card_name.text = "";
        card_type.value = 0;
        mana_cost.value = 0;
        health.value = 0;
        damage.value = 0;
        speed.value = 0;
        ability_phase.value = 0;
    }

    public void SelectFilter(string filter)
    {
        if (!updating)
        {
            updating = true;

            switch (filter)
            {
                case "Mana Cost":
                    health.value = 0;
                    damage.value = 0;
                    speed.value = 0;
                    break;
                case "Health":
                    mana_cost.value = 0;
                    damage.value = 0;
                    speed.value = 0;
                    break;
                case "Damage":
                    mana_cost.value = 0;
                    health.value = 0;
                    speed.value = 0;
                    break;
                case "Speed":
                    mana_cost.value = 0;
                    health.value = 0;
                    damage.value = 0;
                    break;
            }

            DisplayCards();
        }
    }

    public void UpdateLayout()
    {
        card_list.Layout();
        card_cell_list.Layout();
    }
}