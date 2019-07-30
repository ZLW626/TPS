using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBarController : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text grenadeText;
    [SerializeField] private Text shellText;
    [SerializeField] private Slider slider;
    [SerializeField] private Image hpImage;
    [SerializeField] private Text hpText;
    private float maxHpVal = 100;

    // Start is called before the first frame update
    void Start()
    {
        moneyText.text = PlayerPrefs.GetInt("money").ToString();
        ammoText.text = PlayerPrefs.GetInt("money").ToString();
        grenadeText.text = PlayerPrefs.GetInt("grenade").ToString();
        shellText.text = PlayerPrefs.GetInt("shell").ToString();
        int hp = PlayerPrefs.GetInt("hp");
        slider.value = hp / maxHpVal;
        hpImage.fillAmount = slider.value;
        hpText.text = hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoneyText(int val)
    {
        moneyText.text = val.ToString();
    }
    public void SetAmmoText(int val)
    {

    }
    public void SetGrenadeText(int val)
    {

    }
    public void SetShellText(int val)
    {

    }
    public void SetHpBar(int val)
    {
        slider.value = PlayerPrefs.GetInt("hp") / maxHpVal;
        hpImage.fillAmount = slider.value;
        hpText.text = val.ToString();
    }
}
