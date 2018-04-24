using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HouseIntro : MonoBehaviour {

    private string[] dialogue;
    private string[] names;
    private string[] anim;
    private GameObject dialogueBox;
    private Text title;
    private GameObject button;
    private int current = 0;
    private Animator animator;

    // Use this for initialization
    void Start() {
        dialogueBox = GameObject.Find("Dialogue");
        GameObject.Find("ButtonOne").SetActive(false);
        button = GameObject.Find("ButtonTwo");
        title = GameObject.Find("Name").GetComponent<Text>();
        animator = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name.Equals("House Level")) {
            dialogue = new string[6];
            names = new string[6];
            anim = new string[6];
            names[0] = "Horse";
            dialogue[0] = "It's about to turn dark and this looks like a great place to stay!";
            names[1] = "Tiger";
            dialogue[1] = "Alas it would appear there are robbers inside.";
            anim[1] = "HouseAnim";
            names[2] = "Horse";
            dialogue[2] = "Well then, all we've got to do is scare them away! How about we make some noise, knock over some furniture or something.";
            anim[2] = "HouseAnim2";
            names[3] = "Horse";
            dialogue[3] = "That'll get some of them heading our way, then we just have to hide somewhere, then pop out behind them and sing!";
            anim[3] = "HouseAnim3";
            names[4] = "Duck";
            dialogue[4] = "Ooh I'm really small too, so they probably won't mind me walking about, I can probably distract them for a little while to help you get past some of them with my quacks!";
            anim[4] = "HouseAnim4";
            names[5] = "Cat";
            dialogue[5] = "I suppose that's our only option if we want to stay here.";
            anim[5] = "HouseAnim5";

        }
        else if (SceneManager.GetActiveScene().name.Equals("Shed Level")) {
            dialogue = new string[4];
            names = new string[4];
            anim = new string[4];
            names[0] = "Horse";
            dialogue[0] = "It's about to turn dark and this seems like it'll shelter us!";

            names[1] = "Horse";
            dialogue[1] = "However it seems there are some no-do-gooders in there!";
            anim[1] = "ShedAnim";

            names[2] = "Horse";
            dialogue[2] = "Well thenI guess what we've got to do is scare them away! How about we make some noise by knocking over that shelf.";
            anim[2] = "ShedAnim2";

            names[3] = "Horse";
            dialogue[3] = "That'll get them heading that way, then we just have to hide somewhere, then pop out behind them and sing!";
            anim[3] = "ShedAnim3";
        }
        GameState.isTalking = true;
        title.text = names[0];
        dialogueBox.GetComponentInChildren<Text>().text = dialogue[0];
        button.GetComponentInChildren<Text>().text = "Continue";
    }

    // Update is called once per frame
    void Update() {
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            current++;
            Debug.Log(current);
            if (current < dialogue.Length) {
                dialogueBox.GetComponentInChildren<Text>().text = dialogue[current];
                title.text = names[current];
                animator.Play(anim[current]);
            }
            else {
                GameState.isTalking = false;
                //GameObject.Find("ButtonOne").SetActive(true);
                GameObject.Find("DialogueBox").SetActive(false);                
                gameObject.SetActive(false);
            }
        }
    }
}
