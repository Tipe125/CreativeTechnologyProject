using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class PlaybackBot : MonoBehaviour {

    public TextAsset runDataFile;
    private GameObject trackedObject;
    //private Mesh trackedObjectMesh;
    private string runDataFilePath;
    private List<string> fileLines;
    private float numberOfLines;
    private int lineNumber;
    private float steps;
    private float stepsComplete;
    private Vector3 stepMovement;
    private Vector3 stepRotation;
    private string[] words;

	// Use this for initialization
	void Start () {

        runDataFilePath = AssetDatabase.GetAssetPath(runDataFile);
     //   Debug.Log(runDataFilePath);
        fileLines = new List<string>(File.ReadAllLines(runDataFilePath));

        
        //splitting the first line
        words = fileLines[0].Split(' ');

        //assigning the tracked object based on what is in the text file
        trackedObject = GameObject.Find(words[2]);
      //  Debug.Log(trackedObject.gameObject.name);

        //Clear array ready for use splitting another line
        Array.Clear(words, 0, words.Length);

       
        //spliting the second line
        words = fileLines[1].Split(' ');
        //parsing the second word of the line, which should be the number of intervalframes
        float.TryParse(words[1], out steps);
      //  Debug.Log(steps);

        Array.Clear(words, 0, words.Length);
        numberOfLines = fileLines.Count;
       // Debug.Log(numberOfLines);
        lineNumber = 3;
        stepsComplete = 0;

        stepMovement = FindNextMovement(lineNumber);
        stepRotation = FindNextRotation(lineNumber);
       // Debug.Log(stepMovement);
	}
	
	// Update is called once per frame
	void Update () {

        //if stepscomplete <= steps:
        //move one stepmovement
        //stepscomplete++

        if(stepsComplete <= steps)
        {
           
          //  Debug.Log(stepMovement);
            transform.Translate(stepMovement);
           // transform.Rotate(stepRotation);
            stepsComplete++;
        }
        else
        {
            if(lineNumber < numberOfLines)
            {
                
                lineNumber++;
                if(lineNumber != numberOfLines)
                {
                    stepMovement = FindNextMovement(lineNumber);
                    stepRotation = FindNextRotation(lineNumber);
                    stepsComplete = 0;
                }
                else
                {
                    Debug.Log("Run Finished");
                }
                
            }
            

        }

        //else
        //If lineNumber is less than number of lines, Increase linenumber by one
        //else stop
        //stepmovement = find next movement(linenumber)
		
	}

    Vector3 FindNextMovement(int line)
    {

       // Debug.Log(fileLines[line]);
        float xPos = 0;
        float yPos = 0;
        float zPos = 0;
        char[] delimiters = new char[] { ' ', '(', ')',  ',' };
        words = fileLines[line].Split(delimiters);

        
        //find total movement for each axis from file
        float.TryParse(words[12], out xPos);
        float.TryParse(words[14], out yPos);
        float.TryParse(words[16], out zPos);
       
        xPos /= steps;
        yPos /= steps;
        zPos /= steps;


        //Debug.Log(xPos);
        
        Vector3 finalVector = new Vector3(xPos,yPos,zPos);
        Array.Clear(words, 0, words.Length);
        return finalVector;

    }

    Vector3 FindNextRotation(int line)
    {
        
        float xRot = 0;
        float yRot = 0;
        float zRot = 0;
       // float scalar = 0;

        
        char[] delimiters = new char[] { ' ', '(', ')', ',' };
        words = fileLines[line].Split(delimiters);
        float.TryParse(words[31], out xRot);
        float.TryParse(words[33], out yRot);
        float.TryParse(words[35], out zRot);
       // float.TryParse(words[38], out scalar);
        
        // Debug.Log(words[38]);
        //xRot /= steps;
        //yRot /= steps;
        //zRot /= steps;




        // scalar /= steps;

        Vector3 finalVector = new Vector3(xRot, yRot, zRot);
        Array.Clear(words, 0, words.Length);
        
        
        return finalVector; 
    }
}
