using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResolutionModifier : MonoBehaviour
{
    public Camera cam;
    public RawImage alfx0;
    private int targetWidth = Screen.width;
    private int targetHeight = Screen.height;
    
    private void Start() {
        ChangeResolution(targetWidth, targetHeight, FilterMode.Bilinear);
    }

    void OnGUI()
    {


        //originx4
        if (GUI.Button(new Rect(1000, 40, 100, 30), "orignx4")){
            ChangeResolution(targetWidth*4, targetHeight*4, FilterMode.Bilinear);
        }
        //originx2
        if (GUI.Button(new Rect(1000, 80, 100, 30), "orignx2")){
            ChangeResolution(targetWidth*2, targetHeight*2, FilterMode.Bilinear);
        }

        //origin
        if (GUI.Button(new Rect(1000, 120, 100, 30), "orign")){
            ChangeResolution(targetWidth, targetHeight, FilterMode.Bilinear);
        }

        //0.5
        if (GUI.Button(new Rect(1000, 160, 100, 30), "orign/2")){
            ChangeResolution(targetWidth/2, targetHeight/2, FilterMode.Point);
        }

        //0.25
        if (GUI.Button(new Rect(1000, 200, 100, 30), "orign/4")){
            ChangeResolution(targetWidth/4, targetHeight/4, FilterMode.Point);
        }

        //0.125
        if (GUI.Button(new Rect(1000, 240, 100, 30), "orign/8")){
            ChangeResolution(targetWidth/8, targetHeight/8, FilterMode.Point);
        }

    }

    void ChangeResolution(int width, int height, FilterMode mode){
        if ( cam.targetTexture != null ) 
            {
                cam.targetTexture.Release( );
            }
 
        RenderTexture al = new RenderTexture(width, height, 24);
        mode = FilterMode.Bilinear;
        al.filterMode = mode;
        cam.targetTexture = al;
        alfx0.texture = al;
    }
    
}