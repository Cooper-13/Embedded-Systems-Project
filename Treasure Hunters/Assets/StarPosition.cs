using UnityEngine;
using System.IO;


public class StarPosition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        name = gameObject.name;
        string path = "Assets/Resources/starPosition.txt";
        if (name.IndexOf("1") > -1)
        {
            path = "Assets/Resources/starPosition1.txt";
        }
        if (name.IndexOf("2") > -1)
        {
            path = "Assets/Resources/starPosition2.txt";
        }
        if (name.IndexOf("3") > -1)
        {
            path = "Assets/Resources/starPosition3.txt";
        }
        if (name.IndexOf("4") > -1)
        {
            path = "Assets/Resources/starPosition4.txt";
        }
        if (name.IndexOf("5") > -1)
        {
            path = "Assets/Resources/starPosition5.txt";
        }

        string X = gameObject.transform.position.x.ToString();
        string Y = gameObject.transform.position.y.ToString();
        string Z = gameObject.transform.position.z.ToString();
        //Write some text to the positions.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(X + " " + Z);
        writer.Close();
    }
}
