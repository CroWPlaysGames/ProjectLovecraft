using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GridManager : MonoBehaviour
{
    public int column_length;
    public int row_length;

    public float x_space;
    public float y_space;

    public float x_start;
    public float y_start;

    public string grid_type;

    public Scrollbar scrollbar;

    public int child_counter;

    public void Layout()
    {
        if (this.isActiveAndEnabled)
        {
            StartCoroutine(CalculateYStart());
        }
    }

    IEnumerator CalculateYStart()
    {
        child_counter = transform.childCount;

        yield return new WaitForFixedUpdate();

        switch (grid_type)
        {
            case "Deck List":
                GetComponent<RectTransform>().sizeDelta = new Vector2(455, y_space * transform.childCount);

                if (transform.childCount <= 5)
                {
                    y_start = 145;
                }

                else
                {
                    y_start = 145 + (y_space / 2 * (transform.childCount - 5));
                }

                yield return new WaitForEndOfFrame();
                ArrangeCards();
                break;

            case "Cards List":
                int rows;

                if ((Mathf.CeilToInt(transform.childCount) % 4).Equals(0))
                {
                    rows = Mathf.CeilToInt(transform.childCount / 4);
                }

                else
                {
                    rows = Mathf.CeilToInt(transform.childCount / 4) + 1;
                }

                GetComponent<RectTransform>().sizeDelta = new Vector2(825, y_space * rows);

                if (transform.childCount <= 8)
                {
                    y_start = 250;
                }

                else
                {
                    y_start = 250 + (y_space * rows / 3);
                }

                yield return new WaitForEndOfFrame();
                ArrangeCards();
                break;

            case "Deck Cell List":
                GetComponent<RectTransform>().sizeDelta = new Vector2(750, y_space * transform.childCount);

                if (transform.childCount <= 4)
                {
                    y_start = 157.5f;
                }

                else
                {
                    y_start = 157.5f + (52.5f * (transform.childCount - 4));
                }

                yield return new WaitForEndOfFrame();
                ArrangeCards();
                break;
        }
    }

    private void ArrangeCards()
    {        
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.localPosition = new Vector3(x_start + (x_space * (i % column_length)), y_start + (-y_space * (i / column_length)));
            
        }
    }
}
