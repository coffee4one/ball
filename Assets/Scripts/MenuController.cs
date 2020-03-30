using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
   public void ExitGame(){
       Application.Quit();
   }





   public void LoadEscene(string Scena){
       SceneManager.LoadScene(Scena, LoadSceneMode.Single);
   }
}
