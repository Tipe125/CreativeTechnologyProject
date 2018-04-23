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
    Dropdown dropdownInstance;
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
           
            
            runIDs.Add(reader["RunID"].ToString());

            noOfRunsOptions.Add(numberOfRunsIter.ToString());
            numberOfRunsIter++;
            numberOfRuns.ClearOptions();
            numberOfRuns.AddOptions(noOfRunsOptions);
            

        }

        List<string> otherQueries = new List<string>();
        otherQueries.Add("All Runs from Date...");
        otherQueries.Add("All Runs with Event...");
        numberOfRuns.AddOptions(otherQueries);

        CreateDropdowns();
        

        

    }
	
	// Update is called once per frame
	void Update () {

        
		
	}


    public void GenerateHeatMap()
    {

        List<Vector3> transforms = new List<Vector3>();
        List<GameObject> cubes = new List<GameObject>();
        bool makeCubeGreen = false;

        if (numberOfRuns.options[numberOfRuns.value].text == "All Runs from Date...")
        {
            
            //Find all entries with the chosen date in RUNID table
            List<string> applicableRuns = new List<string>();

            sql = "select * from RunID order by RunID asc";
            command = new SqliteCommand(sql, m_dbConnectionRunID);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
               
                if (reader["Date"].ToString() == dropdownInstance.options[dropdownInstance.value].text)
                {
                    
                    //Put those RUNIDs into a list of strings
                    applicableRuns.Add(reader["RunID"].ToString());  
                }
            }

            //Then for each RUNID in the list, go through each entry in the RunData table and find that run,
            foreach (string run in applicableRuns)
            {
                string runIDInUse = run;

                sql = "select * from RunData where RunID =" + runIDInUse + " order by Frame asc ";
                command = new SqliteCommand(sql, m_dbConnectionRunData);
                reader = command.ExecuteReader();
                //then make a cube for each entry in that run
                while (reader.Read())
                {
                    if (reader["CustomEvent"].ToString() != "N/A")
                    {
                        makeCubeGreen = true;
                    }

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

                    //If makecubegreen is true, then spawn the cube and add it to cubes?
                    if (makeCubeGreen)
                    {
                        GameObject instance = Instantiate(heatmapPoints, finalTransform, Quaternion.identity);

                        instance.GetComponent<Renderer>().material.color = new Color(0, 50, 0);
                        cubes.Add(instance);
                        makeCubeGreen = false;
                    }
                    else
                    {
                        //else just add it as normal?
                        transforms.Add(finalTransform);
                    }

                }


            }

        }
        else if (numberOfRuns.options[numberOfRuns.value].text == "All Runs with Event...")
        {
            //Find all entries with the chosen event in the RunData
            //Get the RunID's of those entries
            //Put them into a string list, then do the same as with Dates
            //Add some logic to make the cube green if it was a custom event
            List<string> applicableRuns = new List<string>();

            sql = "select * from RunData order by RunID asc";
            command = new SqliteCommand(sql, m_dbConnectionRunData);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                if (reader["CustomEvent"].ToString() == dropdownInstance.options[dropdownInstance.value].text)
                {

                    //Put those RUNIDs into a list of strings
                    applicableRuns.Add(reader["RunID"].ToString());
                }
            }

            foreach (string run in applicableRuns)
            {
                string runIDInUse = run;

                sql = "select * from RunData where RunID =" + runIDInUse + " order by Frame asc ";
                command = new SqliteCommand(sql, m_dbConnectionRunData);
                reader = command.ExecuteReader();
                //then make a cube for each entry in that run
                while (reader.Read())
                {
                    if (reader["CustomEvent"].ToString() != "N/A")
                    {
                        makeCubeGreen = true;
                    }

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

                    //If makecubegreen is true, then spawn the cube and add it to cubes?
                    if (makeCubeGreen)
                    {
                        GameObject instance = Instantiate(heatmapPoints, finalTransform, Quaternion.identity);

                        instance.GetComponent<Renderer>().material.color = new Color(0, 50, 0);
                        cubes.Add(instance);
                        makeCubeGreen = false;
                    }
                    else
                    {
                        //else just add it as normal?
                        transforms.Add(finalTransform);
                    }


                }


            }



        }
        else
        {
            for (int i = 0; i < dropdowns.Count; i++)
            {
                //Look at value of Dropdown
                // Debug.Log(dropdown.value + 1);
                

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
                    if(reader["CustomEvent"].ToString() != "N/A")
                    {
                        makeCubeGreen = true;
                    }

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

                    //If makecubegreen is true, then spawn the cube and add it to cubes?
                    if(makeCubeGreen)
                    {
                        GameObject instance = Instantiate(heatmapPoints, finalTransform, Quaternion.identity);
                        
                        instance.GetComponent<Renderer>().material.color = new Color(0, 50, 0);
                        cubes.Add(instance);
                        makeCubeGreen = false;
                    }
                    else
                    {
                        //else just add it as normal?
                        transforms.Add(finalTransform);
                    }

                    

                    

                }
            }
        }

        
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
        foreach (Dropdown dropdownObj in dropdowns)
        {
            Destroy(dropdownObj.gameObject);
        }
        dropdowns.Clear();

        
        if(numberOfRuns.options[numberOfRuns.value].text == "All Runs from Date...")
        {
            
            dropdownInstance = Instantiate(runNumberPrefab);
            dropdownInstance.GetComponentInChildren<Text>().text = "Date";
            dropdownInstance.transform.position = new Vector3(90, 275, 0);
            dropdownInstance.transform.parent = interfaceCanvas.transform;
            dropdownInstance.ClearOptions();
            dropdowns.Add(dropdownInstance);

            List<string> options = new List<string>();

            sql = "select * from RunID order by RunID asc";
            command = new SqliteCommand(sql, m_dbConnectionRunID);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                bool isNotInOptions = true;

                foreach (string date in options)
                {
                    if(reader["Date"].ToString() == date)
                    {
                        isNotInOptions = false;
                    }
                }

                if(isNotInOptions)
                {
                    options.Add(reader["Date"].ToString());
                }
            }

            dropdownInstance.AddOptions(options);

         
        }
        else if(numberOfRuns.options[numberOfRuns.value].text == "All Runs with Event...")
        {
            
            dropdownInstance = Instantiate(runNumberPrefab);
            dropdownInstance.transform.position = new Vector3(90, 275, 0);
            dropdownInstance.transform.parent = interfaceCanvas.transform;
            dropdownInstance.ClearOptions();
            dropdowns.Add(dropdownInstance);

            List<string> options = new List<string>();

            sql = "select * from RunData order by RunID asc";
            command = new SqliteCommand(sql, m_dbConnectionRunData);
            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if(reader["CustomEvent"].ToString() != "N/A")
                {
                    bool isNotInOptions = true;
                   foreach(string events in options)
                    {
                        if(reader["CustomEvent"].ToString() == events)
                        {
                            isNotInOptions = false;
                        }
                    }

                   if(isNotInOptions)
                    {
                        options.Add(reader["CustomEvent"].ToString());
                    }
                }
            }

            dropdownInstance.AddOptions(options);


            //Look for content in custom events column
            //If any, put whatever the value is as an option on the dropdown
            //Then when the user selects that event, go through all runs and find any runs with that event, then use them in the heatmap
        }
        else
        {
            noOfRunsInt = numberOfRuns.value + 1;
            

            int posX = 90;
            int posY = 275;

            for (int i = 0; i < noOfRunsInt; i++)
            {
                Dropdown instance = Instantiate(runNumberPrefab);

                instance.transform.position = new Vector3(posX, posY, 0);
                instance.transform.parent = interfaceCanvas.transform;
                dropdowns.Add(instance);
                
                posY -= 50;
                dropdowns[i].ClearOptions();
                dropdowns[i].AddOptions(runIDs);
                if(i >= 6)
                {
                    posY = 275;
                    posX += 90;
                }
                //dropdowns.Add()

            }
        }
        
    }

    
}
