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
            collectedCount++;

            // Optionally, print to the console
            Debug.Log("Star collected! You've collected " + collectedCount + " stars.");

            // Destroy the collectible
            Destroy(gameObject);
            // moving to next scene with 2  specific stars collected
            // revise this logic to suit your needs
            if (collectedCount == 2)
            {
                collectedCount = 0; // Reset the counter
                int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                int nextSceneIndex = (currentSceneIndex == 0) ? 1 : 0;
                
                Debug.Log("You collected all the stars! Loading next scene...");
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
}