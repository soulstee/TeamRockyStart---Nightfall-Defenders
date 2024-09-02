using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;

    public static bool menuEnabled = false;

    public GameObject[] confirmMenu;

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && MouseController.mouseMode == MouseController.MouseMode.Default){
            Continue();
        }else if(Input.GetKeyDown(KeyCode.Escape) && MouseController.mouseMode == MouseController.MouseMode.Build){
            GameManager.instance.ChangeToDefaultMode();
        }
    }

    public void Quit(){
        Application.Quit();
    }

    public void Restart(){
        
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

    public void Warning(int confirmID){
        confirmMenu[confirmID].SetActive(true);
    }
}
