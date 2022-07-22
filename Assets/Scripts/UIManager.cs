using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private int _score;

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Sprite[] _spriteLives;

    [SerializeField]
    private Image _livesIMG;


    void Start()
    {
        _livesIMG.sprite = _spriteLives[3];
        _scoreText.text = "Score: 0";
        
    }

    public void ChangeLives(int currentLives)
    {
        _livesIMG.sprite = _spriteLives[currentLives];
    }

    public void AddScore(int points)
    {
        _score += points;
        _scoreText.text = "Score: " + _score;
    }

}
