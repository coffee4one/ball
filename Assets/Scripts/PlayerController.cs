using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioSource audioPlayer;
    public AudioClip poinSound, jumpSound, deadSound;
    public bl_Joystick joistick;
    public GameObject startPoint;
    public GameObject panelGameOver;
    public GameObject panelCongratulations;
     public GameObject uiMobile;
    public bool fin = false;
    public MenuManager menuManager;
   

    float movVertical;
     float movHorizontal;
    public float velocidad = 1.0f;
    public float altitud = 8.0f;
    public bool isJump = false;
    bool pause = false;
    int starts = 0;
    int lifes = 3;
    float totalTime = 120;
    public Text startsText;
    public Text lifesText;
    public Text timeText;
    public Text finalLifesText;
    public Text finalStarsText;
  
   

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_ANDROID
            uiMobile.SetActive(true);
        #endif

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!pause){
            CountDown();
        }

        #if UNITY_ANDROID
            movVertical = joistick.Vertical*0.12f;
            movHorizontal = joistick.Horizontal*0.12f;
        #else
            movVertical = Input.GetAxis("Vertical");
            movHorizontal = Input.GetAxis("Horizontal");
        #endif

        Vector3 movimiento = new Vector3(movHorizontal, 0.0f ,movVertical); 

        rb.AddForce(movimiento*velocidad);

        if(Input.GetKey(KeyCode.Space) && (!isJump)){
            Jump();
        }
    }

    void OnCollisionEnter(Collision collision){
            print(collision.gameObject.name);
            if (collision.gameObject.name == "Floor" || collision.gameObject.name == "Wood")
            {

                isJump = false;
            }
            if (collision.gameObject.name == "Meta" && !fin){


                fin = true;
                FinishedgGame();
            }
    }

    void OnTriggerEnter(Collider collider){
            if (collider.gameObject.name == "star")
            {
                Destroy(collider.gameObject);
                starts += 1;
                startsText.text ="0"+starts.ToString();

                GetComponent<AudioSource>().clip = poinSound;
                GetComponent<AudioSource>().Play();
            }
            if ((collider.gameObject.name == "DeadZone") || (collider.gameObject.name == "Cierra"))
            {
                transform.position = startPoint.transform.position;
                lifes -= 1;
                lifesText.text = "0" + lifes.ToString();

                if (lifes==0)
                {
                    GameOver();
                }
                GetComponent<AudioSource>().clip = deadSound;
                GetComponent<AudioSource>().Play();
            }
            if (collider.gameObject.name == "Meta"){
                FinishedgGame();
            }
    }

     public void PauseGame(){
        pause = !pause;
        rb.isKinematic = pause;
    }

    public void RestardGame(){
        totalTime = 120;
        lifes = 3;
        starts = 0;
        lifesText.text = "03";
        startsText.text = "00";
        transform.position = startPoint.transform.position;
        rb.isKinematic = false;
        pause = false;
    }

        void FinishedgGame(){
        PauseGame();
        menuManager.GoToMenu(panelCongratulations);
        finalLifesText.text = "0" + lifes.ToString();
        finalStarsText.text = "0" + starts.ToString(); 
    }

    public void Jump(){
        Vector3 salto = new Vector3(0,altitud,0);
        rb.AddForce(salto*velocidad);
        isJump = true;
        GetComponent<AudioSource>().clip = jumpSound;
        GetComponent<AudioSource>().Play();
    }

    void GameOver(){
        menuManager.GoToMenu(panelGameOver);
        PauseGame();
    }



    void CountDown(){
        totalTime -= Time.deltaTime;
        int min = Mathf.FloorToInt(totalTime/60f);
        int sec = Mathf.FloorToInt(totalTime-(min*60f));
        timeText.text = string.Format("{0:0}:{01:00}",min,sec);

        if((min == 0) && (sec == 0)){
            GameOver();
        }
    }

   
}
