using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.Assertions;
using System.IO;
namespace Cards
{
    public class CardInit : MonoBehaviour
    {
        public List<CardClass> all_cards = new List<CardClass>();

        // Start is called before the first frame update
        void Start()
        {
            //init firebase connection
            var firestore = FirebaseFirestore.DefaultInstance;
            Query allCardsQuery = firestore.Collection("Cards");

            allCardsQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot Result = task.Result;

                foreach (DocumentSnapshot card in Result.Documents)
                {
                    var CardDictionary = card.ToDictionary();
                    var cardData = new CardClass(CardDictionary["ID"].ToString(), CardDictionary["Name"].ToString(), CardDictionary["Description"].ToString(), CardDictionary["Health"].ToString(), CardDictionary["Damage"].ToString(), CardDictionary["Mana_Cost"].ToString(), CardDictionary["Speed"].ToString(), CardDictionary["Type"].ToString(), CardDictionary["Ability_Type"].ToString(), CardDictionary["Ability_Modifier"].ToString());
                    all_cards.Add(cardData);
                }

                GameObject.Find("Menu").GetComponent<MenuManager>().CreateFile(all_cards);
                Destroy(gameObject);
            });

        }
    }
}