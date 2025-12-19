using UnityEngine;
using UnityEngine.UI;

public class S_Slide_Volume : MonoBehaviour
{
    GameObject Objet_Sound_Manager;
    public Slider volumeSlider;
    S_Sound sound_Manager;


    void Start()
    {
        volumeSlider = GetComponent<Slider>(); // permet d'initialiser la variable en slider

        Objet_Sound_Manager = GameObject.Find("@Sound"); // aussi, elle intialiser Objet_Sound_Manager pemettant de d'utiliser ou changer le volume du source audio

        if (Objet_Sound_Manager != null)
        {
            sound_Manager = Objet_Sound_Manager.GetComponent<S_Sound>(); // initialiser et chercher le script "S_Sound" dans notre Objet_Sound_Manager
        }
        volumeSlider.value = sound_Manager.audioSource_GameObject.volume;


    }

    public void Slide_ChangeVolume(float value) // permet d'utiliser le slider afin de changer le volume du song de jeux
    {
        if (sound_Manager != null) sound_Manager.ChangeVolume(value);
        else Debug.LogWarning("sound_Manager not found in the scene.");
        print("Volume changed to: " + value);
    }
}
