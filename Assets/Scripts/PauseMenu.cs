using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;

    public static bool menuEnabled = false;

    public GameObject[] confirmMenu;

    private void Awake(){
        GameObject[] objs = new GameObject[3];
        objs[0] = confirmMenu[0];
        objs[1] = confirmMenu[1];
        objs[2] = menu;
        foreach(GameObject g in objs){
            Button[] buttons = g.GetComponentsInChildren<Button>();
            foreach(Button b in buttons){
                b.onClick.AddListener(PlayButtonNoise);
            }
        }
    }

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
        GameManager.instance.ResetGame();
        Continue();
    }

    public void PlayButtonNoise(){
        AudioManager.instance.PlayNoise("Button");
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
