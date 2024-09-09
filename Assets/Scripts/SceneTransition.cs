using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Method to load StoryScene_1
    public void LoadStoryScene1()
    {
        SceneManager.LoadScene("StoryScene_1");
    }
}
