using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;

    public static bool menuEnabled = false;

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Continue();
        }
    }

    public void Quit(){
        Application.Quit();
    }

    public void Continue(){
        menuEnabled = !menuEnabled;
        menu.SetActive(menuEnabled);

        if(menuEnabled){
            Time.timeScale = 0;
        }else{
            Time.timeScale = 1;
        }
    }
}
