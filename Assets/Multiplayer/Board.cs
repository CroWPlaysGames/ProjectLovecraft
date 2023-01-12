using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using UnityEngine.UI;


public class Board : MonoBehaviour
{

    public List<GameObject> CardPositions = new List<GameObject>(6);
    public List<GameObject> CardPositions2 = new List<GameObject>(6);
    public GameObject[] opponent_side = new GameObject[6];
    public GameObject[] player_side = new GameObject[6];
    private List<GameObject> Posistions;
    [Header("GameObject References")]
    public GameObject position_1A;
    public GameObject position_1B;
    public GameObject position_1C;
    public GameObject position_1D;
    public GameObject position_1E;
    public GameObject position_1F;
    public GameObject position_2A;
    public GameObject position_2B;
    public GameObject position_2C;
    public GameObject position_2D;
    public GameObject position_2E;
    public GameObject position_2F;

    private GameManager game_manager;

    void Start()
    {
        Posistions = new List<GameObject>() { position_1A , position_1B , position_1C , position_1D , position_1E , position_1F , position_2A , position_2B , position_2C , position_2D , position_2E , position_2F };
        game_manager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (game_manager.player.ToString().Equals("Player_1"))
        {
            position_2A.GetComponent<Image>().raycastTarget = true;
            position_2B.GetComponent<Image>().raycastTarget = true;
            position_2C.GetComponent<Image>().raycastTarget = true;
            position_2D.GetComponent<Image>().raycastTarget = true;
            position_2E.GetComponent<Image>().raycastTarget = true;
            position_2F.GetComponent<Image>().raycastTarget = true;
        }

        else
        {
            position_1A.GetComponent<Image>().raycastTarget = true;
            position_1B.GetComponent<Image>().raycastTarget = true;
            position_1C.GetComponent<Image>().raycastTarget = true;
            position_1D.GetComponent<Image>().raycastTarget = true;
            position_1E.GetComponent<Image>().raycastTarget = true;
            position_1F.GetComponent<Image>().raycastTarget = true;
        }
        CardPositions.Add(position_1A);
        CardPositions.Add(position_1B);
        CardPositions.Add(position_1C);
        CardPositions.Add(position_1D);
        CardPositions.Add(position_1E);
        CardPositions.Add(position_1F);
        CardPositions2.Add(position_2A);
        CardPositions2.Add(position_2B);
        CardPositions2.Add(position_2C);
        CardPositions2.Add(position_2D);
        CardPositions2.Add(position_2E);
        CardPositions2.Add(position_2F);



    }

    public void UpdateBoard()
    {
        CheckTile(position_1A, opponent_side, 0);
        CheckTile(position_1B, opponent_side, 1);
        CheckTile(position_1C, opponent_side, 2);
        CheckTile(position_1D, opponent_side, 3);
        CheckTile(position_1E, opponent_side, 4);
        CheckTile(position_1F, opponent_side, 5);

        CheckTile(position_2A, player_side, 0);
        CheckTile(position_2B, player_side, 1);
        CheckTile(position_2C, player_side, 2);
        CheckTile(position_2D, player_side, 3);
        CheckTile(position_2E, player_side, 4);
        CheckTile(position_2F, player_side, 5);
    }

    private void CheckTile(GameObject tile, GameObject[] side, int index)
    {

        if (tile.transform.childCount > 0)
        {
            GameObject card = tile.transform.GetChild(0).gameObject;
            side[index] = card;
        }

        else
        {
            side[index] = null;
        }
    }
}