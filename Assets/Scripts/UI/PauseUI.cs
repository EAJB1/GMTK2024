using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float startVolume;
    
    [SerializeField] Slider[] sliders;

    void Start()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            if (!PlayerPrefs.HasKey(sliders[i].name))
            {
                Save(sliders[i], startVolume);
            }

            Load(sliders[i]);
        }
    }

    public void ChangeVolume(Slider slider)
    {
        SoundManager.instance.audioMixer.SetFloat(slider.name + "Exposed", Mathf.Log10(slider.value) * 20f);
        Save(slider, slider.value);
    }

    void Load(Slider slider)
    {
        slider.value = PlayerPrefs.GetFloat(slider.name);
    }

    void Save(Slider slider, float value)
    {
        PlayerPrefs.SetFloat(slider.name, value);
    }
}
