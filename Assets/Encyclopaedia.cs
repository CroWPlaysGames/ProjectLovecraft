using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cards;

public class Encyclopaedia : MonoBehaviour
{
    private List<CardClass> all_cards;
    private CardClass selected_card;
    public CardClass card_prefab;
    public int index = 0;
    private int index_temporary = 1;

    new public TextMeshProUGUI name;
    public TextMeshProUGUI type;
    public TextMeshProUGUI ability_type;
    public TextMeshProUGUI mana_cost;
    public TextMeshProUGUI health;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI description;
    public Button back_button;
    public Button previous_button;
    public Button next_button;
    public Button last_button;
    public GameObject posion;
    public GameObject DeathTouch;
    public GameObject posionText;
    public GameObject DeathTouchText;
    public GameObject Text;
    public GameObject CardView;
    public TextMeshProUGUI total_page_count;
    public TextMeshProUGUI current_page;

    private bool wait_timer = false;


    // Start is called before the first frame update
    void Start()
    {
        all_cards = ApplicationModel.AllCardsDeck;
        Debug.Log(all_cards.Count);
        CheckDetails();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDetails();
        UpdatePageNumber();

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
            back_button.gameObject.SetActive(false);
            posion.SetActive(false);
            DeathTouch.SetActive(false);
            posionText.SetActive(false);
            DeathTouchText.SetActive(false);
            CardView.SetActive(true);
            Text.SetActive(true);
            last_button.gameObject.SetActive(true);
            next_button.gameObject.SetActive(true);
        }
    }
    public void SetIndex(int i) 
    {
        index = i;
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
            card_prefab.GetComponent<CardClass>().InDeckView = true;
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
    public void ButtonClick() 
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Button Click");
    }
    public void PageFlip() 
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Page Flip");
    }
    public void PreviousCard()
    {
        if (!wait_timer)
        {
            StartCoroutine(PreviousPage());
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Page Flip");
        }        
    }

    IEnumerator PreviousPage()
    {
        wait_timer = true;

        if (index > 0)
        {
            index--;
        }

        yield return new WaitForSeconds(0.25f);

        wait_timer = false;
    }

    public void NextCard()
    {
        if (!wait_timer)
        {
            StartCoroutine(NextPage());
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Page Flip");
        }
    }

    IEnumerator NextPage()
    {
        wait_timer = true;

        if (index < all_cards.Count - 1)
        {
            index++;
        }

        yield return new WaitForSeconds(0.25f);

        wait_timer = false;
    }

    private void UpdatePageNumber()
    {
        total_page_count.text = "/ " + all_cards.Count.ToString();

        current_page.text = (index + 1).ToString();
    }

    public void JumpTo(int next_index)
    {
        index = next_index;
    }
}
