using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotCamera : MonoBehaviour {

    /*--------------------------Snapshot Camera----------------------------------
     * This Script is attached to the Snapshot Camera prefab, which is placed in 
     * the scene and when requested, takes a screenshot of its current field of 
     * view. The default way to trigger it is by pressing the 'Generate Heatmap'
     * button on the DatabaseInterface Canvas.--------------------------------*/ 


          

    
    public int resWidth = 2550;
    public int resHeight = 3300;
    public GameObject[] invisibleObjects;


    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    

    
	// Use this for initialization
	void Start () {

      
		
	}
	
	// Update is called once per frame
	void Update () {

		
	}

    public void TakeScreenshot()
    {
        foreach (GameObject invisObject in invisibleObjects)
        {
            invisObject.gameObject.SetActive(false);
        }


        //https://answers.unity.com/questions/22954/how-to-save-a-picture-take-screenshot-from-a-camer.html
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        this.gameObject.GetComponent<Camera>().targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        this.gameObject.GetComponent<Camera>().Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        this.gameObject.GetComponent<Camera>().targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight);
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));

        foreach (GameObject invisObject in invisibleObjects)
        {
            invisObject.gameObject.SetActive(true);
        }
    }

    
}
