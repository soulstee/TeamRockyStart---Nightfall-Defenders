using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //wave ended means preparation mode is active
    public static bool waveEnded = true;

    public GameObject[] targets;
    public float laneDistanceTolerance = 1f;

    private void Start(){
        instance = this;
    }

    //Search for target within y-position to find correct lane (adjust float value)
    public Transform FindTarget(Transform enemy){
        foreach(GameObject t in targets){
            if(Mathf.Abs(t.transform.position.y - enemy.position.y) < laneDistanceTolerance){
                return t.transform;
            }
        }
        Debug.Log("Failed to find target.");
        return null;
    }
}
