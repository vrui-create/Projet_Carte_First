using UnityEngine;

//Se programme permet d'utiliser le song dans la scéne.

public class S_Sound : MonoBehaviour
{
    [SerializeField] AudioClip List_ClipSound; // En utiliser 
    public AudioSource audioSource_GameObject;
    void Start() // initialiser tout les valeurs
    {
        DontDestroyOnLoad(gameObject);
        audioSource_GameObject = GetComponent<AudioSource>();
        audioSource_GameObject.clip = List_ClipSound;
        playsound(true);
    }

    public void playsound(bool play) // Fonction permet de lancer ou nom notre musique background
    {
        if (audioSource_GameObject != null)
        {
            if (play)
            {
                audioSource_GameObject.Play();
            }
            else
            {
                audioSource_GameObject.Stop();
            }
        }
    }
    public void ChangeVolume(float value) // elle est utiliser par le script S_Slide_Volume, afin de changer le volume de notre song.
    {
        audioSource_GameObject.volume = value;
    }
}
