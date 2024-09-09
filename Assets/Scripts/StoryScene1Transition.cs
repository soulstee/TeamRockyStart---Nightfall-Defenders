using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryScene1Transition : MonoBehaviour
{
    // First key press will take you to StoryScene_2

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("StoryScene_2");
        }
    }
}
