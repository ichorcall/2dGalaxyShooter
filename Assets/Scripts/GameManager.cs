using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverScreen;
    [SerializeField]
    private GameObject _restartText;

    private bool _gameOver = false;
    public void Update()
    {
        if (_gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(1);
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _gameOver = true;
        _restartText.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    public IEnumerator GameOverFlicker()
    {
        while (_gameOver == true)
        {
            _gameOverScreen.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _gameOverScreen.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }

    }
}
