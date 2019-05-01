using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    public MeshRenderer Player_Display;
    public MeshRenderer Invisible;
    public Text Score_Text;
    public Text Win_Text;
    public int NeededKey;
    public float MaxTime;
    public int Penality;
    private int GainPoints;
    private Rigidbody Player_RB;
    private float TimeLeft;
    static private List<string> Fruits;
    static private int Score;

    void Start()
    {
        Player_RB = GetComponent<Rigidbody>();
        Score = 0;
        Fruits = new List<string> { };
        Score_Text.text = "Score : " + Score.ToString();
        Win_Text.text = "";
        TimeLeft = MaxTime;
        GainPoints = 0;
    }

    void Update()
    {
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0)
        {
            Lose();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            if (transform.localEulerAngles.y == 0)
            {
                StartCoroutine(Move_Forward());
            }
            if (transform.localEulerAngles.y == 90)
            {
                StartCoroutine(Move_Right());
            }
            if (transform.localEulerAngles.y == 270)
            {
                StartCoroutine(Move_Left());
            }
            if (transform.localEulerAngles.y == 180)
            {
                StartCoroutine(Move_Back());
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            Vector3 Rotate_Left = new Vector3(0, -90, 0);
            Quaternion Rotate_Left_Q = Quaternion.Euler(Rotate_Left);
            Player_RB.MoveRotation(Player_RB.rotation * Rotate_Left_Q);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            Vector3 Rotate_Right = new Vector3(0, 90, 0);
            Quaternion Rotate_Right_Q = Quaternion.Euler(Rotate_Right);
            Player_RB.MoveRotation(Player_RB.rotation * Rotate_Right_Q);
        }
    }

    void OnTriggerEnter(Collider other)             // Quand on rentre dans un collectible
    {
        if (other.gameObject.CompareTag("Coin"))    // Pour les Coins
        {
            other.gameObject.SetActive(false);
            Set_Score(1);                           // On ajouter les points au compteur
        }
        else if (other.gameObject.CompareTag("Gem"))        // Pour les Gem
        {
            other.gameObject.SetActive(false);
            Set_Score(3);
        }
        else if (other.gameObject.CompareTag("Clé"))    // Les clés à attraper pour la fin du niveau
        {
            other.gameObject.SetActive(false);
            NeededKey -= 1;
        }
        else if (other.gameObject.CompareTag("Banane") || other.gameObject.CompareTag("Melon") || other.gameObject.CompareTag("Pastèque")
            || other.gameObject.CompareTag("Raisin") || other.gameObject.CompareTag("Pomme"))
            // Les fruits à récolter le long des niveaux
        {
            Fruits.Add(other.gameObject.tag);   // On ajoute le fruit à la liste
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Lethargy Pill"))  // La mauvaise pillule
        {
            other.gameObject.SetActive(false);
            StartCoroutine(SpeedTime());        // On accélère le temps
        }
        else if (other.gameObject.CompareTag("Bouncy Pill"))    // La bonne pillule
        {
            other.gameObject.SetActive(false);

        }
        else if (other.gameObject.CompareTag("Sun Glass"))  // Lunette de soleil
        {
            other.gameObject.SetActive(false);
            StartCoroutine(SeeInvisible());     // On affiche les blocs invisible
        }
        else if (other.gameObject.CompareTag("HourGlass"))  // Le sablier
        {
            other.gameObject.SetActive(false);
            TimeLeft = MaxTime - TimeLeft;      // On inverse le temps restant par rapport au temps max ( retourner le sablier )
        }
        else if (other.gameObject.CompareTag("Spikes") || other.gameObject.CompareTag("Captivator"))    // Si on est touché par des pics ou un Captivator
        {
            Lose(); // On perd
        }
    }

    void OnCollisionEnter(Collision collision)      // Quand on touche un bloc
    {
        if (collision.gameObject.CompareTag("Flimsy"))  // Les blocs Flimsy sont ceux qui se casse lors du passage du joueur
        {
            StartCoroutine(DelFlimsy(collision));   // On fait disparaître le bloc
        }
        else if (collision.gameObject.CompareTag("End"))
        {
            Win();
        }
    }

    private void OnCollisionStay(Collision collision)   // Si on reste sur le bloc
    {
        if (collision.gameObject.CompareTag("Stop Time"))   // Le bloc qui stop le temps
        {
            TimeLeft += Time.deltaTime;     // On compense la diminution du temps
        }
    }

    private void Win()
    {
        File.AppendAllText(@"High_Score.txt", DateTime.Now + ":" + Score + Environment.NewLine);
    }

    void Lose()     // Si on perd
    {
        Score -= (GainPoints + Penality);       // On diminue le score
        if (Score >= 0)     // Si on a pas assez de points pour continuer
        {
            Application.LoadLevel(Application.loadedLevel);    // On restart le niveau
        }
    }

    void Save()
    {
        StreamWriter sw = new StreamWriter("Save.txt");
        sw.WriteLine(SceneManager.GetActiveScene().name + ":" + Score);
        sw.Close();
    }

    void Set_Score(int Points)  // Modification et affichage du score
    {
        Score += Points;    // On ajoute les points
        GainPoints += Points;
        Score_Text.text = "Score : " + Score.ToString();    // On affiche le score
    }

    IEnumerator SeeInvisible()      // Pour voir les blocs invisible
    {
        Invisible.enabled = true;       // On les affiche
        yield return new WaitForSeconds(10);        // Attente de 10 secondes
        Invisible.enabled = false;      // On les fait disparaître
    }

    IEnumerator SpeedTime()     // On accélère le temps
    {
        for (int i = 0; i < 240; i++)
        {
            TimeLeft += Time.deltaTime;     // On ajoute du temps supplémentaire au timer
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator DelFlimsy(Collision collision)  // Faire disparaître le bloc Flimsy
    {
        yield return new WaitForSeconds(3);     // On attend 3 secondes
        collision.gameObject.SetActive(false);  // On le désactive
    }

    IEnumerator Move_Left()
    {
        yield return new WaitForSeconds(0);
        Vector3 Target = new Vector3(transform.position.x-2, transform.position.y, transform.position.z);
        while (transform.position.x > Target.x + 0.1f)
        {
            yield return new WaitForSeconds(0.02f);
            transform.position = Vector3.Lerp(transform.position, Target, 0.01f);
        }
    }

    IEnumerator Move_Right()
    {
        yield return new WaitForSeconds(0);
        Vector3 Target = new Vector3(transform.position.x+2, transform.position.y, transform.position.z);
        while (transform.position.x < Target.x + 0.1f)
        {
            yield return new WaitForSeconds(0.02f);
            transform.position = Vector3.Lerp(transform.position, Target, 0.01f);
        }
    }

    IEnumerator Move_Forward()
    {
        yield return new WaitForSeconds(0);
        Vector3 Target = new Vector3(transform.position.x, transform.position.y, transform.position.z+2);
        while (transform.position.z < Target.z + 0.1f)
        {
            yield return new WaitForSeconds(0.02f);
            transform.position = Vector3.Lerp(transform.position, Target, 0.01f);
        }
    }

    IEnumerator Move_Back()
    {
        yield return new WaitForSeconds(0);
        Vector3 Target = new Vector3(transform.position.x, transform.position.y, transform.position.z-2);
        while (transform.position.z > Target.z + 0.1f)
        {
            yield return new WaitForSeconds(0.02f);
            transform.position = Vector3.Lerp(transform.position, Target, 0.01f);
        }
    }
}