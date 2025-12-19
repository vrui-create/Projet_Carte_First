using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class S_Game_Principale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler //veiller a implémenter c'est 2 erreur avec clickDroit>
{
    [SerializeField] bool Securiser { set; get; }
    GameObject Target_move;
    Vector2 Self_Position;

    [SerializeField] S_Game_Principale[] Stack_Script;       //Manier d'enregistrer dans la liste tous les scripte, afin quand utiliser la couroutine est d'invoquer individuellement a l'aide du delay.
    [SerializeField] GameObject Ref_Prefable;               //Cette variable a gauche: permet de spawn un UI dans le canvas
    public bool Deplacement_au_centre { set; get; }     //Variable qui sécuriser la fonction du update
    public string Action_Animation { set; get; }        //Maniére noter le nom des animation, et pour pouvoir le transmêttre a tout les script
    Rigidbody2D _rb;
    Animator zz;

    void Start() //Grand majeur partie de la fonction start, c'est quelle chercher sur son objet c'est variable
    {
        Target_move = GameObject.Find("Target_Center");
        _rb = GetComponent<Rigidbody2D>();
        zz = GetComponent<Animator>();
        if (zz == null) Debug.LogWarning($"Attention un animator n'est pas présent, veiller a vêrifier dans {this}");
        Self_Position = _rb.position;
        Deplacement_au_centre = false;
        Securiser = false;
    }
    public void UI_Lance_Animation(string rr)// Dans celui-ci est utiliser pour faire un feedBack en fonction de l'action du joueur.
    {
        if (Securiser == false)
        {
            switch (rr)
            {
                case "Scene_Vats":
                    // La Variable Action_Animation est une maniére d'optimiser mon programme, ça fonction est d'initialiser le bon nom animation puis a le transmêttre a d'autres cartes sur la scene
                    Action_Animation = "Eject_Carte";
                    Deplacement_au_centre = true; // Cette variable "Deplacement_au_centre", est utiliser dans le Update pour faire une boucle de répétition de déplacement vers le mileur de la map. 
                    zz.SetTrigger("Trigger_Bouton_Agrandir"); // Lance un Feedback animation
                    StartCoroutine("Delay_enlever_les_carte"); // permet de lancer la couroutine Delay_enlever_les_carte
                    break;

                case "S":
                    Action_Animation = "Eject_Carte";
                    Deplacement_au_centre = true;
                    StartCoroutine("Delay_enlever_les_carte");
                    break;

                case "Remêttre_les_cartes":
                    Action_Animation = "Ré_integre_la_carte"; //la en note quelle ordre en strig a distribuer au autres carte
                    StartCoroutine("Delay_enlever_les_carte");
                    break;

                case "Ré_integre_la_carte":
                    zz.SetTrigger("Trigger_Bouton_Ajouter");
                    break;

                case "Eject_Carte":
                    Securiser = true;
                    zz.SetTrigger("Trigger_Bouton_delete");
                    Invoke("securiser", 2);
                    break;
            }
        }
        else Debug.LogWarning("securiser activer");
    }
    void securiser() // cette fonction et utiliser pour a la fois ramener lui même a son emplacement habituelle, et éviter que le joueur spam le bouton
    {
        Securiser = false;
        _rb.position = Self_Position;
    }

    private IEnumerator Delay_enlever_les_carte() // manier élégant de lancer individuellement les animation de tout les cartes présent sur la scene.
    {
        int i = 0;

        while (i < Stack_Script.Length && Stack_Script[i] != this)
        {
            yield return new WaitForSecondsRealtime(0.1f); // c'est le seul moyen de faire un délay du programme
            if (Stack_Script[i] != this)
            {
                Stack_Script[i].UI_Lance_Animation(Action_Animation);
            }
            i++;

        }
        Securiser = false;

    }

    void pourchasseTarget()
    {
        float ERT = Vector2.Distance(Target_move.transform.position, this.transform.position);     //Permet de calculer la distance entre le target_move et sa position
        if (ERT <= 120)                                                                              //Ceux code permet de vêrifier si la distance entre sa position et le centre de la scéne.
        {
            Deplacement_au_centre = false;
            zz.SetTrigger("Trigger_Bouton_Agrandir");
            Invoke("securiser", 2);                         //          <------------------------------------Ceux code permet d'appeler la fonction "securiser", dans 2sec
        }
        _rb.position = Vector2.Lerp(this.transform.position, Target_move.transform.position, Time.deltaTime * 8);           //permet de déplacer elle même jusqu'au milieu de la map.
    }

    public void instance_Prefable() //cette fonction permet d'ajouter d'ajouter un préfable UI
    {
        GameObject OP = GameObject.Find("Canvas");      //En Créer une variable OP, Ce code nous permet de chercher dans la scene un objet"Canvas".
        // Ici Nous utilison le Instantiate pour spawn un object, et avec le OP, en attacher notre Prefable sur le canvas.
        GameObject RR = GameObject.Instantiate(Ref_Prefable, OP.transform);
    }


    private void Update()
    {
        if (Target_move != null && Deplacement_au_centre) pourchasseTarget(); // Sécuriter si Target_move et présent et que le Deplacement_au_centre est en true
    }





    public void OnPointerEnter(PointerEventData eventData) //ce code nous permet de cette un événement, par exemple si la sourie passer sur notre objet, allors se programme se lance
    {
        zz.SetBool("Bool_Mouse_Surevol", true); // Le zz, est une variable de type Animator, et ce code permet d'utiliser le nom de la variable plus change sa valeur a true
        //En Bref c'est un code permetant de faire une animation FeedBack
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        zz.SetBool("Bool_Mouse_Surevol", false);
    }
}
