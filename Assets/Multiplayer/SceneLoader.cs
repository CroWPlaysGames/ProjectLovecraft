using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        LeanTween.cancelAll(false);
        Destroy(GameObject.Find("Audio Manager").gameObject);
        Destroy(GameObject.Find("Player Decks").gameObject);
        AsyncOperation operation = SceneManager.LoadSceneAsync(0);
    }
}
