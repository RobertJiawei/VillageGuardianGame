using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManagement : MonoBehaviour
{
    public Texture2D[] CursorImage;
    RaycastHit HitInfo;
    private int CurrentIndex = 0;
    public GameObject Player;
    
    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Player.transform.position, Player.transform.forward, out HitInfo, 100f))
        {
            Debug.DrawRay(Player.transform.position, Player.transform.forward * 100f, Color.red);
        }
    }


    //Display a Cursor image at the center of the screen
    private void OnGUI()
    {
        Rect rect = new Rect(0, 0, CursorImage[CurrentIndex].width * 0.15f, CursorImage[CurrentIndex].height * 0.15f);
        rect.center = new Vector2(Screen.width / 2, Screen.height / 2);
        GUI.DrawTexture(rect, CursorImage[CurrentIndex]);
    }
}
