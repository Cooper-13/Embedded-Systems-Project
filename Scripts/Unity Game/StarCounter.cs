using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;



public class Collectible : MonoBehaviour
{
    // Global counter for collected items
    public static int collectedCount = 0;
    public bool hasCompletedEvent = false; // Flag to indicate event completion


    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Increment the global counter
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            collectedCount++;
            if (currentSceneIndex == 0){
                int collectedCountmod = collectedCount-1;
                if (collectedCountmod == 1){
                    Debug.Log("Star collected! You've collected " + collectedCountmod + " star.");
                }
                if (collectedCountmod > 1){
                    Debug.Log("Star collected! You've collected " + collectedCountmod + " stars.");
                }
                Destroy(gameObject);
                if (collectedCount >= 6){
                    collectedCount = 0; // Reset the counter
                    int nextSceneIndex = (currentSceneIndex +1);
                    currentSceneIndex = nextSceneIndex;
                    Debug.Log("You collected all the stars! Loading next scene...");
                    SceneManager.LoadScene(nextSceneIndex);
                    }
            }
            if (currentSceneIndex == 1){
                if (collectedCount == 1){
                    Debug.Log("Star collected! You've collected " + collectedCount + " star.");
                }
                if (collectedCount > 1){
                    Debug.Log("Star collected! You've collected " + collectedCount + " stars.");
                }                
                Destroy(gameObject);
                if (collectedCount >= 5){
                    collectedCount = 0; // Reset the counter
                    int nextSceneIndex = (currentSceneIndex +1);
                    currentSceneIndex = nextSceneIndex;
                    Debug.Log("You collected all the stars! Loading next scene...");
                    SceneManager.LoadScene(nextSceneIndex);
                    }
            }
            if (currentSceneIndex == 2){
                if (gameObject.name.Contains("Real")) {
                    if (collectedCount <= 2){
                        Debug.Log("Star collected! Look for the other real star within the maze...");
                    }        
                    Destroy(gameObject);
                    if (collectedCount > 2 ){
                        collectedCount = 0; // Reset the counter
                        int nextSceneIndex = (currentSceneIndex +1);
                        Debug.Log("You collected all the stars! Loading final scene...");
                        SceneManager.LoadScene(nextSceneIndex);
                    }
                }
                if (gameObject.name.Contains("Fake")) {
                    Debug.Log("This is a fake star! They won't count towards level completion...");
                    Destroy(gameObject);
                }
            }
            if (currentSceneIndex == 3){
                if (collectedCount == 1){
                    Debug.Log("You've collected all the stars! Sending you back to level 1...");
                    Destroy(gameObject);
                    int nextSceneIndex = (0);
                    currentSceneIndex = nextSceneIndex;
                    SceneManager.LoadScene(nextSceneIndex);

                }
            }
        }
    }
}