using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    public CanvasGroup splash_bakcground;
    public CanvasGroup patchwork_productions;
    public CanvasGroup wildlife_studios;

    private bool part_1 = true;
    private bool part_2 = false;
    private bool part_3 = false;
    private bool part_4 = false;
    private bool part_5 = false;

    IEnumerator WaitTime(int part_number, int time)
    {
        yield return new WaitForSeconds(time);

        switch (part_number)
        {
            case 2:
                part_2 = true;
                break;
            case 4:
                part_4 = true;
                break;
        }
    }


    void Update()
    {
        if (part_1)
        {
            patchwork_productions.alpha += Time.deltaTime * 2;

            if (patchwork_productions.alpha == 1)
            {
                part_1 = false;
                StartCoroutine(WaitTime(2, 2));
            }
        }

        else if (part_2)
        {
            patchwork_productions.alpha -= Time.deltaTime * 2;

            if (patchwork_productions.alpha == 0)
            {
                part_2 = false;
                part_3 = true;
            }
        }

        else if (part_3)
        {
            wildlife_studios.alpha += Time.deltaTime * 2;

            if (wildlife_studios.alpha == 1)
            {
                part_3 = false;
                StartCoroutine(WaitTime(4, 2));
            }
        }

        else if (part_4)
        {
            wildlife_studios.alpha -= Time.deltaTime * 2;

            if (wildlife_studios.alpha == 0)
            {
                part_4 = false;
                part_5 = true;
            }
        }

        else if (part_5)
        {
            splash_bakcground.alpha -= Time.deltaTime;

            if (splash_bakcground.alpha == 0)
            {
                part_5 = false;

                splash_bakcground.gameObject.SetActive(false);
                patchwork_productions.gameObject.SetActive(false);
                wildlife_studios.gameObject.SetActive(false);

                Destroy(gameObject);
            }
        }
    }
}