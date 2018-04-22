using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;


public class DatabaseInterface : MonoBehaviour {

    public SnapshotCamera cam;
    public GameObject heatmapPoints;
    public Dropdown numberOfRuns;
    public Dropdown runNumberPrefab;
    public Canvas interfaceCanvas;
    public List<Dropdown> dropdowns;
    public List<string> runIDs;
    SqliteConnection m_dbConnectionRunData;
    SqliteConnection m_dbConnectionRunID;
    string sql;
    int noOfRunsInt;
    SqliteCommand command;

    // Use this for initialization
    void Start () {

        dropdowns = new List<Dropdown>();
        
        int numberOfRunsIter = 1;
        List<string> noOfRunsOptions = new List<string>();

        m_dbConnectionRunData = new SqliteConnection("Data Source=RunData.sqlite;Version=3");
        m_dbConnectionRunID = new SqliteConnection("Data Source=RunID.sqlite;Version=3");

        m_dbConnectionRunData.Open();
        m_dbConnectionRunID.Open();

        //Fill the list
        sql = "select * from RunID order by RunID asc ";
        command = new SqliteCommand(sql, m_dbConnectionRunID);
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log("RunID " + reader["RunID"]);
            
            runIDs.Add(reader["RunID"].ToString());

            noOfRunsOptions.Add(numberOfRunsIter.ToString());
            numberOfRunsIter++;
            numberOfRuns.ClearOptions();
            numberOfRuns.AddOptions(noOfRunsOptions);

        }

        CreateDropdowns();
        

        

    }
	
	// Update is called once per frame
	void Update () {

        
		
	}


    public void GenerateHeatMap()
    {

        List<Vector3> transforms = new List<Vector3>();
        for (int i = 0; i < dropdowns.Count; i++)
        {
            //Look at value of Dropdown
           // Debug.Log(dropdown.value + 1);
            Debug.Log(i);

            int runIDInUse = dropdowns[i].value + 1;

            //Using SQLite, try to find all entries with the same Run ID as the Dropdown number in the RunData database

            sql = "select * from RunData where RunID =" + runIDInUse + " order by Frame asc ";
            command = new SqliteCommand(sql, m_dbConnectionRunData);
            SqliteDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                // Debug.Log("RunID " + reader["RunID"]);
                //Debug.Log("Frame: " + reader["Frame"]);
                //Debug.Log("Transform: " + reader["Transform"]);


                //Convert the transform into a string, then back into a vector
                string sVector = reader["Transform"].ToString();
                if (sVector.StartsWith("(") && sVector.EndsWith(")"))
                {
                    sVector = sVector.Substring(1, sVector.Length - 2);
                }

                string[] vSplit = sVector.Split(',');

                Vector3 finalTransform = new Vector3(
                    float.Parse(vSplit[0]),
                    float.Parse(vSplit[1]),
                    float.Parse(vSplit[2]));

                transforms.Add(finalTransform);

            }
        }


        List<GameObject> cubes = new List<GameObject>();
        //Take the transform for each entry and spawn a light/red cube at that location
        foreach (Vector3 v3 in transforms)
        {

            bool makeCube = true;
            for(int i = 0; i < cubes.Count; i++)
            {
                if(v3 == cubes[i].transform.position)
                {
                    //increase colour intensity of cube i
                    makeCube = false;
                    Color currentColor = cubes[i].GetComponent<Renderer>().material.color;
                    cubes[i].GetComponent<Renderer>().material.color = currentColor + new Color(1, 0, 0);
                }
                
            }

            if(makeCube)
            {
                GameObject instance = Instantiate(heatmapPoints, v3, Quaternion.identity);
                cubes.Add(instance);
            }
            
            Debug.Log(v3);
        }


        //Using the snapshot camera, take a screenshot of the current scene
        cam.TakeScreenshot();

        foreach(GameObject cube in cubes)
        {
            Destroy(cube.gameObject);
        }


    }

    void OnApplicationQuit()
    {
        m_dbConnectionRunData.Close();
        m_dbConnectionRunID.Close();
    }


   public void CreateDropdowns()
    {

        noOfRunsInt = numberOfRuns.value + 1;
        Debug.Log(noOfRunsInt);

        int posX = 90;
        int posY = 275;

        foreach (Dropdown dropdownObj in dropdowns)
        {
            Destroy(dropdownObj.gameObject);
        }
        dropdowns.Clear();


        for(int i = 0; i < noOfRunsInt; i++)
        {
            Dropdown instance = Instantiate(runNumberPrefab);
            
            instance.transform.position = new Vector3(posX, posY, 0);
            instance.transform.parent = interfaceCanvas.transform;
            dropdowns.Add(instance);
            Debug.Log(dropdowns[i]);
            posY -= 50;
            dropdowns[i].ClearOptions();
            dropdowns[i].AddOptions(runIDs);
            //dropdowns.Add()

        }
    }

    
}
