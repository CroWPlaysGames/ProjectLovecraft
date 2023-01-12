using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using System.IO;

public class Options : MonoBehaviour
{
    [Header("Settings")]
    public float effects_volume;
    public float music_volume;
    public float voice_volume;

    public bool enable_particle_effects;
    public bool subtitles;
    private string saveFilePath;
    private string decksFilePath;
    private string settingsFilePath;
    private string file_name = "options.data";

    [Header("GameObject References")]
    public Slider effects_slider;
    public Slider music_slider;
    public Slider voice_slider;
    public Toggle particle_effects_button;
    public Toggle subtitles_button;

    public TextMeshProUGUI effects_value;
    public TextMeshProUGUI music_value;
    public TextMeshProUGUI voice_value;

    public AudioMixer master;
    public AudioManager audio_manager;

    [HideInInspector] public bool multiplayer_tutorial = true;
    [HideInInspector] public bool deck_builder_tutorial = true;
    [HideInInspector] public bool card_manager_tutorial = true;
    [HideInInspector] public bool hotseat_tutorial = true;


    void Awake()
    {
        saveFilePath = Application.persistentDataPath + "save.data";
        decksFilePath = Application.persistentDataPath + "decks.data";
        settingsFilePath = Application.persistentDataPath + "options.data";
        if (File.Exists(settingsFilePath))
        {
            using (var stream = File.Open(settingsFilePath, FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    effects_slider.value = float.Parse(reader.ReadLine());
                    music_slider.value = float.Parse(reader.ReadLine());
                    voice_slider.value = float.Parse(reader.ReadLine());
                    string enable_particle_effects_set = reader.ReadLine();
                    string subtitles_set = reader.ReadLine();
                    string multiplayer_tutorial_set = reader.ReadLine();
                    string deck_builder_tutorial_set = reader.ReadLine();
                    string card_manager_tutorial_set = reader.ReadLine();
                    string hotseat_tutorial_set = reader.ReadLine();

                    if (enable_particle_effects_set.Equals("True"))
                    {
                        enable_particle_effects = true;
                        particle_effects_button.isOn = true;
                    }

                    else
                    {
                        enable_particle_effects = false;
                        particle_effects_button.isOn = false;
                    }

                    if (subtitles_set.Equals("True"))
                    {
                        subtitles = true;
                        subtitles_button.isOn = true;
                    }

                    else
                    {
                        subtitles = false;
                        subtitles_button.isOn = false;
                    }

                    if (multiplayer_tutorial_set.Equals("False"))
                    {
                        multiplayer_tutorial = false;
                    }                    

                    else
                    {
                        multiplayer_tutorial = true;
                    }

                    if (deck_builder_tutorial_set.Equals("False"))
                    {
                        deck_builder_tutorial = false;
                    }

                    else
                    {
                        deck_builder_tutorial = true;
                    }

                    if (card_manager_tutorial_set.Equals("False"))
                    {
                        card_manager_tutorial = false;
                    }

                    else
                    {
                        card_manager_tutorial = true;
                    }

                    if (hotseat_tutorial_set.Equals("False"))
                    {
                        hotseat_tutorial = false;
                    }

                    else
                    {
                        hotseat_tutorial = true;
                    }
                }
            }
        }

        else
        {
            effects_volume = 5;
            music_volume = 5;
            voice_volume = 5;
            enable_particle_effects = true;
            subtitles = false;
        }

        audio_manager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
    }

    void Update()
    {
        // Update sound values in UI
        effects_value.text = Math.Round(effects_slider.value).ToString();
        music_value.text = Math.Round(music_slider.value).ToString();
        voice_value.text = Math.Round(voice_slider.value).ToString();

        // Manage sound values
        effects_volume = effects_slider.value;
        music_volume = music_slider.value;
        voice_volume = voice_slider.value;

        // Adjust slider to whole numbers
        effects_slider.value = (int)Math.Round(effects_slider.value);
        music_slider.value = (int)Math.Round(music_slider.value);
        voice_slider.value = (int)Math.Round(voice_slider.value);

        // Manage mixer volumes
        master.SetFloat("effectsVol", Mathf.Log10(effects_volume / 10) * 30);
        master.SetFloat("musicVol", Mathf.Log10(music_volume / 10) * 20);
        master.SetFloat("voiceVol", Mathf.Log10(voice_volume / 10) * 20);

        if (particle_effects_button.isOn)
        {
            enable_particle_effects = true;
        }

        else
        {
            enable_particle_effects = false;
        }

        if (subtitles_button.isOn)
        {
            subtitles = true;
        }

        else
        {
            subtitles = false;
        }        
    }

    public void SaveSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            File.Delete(settingsFilePath);
        }

        using (var stream = File.Open(settingsFilePath, FileMode.Create))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine(effects_volume);
                writer.WriteLine(music_volume);
                writer.WriteLine(voice_volume);
                writer.WriteLine(enable_particle_effects);
                writer.WriteLine(subtitles);
                writer.WriteLine(multiplayer_tutorial);
                writer.WriteLine(deck_builder_tutorial);
                writer.WriteLine(card_manager_tutorial);
                writer.WriteLine(hotseat_tutorial);
            }
        }
    }

    public void ToggleSoundEffect()
    {
        if ((particle_effects_button.isOn && !enable_particle_effects) || (!particle_effects_button.isOn && enable_particle_effects))
        {
            audio_manager.Play("Button Toggle");
        }

        else if ((subtitles_button.isOn && !subtitles) || (!subtitles_button.isOn && subtitles))
        {
            audio_manager.Play("Button Toggle");
        }
    }

    public void PlaySound(string sound)
    {
        audio_manager.Play(sound);
    }

    public void ResetSettings()
    {
        effects_volume = 5;
        music_volume = 5;
        voice_volume = 5;
        enable_particle_effects = true;
        subtitles = false;

        multiplayer_tutorial = true;
        deck_builder_tutorial = true;
        card_manager_tutorial = true;
        hotseat_tutorial = true;

        GameObject.Find("Tutorials").GetComponent<Tutorials>().index = 0;
    }
}
