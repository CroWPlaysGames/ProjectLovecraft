using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cards;

public class HUD : MonoBehaviour
{
    public GameManager game_manager;

    [Header("Battery")]
    public static float batteryLevel;
    public Image battery_icon;

    [Header("Mana")]
    public Sprite mana_unused;
    public Sprite mana_spent;

    private List<GameObject> mana_pool_b;
    private List<GameObject> mana_pool_a;
    public int player_1_current_mana;
    public int player_2_current_mana;

    [Header ("Timer")]
    private bool started_counting = false;
    public Button end_turn_button;
    [HideInInspector] public bool counting = false;
    [HideInInspector] public int round_count = 1;

    [Header ("Transition")]
    public Animator fade_black;
    public Image blur;
    public GameObject recycle_UI;
    public GameObject recycle_UI2;

    [Header ("Mana Icons")]
    public GameObject mana_icon_1A;
    public GameObject mana_icon_1B;
    public GameObject mana_icon_1C;
    public GameObject mana_icon_1D;
    public GameObject mana_icon_1E;

    public GameObject mana_icon_2A;
    public GameObject mana_icon_2B;
    public GameObject mana_icon_2C;
    public GameObject mana_icon_2D;
    public GameObject mana_icon_2E;
    [Header ("Announcements")]
    public TMP_Text your_turn;
    public TMP_Text opponent_place;
    public TMP_Text opponent_skip;

    public TMP_Text opponent_turn;
    public TMP_Text you_place;
    public TMP_Text you_skip;

    void Start() 
    {
        mana_pool_a = new List<GameObject>() { mana_icon_1A, mana_icon_1B, mana_icon_1C, mana_icon_1D, mana_icon_1E };
        mana_pool_b = new List<GameObject>() { mana_icon_2A, mana_icon_2B, mana_icon_2C, mana_icon_2D, mana_icon_2E };
    }
    void Update()
    {
        RoundCount();
    }


    private void RoundCount()
    {
        TextMeshProUGUI counter = GameObject.Find("Round Counter").transform.Find("Number").GetComponent<TextMeshProUGUI>();
        counter.text = game_manager.round_count.ToString();
    }

    public void Fade()
    {
        fade_black.SetTrigger("Fade Out");
    }

    public void ManaUpdate(int p1, int p2)
    {
        //mana_icon_2A.SetActive(true);
        int I = 0;
        foreach (GameObject ManaIcon in mana_pool_b) 
        {
            if (I < p1 && !game_manager.manaflipped)
            {
                ManaIcon.SetActive(true);
            }
            else { ManaIcon.SetActive(false); }
            I ++;   
        }
        I = 0;
        foreach (GameObject ManaIcon in mana_pool_a)
        {
            if (I < p2 && game_manager.manaflipped)
            {
                ManaIcon.SetActive(true);
            }
            else { ManaIcon.SetActive(false); }
            I++;
        }
    }

    public void ConfirmSelection()
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Button Click");
        for (int i = 0; i < 5; i++)
        {
            GameObject.Find("View Cards").transform.GetChild(i).GetComponent<CardClass>().InDeckView = false;
            GameObject.Find("View Cards").transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        game_manager.player_1_confirmed = true;

        game_manager.ShuffleCards(game_manager.player_1_deck);

        recycle_UI.SetActive(false);

        Transform old_parent = GameObject.Find("View Cards").transform;
        Transform new_parent = game_manager.player_1_hand.transform;

        game_manager.TransferCards(GameObject.Find("View Cards"), game_manager.player_1_hand, 1, false);
        game_manager.player_1_hand.SetActive(false);
        game_manager.ReadyText.transform.GetChild(0).GetComponent<TMP_Text>().text = "Hand " + ApplicationModel.Player_2_name + " the device";
        game_manager.Ready.SetActive(true);
        game_manager.ReadyText.SetActive(true);
        game_manager.player_1_hand.GetComponent<Button>().enabled = true;
        game_manager.DrawFirstHand2(game_manager.player_2_deck);

    }

    public void Recycle(int number)
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Button Click");
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Recycle");
        GameObject card = GameObject.Find("View Cards").transform.GetChild(number - 1).gameObject;
        CardClass old_details = card.GetComponent<CardClass>();
        CardClass new_details = game_manager.player_1_deck[0];
        card.GetComponent<CardClass>().SetDetails(new_details);

        game_manager.player_1_deck.Remove(game_manager.player_1_deck[0]);
        game_manager.player_1_deck.Add(old_details);
    }
    public void ConfirmSelection2()
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Button Click");
        GameObject.Find("Blur").GetComponent<Image>().enabled = false;
        for (int i = 0; i < 5; i++)
        {
            GameObject.Find("View Cards").transform.GetChild(i).GetComponent<CardClass>().InDeckView = false;
            GameObject.Find("View Cards").transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        game_manager.player_2_confirmed = true;

        game_manager.ShuffleCards(game_manager.player_2_deck);

        recycle_UI2.SetActive(false);

        Transform old_parent = GameObject.Find("View Cards").transform;
        Transform new_parent = game_manager.player_2_hand.transform;

        game_manager.TransferCards(GameObject.Find("View Cards"), game_manager.player_2_hand, 1, false);
        game_manager.player_2_hand.SetActive(false);
        game_manager.player_1_hand.SetActive(true);
        game_manager.is_viewing = false;
        end_turn_button.interactable = true;
        game_manager.player_1_hand.GetComponent<Button>().interactable = true;
        game_manager.player_2_hand.GetComponent<Button>().interactable = true;
        game_manager.ReadyText.transform.GetChild(0).GetComponent<TMP_Text>().text = "Hand " + ApplicationModel.Player_1_name + " the device";
        game_manager.player_2_hand.GetComponent<Button>().enabled = true;
        game_manager.Ready.SetActive(true);
        game_manager.ReadyText.SetActive(true);
        
    }

    public void Recycle2(int number)
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Button Click");
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Recycle");
        GameObject card = GameObject.Find("View Cards").transform.GetChild(number - 1).gameObject;
        CardClass old_details = card.GetComponent<CardClass>();
        CardClass new_details = game_manager.player_2_deck[0];

        card.GetComponent<CardClass>().SetDetails(new_details);

        game_manager.player_2_deck.Remove(game_manager.player_2_deck[0]);
        game_manager.player_2_deck.Add(old_details);
    }
}
