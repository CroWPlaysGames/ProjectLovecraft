using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cards;
public class CardViewcard : MonoBehaviour
{
    [HideInInspector] public CardViewController CardViewController;
    [HideInInspector] public CardClass card;

    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Description;
    [SerializeField] private TMP_Text Cost;
    [SerializeField] private TMP_Text Health;
    [SerializeField] private TMP_Text Speed;
    [SerializeField] private TMP_Text Damage;
    [SerializeField] private Button _cardSelect;

    void Start()
    {
        _cardSelect.onClick.AddListener(CardSelect);
        Name.text = card.Name;
        Description.text = card.Description;
        Cost.text = card.Mana_Cost.ToString();
        Health.text = card.Health.ToString();
        Speed.text = card.Speed.ToString();
        Damage.text = card.Damage.ToString();
        GameObject.FindGameObjectWithTag("CardView").GetComponent<DeckCreator>().DeckCreate();
    }
    private void CardSelect()
    {
        GameObject.FindGameObjectWithTag("CardView").GetComponent<DeckCreator>().DeckAdd();
        Debug.Log(card.Name.ToString()+" Selected");
    }
    
}
