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


    [SerializeField]
    private Text _ammoCountText;
    private bool _noAmmo = false;

    [SerializeField]
    private Image _energyBar;

    [SerializeField]
    private Text _waveCountText;
    private IEnumerator _flashWave;

    void Start()
    {
        _livesIMG.sprite = _spriteLives[3];
        _scoreText.text = "Score: 0";
        _waveCountText.enabled = false;
    }

    public void ChangeEnergyBar(float currentEnergy, float maxEnergy)
    {
        _energyBar.fillAmount = currentEnergy /= maxEnergy;

        if (_energyBar.fillAmount >= .75f)
        {
            _energyBar.color = Color.green;
        }
        else if(_energyBar.fillAmount < .75f && _energyBar.fillAmount >= .35f) 
        {
            _energyBar.color = Color.yellow;
        }
        else if (_energyBar.fillAmount < .35f && _energyBar.fillAmount >= 0) 
        {
            _energyBar.color = Color.red;
        }
    }

    public void ChangeLives(int currentLives)
    {
        _livesIMG.sprite = _spriteLives[currentLives];
    }

    public void AddScore(int points)
    {
        _score += points;
        _scoreText.text = "SCORE: " + _score;
    }

    public void ChangeAmmoCount(int ammoCount, int maxAmmoCount)
    {
        _ammoCountText.text = "AMMO: " + ammoCount + " / " + maxAmmoCount;

        if(ammoCount == 0)
        {
            _noAmmo = true;
            _ammoCountText.GetComponent<Text>().color = Color.red;
            StartCoroutine(NoAmmo());
        }
        else
        {
            _noAmmo = false;
            _ammoCountText.GetComponent<Text>().color = Color.white;
        }
    }

    public void ChangeWaveCount(int waveNumber)
    {
        _waveCountText.text = "WAVE " + waveNumber;
        _waveCountText.enabled = true;

        if(_flashWave != null)
        {
            StopCoroutine(_flashWave);
        }

        _flashWave = FlashWave();
        StartCoroutine(_flashWave);
    }

    IEnumerator FlashWave()
    {
        for(int i =0; i< 3; i++)
        {
            yield return new WaitForSeconds(.5f);
            _waveCountText.enabled = false;
            yield return new WaitForSeconds(.5f);
            _waveCountText.enabled = true;
        }

        _waveCountText.enabled = false;
    }

    IEnumerator NoAmmo()
    {
        while (_noAmmo == true)
        {
            yield return new WaitForSeconds(.5f);
            _ammoCountText.enabled = false;
            yield return new WaitForSeconds(.5f);
            _ammoCountText.enabled = true;
        }
    }
}
