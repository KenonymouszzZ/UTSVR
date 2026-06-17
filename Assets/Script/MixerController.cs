using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the Audio Settings panel (World Space Canvas).
/// Sliders adjust BGM/SFX volume, toggles mute each channel,
/// and buttons let the player test the SFX or reset everything.
/// </summary>
public class AudioSettings : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource sfx;

    [Header("Sliders")]
    [SerializeField] private Slider sliderBGM;
    [SerializeField] private Slider sliderSFX;

    [Header("Toggles (Mute)")]
    [SerializeField] private Toggle toggleBGM;
    [SerializeField] private Toggle toggleSFX;

    [Header("Buttons")]
    [SerializeField] private Button buttonTestSFX;

    [Header("Clips")]
    [SerializeField] private AudioClip clickSound;

    [Header("Labels (Percent)")]
    [SerializeField] private TextMeshProUGUI textBGM;
    [SerializeField] private TextMeshProUGUI textSFX;

    [Header("Visual Feedback")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color mutedColor = new Color(0.85f, 0.25f, 0.25f);

    [Header("Defaults")]
    [SerializeField] private float defaultVolume = 1f;

    void Start()
    {
        sliderBGM.value = defaultVolume;
        sliderSFX.value = defaultVolume;

        bgm.volume = defaultVolume;
        sfx.volume = defaultVolume;

        toggleBGM.isOn = false;
        toggleSFX.isOn = false;

        UpdateUI();
    }

    // ========================
    // SLIDERS
    // ========================
    public void SetBGMVolume(float value)
    {
        if (!toggleBGM.isOn) // hanya ubah jika tidak mute
        {
            bgm.volume = value;
        }

        UpdateUI();
    }

    public void SetSFXVolume(float value)
    {
        if (!toggleSFX.isOn)
        {
            sfx.volume = value;
        }

        UpdateUI();
    }

    // ========================
    // TOGGLES (MUTE)
    // ========================
    public void ToggleBGM()
    {
        bool isMuted = toggleBGM.isOn;

        bgm.volume = isMuted ? 0f : sliderBGM.value;
        sliderBGM.interactable = !isMuted;

        UpdateUI();
    }

    public void ToggleSFX()
    {
        bool isMuted = toggleSFX.isOn;

        sfx.volume = isMuted ? 0f : sliderSFX.value;
        sliderSFX.interactable = !isMuted;

        UpdateUI();
    }

    // ========================
    // BUTTONS
    // ========================

    // TEST SFX — play a one-shot clip (only when SFX is not muted)
    public void TestSFX()
    {
        if (!toggleSFX.isOn)
        {
            sfx.PlayOneShot(clickSound);
        }
    }

    // RESET — kembalikan slider, toggle, dan volume ke kondisi awal
    public void ResetAudio()
    {
        sliderBGM.value = defaultVolume;
        sliderSFX.value = defaultVolume;

        toggleBGM.isOn = false;
        toggleSFX.isOn = false;

        bgm.volume = defaultVolume;
        sfx.volume = defaultVolume;

        sliderBGM.interactable = true;
        sliderSFX.interactable = true;

        UpdateUI();
    }

    // ========================
    // UPDATE UI
    // ========================
    void UpdateUI()
    {
        float bgmValue = toggleBGM.isOn ? 0f : sliderBGM.value;
        float sfxValue = toggleSFX.isOn ? 0f : sliderSFX.value;

        // Tampilkan volume dalam persen
        textBGM.text = (bgmValue * 100f).ToString("F0") + "%";
        textSFX.text = (sfxValue * 100f).ToString("F0") + "%";

        // Feedback warna: label persen jadi merah saat mute
        textBGM.color = toggleBGM.isOn ? mutedColor : normalColor;
        textSFX.color = toggleSFX.isOn ? mutedColor : normalColor;

        // Kunci tombol Test SFX selama SFX di-mute
        buttonTestSFX.interactable = !toggleSFX.isOn;
    }
}
