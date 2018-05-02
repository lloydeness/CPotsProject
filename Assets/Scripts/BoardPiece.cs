using UnityEngine;
using System.Collections;

public class BoardPiece : MonoBehaviour {
    // how many Colomns there will be
    public int GamePieceSections;  

    private SpriteRenderer PieceRenderer;
    //scale in vector
    public Vector2 TheTileSize;
    //The Scale set to the sprite
    private float LocalScaleX;
    private float LocalScaleY;
    // THe chords of bottom left of the screen
    private float ScreenEdgeLeft;
    private float ScreenEdgeBot;
    // Game Pieces width/Height Prior to adjustment
    private float pWidth;
    private float pHeight;
    // These are the current resolution of the screen
    private float screenWidth;
    private float screenHeight;   
    // The camera used
    private Camera cam;
    //The position that should be used for the sprite board piece
    Vector2 Postion = new Vector2(0,0);
    //percent of the screen we want to use 1 == no border
    public float borderPercent;
    //used to grab the required materials
    public BoardMaterials theMaterial;

    // Use this for initialization
    void Start () {
        //SetUpBoardPiece(BM_Sprite.BM_Normal);
    }




    // Update is called once per frame
    void Update () {
 


    }

    public void SetPosition(Vector2 Pos)
    {


        Postion = Pos;
    }

    public void SetUpBoardPiece(BM_Sprite inSprite)
    {
       
        //set default material
        PieceRenderer = this.GetComponent<SpriteRenderer>();
        GameObject tempRenderer;

        

       tempRenderer = GameObject.Find("ScriptsToAttach");
      
       if (tempRenderer != null)
       {

           theMaterial = tempRenderer.GetComponent<BoardMaterials>();
       }
       


       PieceRenderer.sprite = theMaterial.getMaterial(((int)inSprite));
        

        //temp this is just to pick one of the probably cameras that will be in the scene
        GameObject temp;
        GameObject temp2;

        temp = GameObject.Find("Main Camera");
        temp2 = GameObject.Find("Camera");
        //
        if (temp != null)
        {
            cam = temp.GetComponentInParent<Camera>();
        }
        else if (temp2 != null)
        {
            cam = temp2.GetComponentInParent<Camera>();
        }

        if (temp2 != null || temp != null)
        {

            SetPieceSizes();

            //this.transform.position = new Vector3(ScreenEdgeLeft, ScreenEdgeBot, 0);

            this.transform.position = new Vector3(ScreenEdgeLeft + ((screenWidth / GamePieceSections) * Postion.x ), ScreenEdgeBot + ((screenWidth / GamePieceSections) * Postion.y), 0);
            this.transform.localScale = new Vector3(LocalScaleX, LocalScaleX, 0);
            TheTileSize = new Vector2(LocalScaleX, LocalScaleX);
            
        }
    }

    public void SetGamePiecesSections(int Sections)
    {
        GamePieceSections = Sections;

    }

    public void setBorderPercent(float border)
    {
        borderPercent = border;

    }

    private void SetPieceSizes()
    {
        pWidth = PieceRenderer.sprite.bounds.size.x;
        pHeight = PieceRenderer.sprite.bounds.size.y;
        
        screenHeight = (cam.orthographicSize * 2.0f) * borderPercent;
        screenWidth = (screenHeight / Screen.height * Screen.width) * borderPercent;

        ScreenEdgeLeft = -screenWidth / 2;
        ScreenEdgeBot = -screenHeight / 2;

        LocalScaleX = (screenWidth / pWidth) / GamePieceSections;
        LocalScaleY = (screenHeight / pHeight) / GamePieceSections;
    }

    public Vector2 getTheTileSize()
    {

        return TheTileSize;
    }

 



}












//Original Code for referance
/*
  PieceRenderer = this.GetComponent<SpriteRenderer>();
       

        GameObject temp;
        GameObject temp2;

        temp = GameObject.Find("Main Camera");
        temp2 = GameObject.Find("Camera");
        //
        if (temp != null)
        {
            cam = temp.GetComponentInParent<Camera>();
        }
        else if (temp2 != null)
        {
            cam = temp2.GetComponentInParent<Camera>();
        }

        if (temp2 != null || temp != null)
        {



            pWidth = PieceRenderer.sprite.bounds.size.x;
            pHeight = PieceRenderer.sprite.bounds.size.y;

            screenHeight = cam.orthographicSize * 2.0f;
            screenWidth = screenHeight / Screen.height * Screen.width;

            ScreenEdgeLeft = -screenWidth / 2;
            ScreenEdgeBot = -screenHeight / 2;          

            LocalScaleX = (screenWidth / pWidth) / GamePieceSections;
            LocalScaleY = (screenHeight / pHeight) / GamePieceSections;

            //This is how far from the left it is over
            int templocation = 0;
            int tempColumn = 1;

            this.transform.position = new Vector3(ScreenEdgeLeft + ((screenWidth / GamePieceSections) * templocation),ScreenEdgeBot + ((screenWidth / GamePieceSections) * tempColumn), 0);
            this.transform.localScale = new Vector3(LocalScaleX, LocalScaleX, 0);

        

        }


     
  
 
 */
