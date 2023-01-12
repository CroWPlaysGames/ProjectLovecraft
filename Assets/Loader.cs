using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadCoroutine());
    }
    IEnumerator LoadCoroutine()
    {
        yield return new WaitForSeconds(1);
        Load();
    }
    void Load() 
    {
        gameObject.GetComponent<GameManager>().enabled = true;
    }
}
