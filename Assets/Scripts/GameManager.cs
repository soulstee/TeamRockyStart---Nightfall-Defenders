using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GridManager2D gridManager;

    public static bool waveEnded = true;

    private void Start(){
        instance = this;
    }
}
