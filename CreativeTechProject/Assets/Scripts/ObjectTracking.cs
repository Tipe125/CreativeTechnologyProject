using System.Collections;
using System.Collections.Generic;

using System.IO;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class ObjectTracking : MonoBehaviour {


    public GameObject trackedObject;
    public int intervalFrames;
    private int framesPassed;
    private int currentFrame;

    private Vector3 currentPos;
    private Vector3 previousPos;
    private Quaternion currentRot;
    private Quaternion previousRot;
   
    private Vector3 movement;
    private Quaternion rotationChange;
    private bool firstWrite;
    private int previousRunID;
    private int currentRunID;
    SqliteConnection m_dbConnectionRunData;
    SqliteConnection m_dbConnectionRunID;
    string sql;
    SqliteCommand command;
    string filename;
    
    

    public static string FileName()
    {
        return string.Format("{0}/tracking/RunData_{1}.txt",
                             Application.dataPath,
                             
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    //file path info 



    // Use this for initialization
    void Start () {
        currentFrame = 0;
        framesPassed = intervalFrames;
        firstWrite = true;
        filename = FileName();
        //ADD CONNECTION AND CREATE TABLES ONLY IF THEY DON'T EXIST
        //IF THEY DON'T, CREATE A TABLE FOR RUN ID AND A TABLE CONTAINING THE ACTUAL DATA

        if(!File.Exists("RunData.sqlite"))
        {
            SqliteConnection.CreateFile("RunData.sqlite");
        }

        if (!File.Exists("RunID.sqlite"))
        {
            SqliteConnection.CreateFile("RunID.sqlite");
        }
        
        //This creates a connection to the file being used for the database
        m_dbConnectionRunData = new SqliteConnection("Data Source=RunData.sqlite;Version=3");
        m_dbConnectionRunID = new SqliteConnection("Data Source=RunID.sqlite;Version=3");



        m_dbConnectionRunData.Open();
        m_dbConnectionRunID.Open();

        if (m_dbConnectionRunData == null)
        {
            Debug.Log("NO database found");
        }
        else
        {
            Debug.Log("Database Found");
            Debug.Log(m_dbConnectionRunData.Database);
        }

        if (m_dbConnectionRunID == null)
        {
            Debug.Log("NO database found");
        }
        else
        {
            Debug.Log("Database Found");
            Debug.Log(m_dbConnectionRunID.Database);
        }

        sql = "create table if not exists RunID(RunID int, DateTime varchar(20))";
        command = new SqliteCommand(sql, m_dbConnectionRunID);
        command.ExecuteNonQuery();

        sql = "create table if not exists RunData(RunID int, Frame int, Transform varchar(20), Movement varchar(20), Rotation varchar(20))";
        command = new SqliteCommand(sql, m_dbConnectionRunData);
        command.ExecuteNonQuery();

        sql = "select * from RunID WHERE RunID=(select max(RunID) from RunID)";
        command = new SqliteCommand(sql, m_dbConnectionRunID);
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            previousRunID = (int)reader["RunID"] ;
            Debug.Log("RunID " + reader["RunID"]);
            Debug.Log("Date/Time: " + reader["DateTime"]);

        }

        currentRunID = previousRunID + 1;
        //INSERT RUNID INTO RUNID TABLE HERE, FIGURE OUT HOW TO INCREMENT BASED ON MISSING DATA
        sql = "insert into RunID(RunID, DateTime) values (" + currentRunID + ", '" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "')";
        command = new SqliteCommand(sql, m_dbConnectionRunID);
        command.ExecuteNonQuery();
        
        sql = "select * from RunID order by RunID desc";
        command = new SqliteCommand(sql, m_dbConnectionRunID);
        reader = command.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log("RunID " + reader["RunID"]);
            Debug.Log("Date/Time: " + reader["DateTime"]);

        }


    }
	
	// Update is called once per frame
	void Update () {
        
        if (framesPassed == intervalFrames)
        {
            //storing info on current and previous positions and calculating the movement
            previousPos = currentPos;
            currentPos = trackedObject.transform.position;
            movement = currentPos - previousPos;

            //storing info on current and previous rotations and calculating rotation change
            previousRot = currentRot;
            currentRot = trackedObject.transform.rotation;
            rotationChange = currentRot * Quaternion.Inverse(previousRot);

            WriteString();
            framesPassed = 0;
        }
        else
        {

            framesPassed++;
        }

        currentFrame++;

    }

    void WriteString()
    {
        //Writeline including CurrentTransform, movement, rotation and framenumber
        //Save that to file 
        //ALSO WRITE TO DATABASE

        string Line = "Frame: " + currentFrame + " Position: " + currentPos + " Movement: " + movement + " Rotation: " + currentRot + " Rotation Change: " + rotationChange + "\r\n";
        if (firstWrite)
        {
            
            File.WriteAllText(filename, "Object Tracked: " + trackedObject.name + "\r\n" + "Interval: " + intervalFrames + "\r\n" + Line);
            firstWrite = false;
        }
        else
        {
            File.AppendAllText(filename, Line);
            
        }

        //WRITE LINE TO DATABASE INCLUDING RUNID 
        sql = "insert into RunData(RunID, Frame, Transform, Movement, Rotation) values (" + currentRunID + ",'" + currentFrame + "','" + currentPos + "','" + movement + "','" + currentRot + "' )";
        command = new SqliteCommand(sql, m_dbConnectionRunData);
        command.ExecuteNonQuery();

        /*sql = "select * from RunData order by Frame desc ";
        command = new SqliteCommand(sql, m_dbConnectionRunData);
        SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log("RunID " + reader["RunID"]);
            Debug.Log("Frame: " + reader["Frame"]);
            Debug.Log("Transform: " + reader["Transform"]);
            Debug.Log("Movement: " + reader["Movement"]);
            Debug.Log("Rotation: " + reader["Rotation"]);
            
        }*/

    }

    void OnApplicationQuit()
    {
        m_dbConnectionRunData.Close();
        m_dbConnectionRunID.Close();
    }
}
