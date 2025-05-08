using UnityEngine;
using System.IO;

public class TestScript2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string path = "Assets/Resources/positions.txt";
        //Write some text to the positions.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(gameObject.transform.position);
        writer.Close();
        // //Re-import the file to update the reference in the editor
        // AssetDatabase.ImportAsset(path); 
        // TextAsset asset = Resources.Load("positions");
        //Print the text from the file
        // Debug.Log(asset.text);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}