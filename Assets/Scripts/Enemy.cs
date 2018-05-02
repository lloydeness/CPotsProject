using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    // how many Colomns there will be
    public int BoardWidthSections;

    private SpriteRenderer EnemyRenderer;
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
    Vector2 Postion = new Vector2(0, 0);
    //percent of the screen we want to use 1 == no border
    public float borderPercent;
    //used to grab the required materials
    public BoardMaterials theMaterial;
    //the script used to runt he game reference
    public PlayGame playgame;
   

    //Character Movement
    public float maxSpeed;
    public Rigidbody objectToMove;
    private Vector3 steering = Vector3.zero;
    public float turnSpeed;
    public bool CompletedMovement = true;
    public Vector3 currentMoveTarget;
    public Vector2 CurrentGridSpace;
    public bool MovingRight = true;    
    public bool EnteredDaughtersRoom = false;
    public bool OriginalMovement = true;
    public bool beenRemoved = false;




    //Enemy Info
    public int EnemyMovementType;
    private Vector2 position;
    private float turningRadiusSlow = 0;


    void Awake()
    {
        GameObject temp;
        temp = GameObject.Find("ScriptsToAttach");
        if (temp != null)
        {
            playgame = temp.GetComponent<PlayGame>();
        }
        SetGamePiecesSections();
        setBorderPercent();

    }
    

    // Use this for initialization
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPosition(Vector2 Pos)
    {
        Postion = Pos;
    }

    public void SetUpAnEnemy(EnemyType inSprite)
    {

        //set default material
        EnemyRenderer = this.GetComponent<SpriteRenderer>();
        GameObject tempRenderer;

        EnemyMovementType = (int)inSprite;

        tempRenderer = GameObject.Find("ScriptsToAttach");

        if (tempRenderer != null)
        {

            theMaterial = tempRenderer.GetComponent<BoardMaterials>();
            EnemyRenderer.sprite = theMaterial.getEnemyMaterials(((int)inSprite));
        }

        


        //temp this is just to pick one of the cameras that will be in the scene
        GameObject temp;
        GameObject temp2;

        temp = GameObject.Find("Main Camera");
        temp2 = GameObject.Find("Camera");
        //the Camera will be different depending on where you are running the game from (debug code to be removed later)
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

            SetEnemySize();       

            this.transform.position = new Vector3(ScreenEdgeLeft + ((screenWidth / BoardWidthSections) * Postion.x), ScreenEdgeBot + ((screenWidth / BoardWidthSections) * Postion.y), 0);
            this.transform.localScale = new Vector3(LocalScaleX, LocalScaleX, 0);

        }
    }

    //We get the size this so we know how big to make the enemy 
    public void SetGamePiecesSections()
    {
        BoardWidthSections = (int)playgame.BoardSize.x;
    }

    public void setBorderPercent()
    {
        borderPercent = playgame.ScreenPercentToUse;
    } 

    private void SetEnemySize()
    {
        pWidth = EnemyRenderer.sprite.bounds.size.x;
        pHeight = EnemyRenderer.sprite.bounds.size.y;
        screenHeight = (cam.orthographicSize * 2.0f) * borderPercent;
        screenWidth = (screenHeight / Screen.height * Screen.width) * borderPercent;
        ScreenEdgeLeft = -screenWidth / 2;
        ScreenEdgeBot = -screenHeight / 2;
        LocalScaleX = (screenWidth / pWidth) / BoardWidthSections;
        LocalScaleY = (screenHeight / pHeight) / BoardWidthSections;
    }

    public void MoveTo(Vector3 TargetLocation, float slowingRadius, Vector2 gridPosition)
    {

        currentMoveTarget = TargetLocation;
        turningRadiusSlow = slowingRadius;
        CompletedMovement = false;
        SetCurrentEnemyPosition(gridPosition);
        maxSpeed = (Vector3.Distance(currentMoveTarget, objectToMove.transform.position) / 50) * 50;

    }


    public void ApplySteering()
    {
        


        Vector3 desiredVelocity = currentMoveTarget - transform.position;
        float distance = desiredVelocity.magnitude;

        // Check the distance to detect whether the character  is inside the slowing area
        if (distance < turningRadiusSlow)
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed * (distance / (turningRadiusSlow * 1000));
        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * maxSpeed;
        }
        steering += desiredVelocity - objectToMove.velocity;


        if (CompletedMovement == true)
        {
            Reset();
        }
        else if (CompletedMovement == false)
        {

            if (Vector3.Distance(currentMoveTarget, objectToMove.transform.position) <= 0.03)
            {

                CompletedMovement = true;
                Reset();
            }
            else
            {

                if (steering.magnitude > turnSpeed)
                {
                    steering = steering.normalized * turnSpeed;
                }

                objectToMove.GetComponent<Rigidbody>().velocity = objectToMove.GetComponent<Rigidbody>().velocity + steering;

                if (objectToMove.GetComponent<Rigidbody>().velocity.magnitude > maxSpeed)
                {
                    objectToMove.GetComponent<Rigidbody>().velocity = (objectToMove.GetComponent<Rigidbody>().velocity).normalized * maxSpeed;
                }
                //Right Now I do not need the rotation, 
                //objectToMove.transform.rotation = Quaternion.LookRotation(objectToMove.velocity);
            }
        }

    }

    // Reset the internal steering force.
    public void Reset()
    {
        steering = Vector3.zero;
        objectToMove.velocity = steering;
    }

    public void SetCurrentEnemyPosition(Vector2 currentPosition)
    {
        CurrentGridSpace = currentPosition;
    } 

    public int getEnemyMovementType()
    {

        return EnemyMovementType;
    }

    public Vector2 getCurrentGridPosition()
    {

        return CurrentGridSpace;
    }
    
    public void setMovingRight(bool isMovingRight)
    {
        MovingRight = isMovingRight;
    }

    public bool getMovingRight()
    {

        return MovingRight;
    }


}
