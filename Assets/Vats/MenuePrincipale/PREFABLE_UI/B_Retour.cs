using UnityEngine;
using UnityEngine.SceneManagement;

public class B_Retour : MonoBehaviour
{
    S_Game_Principale[] Lol;
    public GameObject Objet_Destroy;
    bool securiser = false;

    void Start()
    {
        Invoke("secure_event_boutton", 2f); // ce code permet de lancer une fonction "secure_event_boutton" dans les 2sec
    }

    public void DestroyPrefable() // ce code permet de retournée dans le menue principale + a se détruire lui même + a lancéer les animations de tous les objets qui ont le script S_Game_Principale
    {
        if (securiser)
        {
            Lol = FindObjectsOfType<S_Game_Principale>();
            for (int i = 0; i < Lol.Length;)
            {
                Lol[i].Deplacement_au_centre = false;
                Lol[i].Action_Animation = "Ré_integre_la_carte";
                Lol[i].UI_Lance_Animation("Ré_integre_la_carte");
                i++;
            }
            if (Objet_Destroy != null) Destroy(Objet_Destroy);
        }

    }
    void secure_event_boutton() // Cette fonction est juste utiliser pour sécuriser la fonction DestroyPrefable
    {
        securiser = true;
    }

    public void UI_Load_Game() // Fonction permettant de lancéer une scéne a partire du nom de la scéne.
    {
        SceneManager.LoadScene("Main");
    }
    public void UI_ExitGame()
    {
        Application.Quit(); // Ce code est utiliser pour quitter le jeux, elle fonctionne que dans un builde, ou pour les amateurs un exe
    }
}
