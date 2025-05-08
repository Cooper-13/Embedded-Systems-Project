using UnityEngine;
using System.IO;

public class PlayerPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        // Clear the contents of the playerPosition.txt file
        string path = "Assets/Resources/playerPosition.txt";

        string X = gameObject.transform.position.x.ToString();
        string Y = gameObject.transform.position.y.ToString();
        string Z = gameObject.transform.position.z.ToString();
        Vector3 eRotation = gameObject.transform.eulerAngles;
        //Write some text to the positions.txt file

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(X + " " + Z + " " + Y + " " + eRotation.y%360);
        writer.Close();

    }
}