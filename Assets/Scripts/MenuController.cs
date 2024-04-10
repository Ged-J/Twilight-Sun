using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private PlayerController playerController;
    /*private Animator playerAnimator;*/
    //public UICollection menu;
    public UICollection settings;
    //public TMP_Dropdown qualitydropdown;
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider;
    
    private void Awake()
    {
          
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        /*playerAnimator = playerController.GetComponent<Animator>();*/
        // qualitydropdown.value = 2;
       // SetQuality(2);
        Cursor.lockState = CursorLockMode.None;
        //SceneTransitions.Fadein();
        //MusicManager.SetMusic(0);
        //StartCoroutine(MusicManager.FadeIn(10 ,0.8f));
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.00");
        //volumeSlider.value = volume;
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
    }

    public void SetQuality(int index)
    {
        //QualitySettings.SetQualityLevel(index);
       // if (index == 3)
       // {
        //    pp.profile.GetSetting<Bloom>().active = false;
          //  pp.profile.GetSetting<AmbientOcclusion>().active = false;
       // }
        //else
        //{
        //    pp.profile.GetSetting<Bloom>().active = true;
        //    pp.profile.GetSetting<AmbientOcclusion>().active = true; 
       // }
    }
    
    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
    
    private void Update()
    {
        //if (Input.GetButtonDown("Menu"))
        //{
        //    if (!settings.Visible)
        //    {
        //        if (menu.Visible)
        //        {
        //            //SFXManager.PlaySFX("cancel");
        //            Time.timeScale = 1;
        //            playerAnimator.enabled = true;
        //            menu.Hide();
        //            MusicManager.SetVolume(0.8f);
//
//
        //        }
        //        else
        //        {
        //            //SFXManager.PlaySFX("select");
        //            playerAnimator.enabled = false;
        //            Time.timeScale = 0;
        //            menu.Show();
        //            MusicManager.SetVolume(0.2f);
//
        //        }
        //    }
        //}

        /*if (Input.GetButtonDown("Settings"))
        {
            //if (!menu.Visible)
            //{
                if (settings.Visible)
                {
                    SFXManager.PlaySFX("cancel1");
                    playerController.canCast = true;
                    Time.timeScale = 1;
                    
                    playerAnimator.enabled = true;
                    settings.Hide();
                    MusicManager.SetVolume(0.8f);
                }
                else
                {
                    SFXManager.PlaySFX("click1");
                    playerController.canCast = false;
                    playerAnimator.enabled = false;
                    Time.timeScale = 0;
                    settings.Show();
                    MusicManager.SetVolume(0.2f);
                }
            //}
        }*/
    }
}