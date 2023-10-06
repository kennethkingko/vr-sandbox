using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class TimeSession : MonoBehaviour
{
    System.Diagnostics.Stopwatch stopwatch;
    bool isStarted;
    string fileName;
    // Start is called before the first frame update
    void Start()
    {
        stopwatch = new System.Diagnostics.Stopwatch();
        isStarted = false;
        fileName = SceneManager.GetActiveScene().name + "-timer.txt";
        Debug.Log("file: " + fileName);
    }
    public void StartWatch()
    {
        if(!isStarted)
        {
            stopwatch.Start();
            isStarted = true;
        }
    }
    public void StopWatch()
    {
        stopwatch.Stop();
        Debug.Log ("Time taken: "+(stopwatch.Elapsed));

        using(TextWriter writer = new StreamWriter(fileName, true))
        {
            writer.WriteLine(DateTime.Now.ToString() + " " + stopwatch.Elapsed + "\n");
            writer.Close();
        }

        isStarted = false;
        stopwatch.Reset();
    }
}
