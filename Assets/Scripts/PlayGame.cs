using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayGame : MonoBehaviour {
        
    public BoardPiece[,] boardPieces;    
    public int[,] BoardPlacements;   
    public int SpacesFromBottom;
    public float ScreenPercentToUse;
    public Vector2 BoardSize;
    public BoardPiece theBoardPiece;
    public int AmountOfDaughtersRooms;
    public int enemiesToSpawn;
    public int TotalEnemyTypes = 2;
    public bool GameIsOver = false;
    public Vector2 TheTileSize;


    //Enemy
    public List<Enemy> AllEnemies;
    public Enemy enemy;
    public int EnemyMovementStage = 0;
    public float TimeToWaitBeforeMoveingEnemies;
    private float enemyTimer;

    //Daughter Window stuff
    int DaughterDelete;
    public List<int> DaughterLocations;
    public List<Vector2> WindowLocations;

    //Player Information
    public ControlledCharacter player;


    // Use this for initialization
    void Start () {
        //initiate arrays
        boardPieces = new BoardPiece[(int)BoardSize.x, (int)BoardSize.y];
               
        int ArrayYSize = boardPieces.GetLength(1);
        //initialize all options to 99 
        BoardPlacements = new int[AmountOfDaughtersRooms, ArrayYSize];

        for (int i = 0; i < BoardPlacements.GetLength(0); i++)
        {
            for (int j = 0; j < BoardPlacements.GetLength(1); j++)
            {
                BoardPlacements[i, j] = 99;
            }
        }
        BuildSpecialBoardPieces();
        BuildBoard();
        addEnemy(enemiesToSpawn);
        TheTileSize = boardPieces[0, 0].getTheTileSize();
        player.SetSize(TheTileSize);

	}
	
	// Update is called once per frame
	void Update () {
        //Test stuff Below

        EnemyMovement();

    }


    void BuildBoard()
    {
        BoardPiece[] temp = new BoardPiece[(int)BoardSize.x];


        for (int i = 0; i < BoardSize.x; i++)
        {
            for (int j = 0; j < BoardSize.y; j++)
            {
                BoardPiece tempObject = (BoardPiece)Instantiate(theBoardPiece);
                if (tempObject != null)
                {
                    tempObject.setBorderPercent(ScreenPercentToUse);
                    tempObject.SetGamePiecesSections((int)BoardSize.x);
                    //BoardPiece material to use
                    BM_Sprite tempMaterial = BM_Sprite.BM_Normal;

                    //change material to the correct one
                    if (BoardPlacements[0, j] == i && j < BoardSize.y - 1)
                    {
                        tempMaterial = BM_Sprite.BM_Window;
                        WindowLocations.Add(new Vector2(i, j));
                    }
                    else if (j == BoardSize.y - 1)
                    {
                        temp[i] = tempObject;
                        
                        for (int x = 0; x < AmountOfDaughtersRooms; x++)
                        {

                            if (BoardPlacements[x, (int)BoardSize.y - 1] == i)
                            {
                                tempMaterial = BM_Sprite.BM_Daughter;
                                DaughterLocations.Add(i);
                            }
                        }
                    }

                    tempObject.SetPosition(new Vector2(i, j + SpacesFromBottom));
                    boardPieces[i, j] = tempObject;
                    tempObject.SetUpBoardPiece(tempMaterial);
                }
            }

            player.setMovementLocations(temp);
        }

        DaughterDelete = AmountOfDaughtersRooms; 


        List<Vector2> TempWindowLocations;
        TempWindowLocations = new List<Vector2>();
        //Sort the WindowLocations
        for (int i = 0; i < WindowLocations.Count; i++)
        {
            for (int j = 0; j < WindowLocations.Count; j++)
            {
                if (WindowLocations[j].y == i)
                {
                    TempWindowLocations.Add(WindowLocations[j]);
                }
            }

        }

        WindowLocations = TempWindowLocations;
        
    }


    void BuildSpecialBoardPieces()
    {
        int ArrayXSize = BoardPlacements.GetLength(0);
        int ArrayYSize = BoardPlacements.GetLength(1);

        for (int i = 0; i < ArrayYSize; i++)
        {            
            BoardPlacements[0, i] = Random.Range(0, (int)BoardSize.x);
            //check that this next one is not in the same space
            if (i != 0)
            {
                if (BoardPlacements[0,i] == BoardPlacements[0,i-1])
                {
                    do
                    {
                        BoardPlacements[0, i] = Random.Range(0, (int)BoardSize.x);

                    } while (BoardPlacements[0, i] == BoardPlacements[0, i - 1]);
                }

            }

            if (i == ArrayYSize - 1)
            {


                for (int j = 0; j < AmountOfDaughtersRooms; j++)
                {
                    BoardPlacements[j, i] = Random.Range(0, (int)BoardSize.x);                    
                    
                    if (j == 0)
                    {
                        BoardPlacements[j, i] = Random.Range(0, (int)BoardSize.x);
                    }
                    else
                    {
                        bool isUnigue = true;

                        //make sure the daughters are placed in different windows

                        do
                        {
                            

                            int tempRandom = Random.Range(0, (int)BoardSize.x);
                            isUnigue = true;

                            for (int k = 0; k < ArrayXSize; k++)
                            {
                                if (tempRandom == BoardPlacements[k, i])
                                {
                                    isUnigue = false;
                                    break;
                                }
                            }

                            if (isUnigue == true)
                            {
                                BoardPlacements[j, i] = tempRandom;
                            }

                        } while (isUnigue == false);
                    }
                }
            }
        }
    }


    public void addEnemy(int EnemiesToCreate)
    {
        
        //List of spawns to use
        List<int> SpawnPosition; 
        
        //This should never happen, but who knows, maybe so just in case
        if (EnemiesToCreate > BoardSize.x)
        {

            EnemiesToCreate = (int)BoardSize.x;
        }
        //Initialize spawnPosition
        SpawnPosition = new List<int>(EnemiesToCreate);

        //create Random spawn locations based on the amount of enemiesToCreate
        for (int j = 0; j < 2; j++)
        {
            int tempRandomNumber = 0;

            if (j == 0)
            {
                tempRandomNumber = Random.Range(0, (int)BoardSize.x);
                SpawnPosition.Add(tempRandomNumber);

            }
            else
            {
                for (int l = 1; l < EnemiesToCreate; l++)
                {
                    bool isUnique = true;
                    do
                    {
                        isUnique = true;
                        tempRandomNumber = Random.Range(0, (int)BoardSize.x);
                        for (int k = 0; k < SpawnPosition.Count; k++)
                        {

                            if (SpawnPosition[k] == tempRandomNumber)
                            {
                                isUnique = false;                                
                            }
                        }


                    } while (isUnique == false);
                    SpawnPosition.Add(tempRandomNumber);
                }
            }
        }        

        for (int i = 0; i < EnemiesToCreate; i++)
        {
            int tempEnemyType = Random.Range(0, TotalEnemyTypes);

        
            Enemy tempEnemy = (Enemy)Instantiate(enemy);

            EnemyType tempMaterial = (EnemyType)tempEnemyType;

            tempEnemy.SetPosition(new Vector2(SpawnPosition[i], 0 + SpacesFromBottom));
            tempEnemy.SetCurrentEnemyPosition(new Vector2(SpawnPosition[i], 0));
            tempEnemy.SetUpAnEnemy(tempMaterial);

            AllEnemies.Add(tempEnemy);

        }
    }

    //These are the enemy movement stages
    public void EnemyMovement()
    {
        /*EnemyStages
         *0- give Enemy a movelocation
         *1 - Move the Enemy
         *2 - Don't move the enemy until enemymovement phase
         */

        if (EnemyMovementStage == 0)
        {
            player.canShoot = false;
            setEnemyMovement();
        }
        else if (EnemyMovementStage == 1)
        {
            //this is temp
            RemovedKnockedOutEnemies();
            moveEnemies();
    
        } 

        //remove the pieces from the board that have entered a daughters room
        else if (EnemyMovementStage == 2)
        {
            
            ClearEnemyPieces();
            if (DaughterLocations.Count  == 0)
            {
                GameIsOver = true;
                ClearEnemyPieces();
            }
            EnemyMovementStage = 3;
            player.canShoot = true;

        }
        else if (EnemyMovementStage == 3)
        {
            enemyTimer -= Time.deltaTime;
         
            if (enemyTimer <= 0)
            {
               
                EnemyMovementStage = 0;

                if (GameIsOver == true)
                {
                    cleanTheBoard();
                }
            }

        }


    }

    public void setEnemyMovement()
    {
        /*go through each enemies in the array
         get current position
        */

         int[,] UsedBoardPlacement;
         UsedBoardPlacement = new int[(int)BoardSize.x, (int)BoardSize.y]; 

        //Initialize the board to tells us that no spaces are used by setting value to 0
        for (int i = 0; i < (int)BoardSize.x; i++ )
        {
            for (int j = 0; j < (int)BoardSize.y; j++)
            {
                UsedBoardPlacement[i, j] = 0;
            }
        }



        for (int i = 0; i < AllEnemies.Count; i++)
        {
            Vector2 temperaryPosition = AllEnemies[i].getCurrentGridPosition();
            //if the character is on the top level of the house, go to the Daughters window
            if (temperaryPosition.y == (int)BoardSize.y - 1)
            {

            //if there are any daughters rooms left, move to that room,
            if (DaughterLocations.Count > 0)
                {
                    //get the closest daughters room, getting the absolute value, if the person lands on the space, we 
                    // will give them a turn to try and rid of them
                
                    Vector2 MovePosition = new Vector2((int)DaughterLocations[0], (int)temperaryPosition.y);
                    //Vector2 PositionToGoTo = ReturnPostion(MovePosition, UsedBoardPlacement);

                    AllEnemies[i].MoveTo(boardPieces[(int)DaughterLocations[0], (int)temperaryPosition.y].transform.position, 0, MovePosition);
                    AllEnemies[i].EnteredDaughtersRoom = true;
                    DaughterLocations.RemoveAt(0);
                    
                    
                }
                // else,  the game has been lost
                else
                {
                    GameIsOver = true;
                    
                }


            }
            //move the character normally
            else
            {
                if (GameIsOver == false)
                {

                    //Enemytypes are 0: Straight, 1: Diagonal
                    if (AllEnemies[i].getEnemyMovementType() == 0)
                    {
                        Vector2 tempPosition = AllEnemies[i].getCurrentGridPosition();
                        //This is the final 
                        Vector2 MovePosition = new Vector2((int)tempPosition.x, (int)tempPosition.y + 1);
                        Vector2 PositionToGoTo = ReturnPostion(MovePosition, UsedBoardPlacement);

                        AllEnemies[i].MoveTo(boardPieces[(int)PositionToGoTo.x, (int)PositionToGoTo.y].transform.position, 0, PositionToGoTo);

                    }
                    else if (AllEnemies[i].getEnemyMovementType() == 1)
                    {
                        //Check if piece is on the end of the board
                        if (AllEnemies[i].getCurrentGridPosition().x == 0 || AllEnemies[i].getCurrentGridPosition().x == (BoardSize.x - 1))
                        {
                            if (AllEnemies[i].getCurrentGridPosition().x == 0)
                            {
                                AllEnemies[i].setMovingRight(true);

                            }
                            else if (AllEnemies[i].getCurrentGridPosition().x == BoardSize.x - 1)
                            {
                                AllEnemies[i].setMovingRight(false);

                            }

                        }
                        else
                        { }

                        if (AllEnemies[i].getMovingRight() == true)
                        {
                            Vector2 tempPosition = AllEnemies[i].getCurrentGridPosition();
                            Vector2 MovePosition = new Vector2((int)tempPosition.x + 1, (int)tempPosition.y + 1);
                            Vector2 PositionToGoTo = ReturnPostion(MovePosition, UsedBoardPlacement);


                            AllEnemies[i].MoveTo(boardPieces[(int)PositionToGoTo.x, (int)PositionToGoTo.y].transform.position, 0, PositionToGoTo);

                        }
                        else if (AllEnemies[i].getMovingRight() == false)
                        {
                            Vector2 tempPosition = AllEnemies[i].getCurrentGridPosition();
                            Vector2 MovePosition = new Vector2((int)tempPosition.x - 1, (int)tempPosition.y + 1);
                            Vector2 PositionToGoTo = ReturnPostion(MovePosition, UsedBoardPlacement);

                            AllEnemies[i].MoveTo(boardPieces[(int)PositionToGoTo.x, (int)PositionToGoTo.y].transform.position, 0, PositionToGoTo);

                        }
                    }


                    else if (AllEnemies[i].getEnemyMovementType() == 2)
                    {

                       

                        //Check if piece is on the end of the board
                        if (AllEnemies[i].getCurrentGridPosition().x == 0 || AllEnemies[i].getCurrentGridPosition().x == (BoardSize.x - 1))
                        {
                            if (AllEnemies[i].getCurrentGridPosition().x == 0)
                            {
                                AllEnemies[i].setMovingRight(true);
                                AllEnemies[i].OriginalMovement = false;
                            }
                            else if (AllEnemies[i].getCurrentGridPosition().x == BoardSize.x - 1)
                            {
                                AllEnemies[i].setMovingRight(false);
                                AllEnemies[i].OriginalMovement = true;

                            }

                        }
                        else
                        {
                            if (AllEnemies[i].OriginalMovement == true)
                            {
                                AllEnemies[i].setMovingRight(false);
                            }
                            else
                            {
                                AllEnemies[i].setMovingRight(true);
                            }
                        }


                        if (AllEnemies[i].getMovingRight() == true)
                        {
                            Vector2 tempPosition = AllEnemies[i].getCurrentGridPosition();
                            Vector2 MovePosition = new Vector2((int)tempPosition.x + 1, (int)tempPosition.y + 1);
                            Vector2 PositionToGoTo = ReturnPostion(MovePosition, UsedBoardPlacement);


                            AllEnemies[i].MoveTo(boardPieces[(int)PositionToGoTo.x, (int)PositionToGoTo.y].transform.position, 0, PositionToGoTo);

                        }
                        else if (AllEnemies[i].getMovingRight() == false)
                        {
                            Vector2 tempPosition = AllEnemies[i].getCurrentGridPosition();
                            Vector2 MovePosition = new Vector2((int)tempPosition.x - 1, (int)tempPosition.y + 1);
                            Vector2 PositionToGoTo = ReturnPostion(MovePosition, UsedBoardPlacement);

                            AllEnemies[i].MoveTo(boardPieces[(int)PositionToGoTo.x, (int)PositionToGoTo.y].transform.position, 0, PositionToGoTo);

                        }
                    }
                }

            }
        }


        EnemyMovementStage = 1;

    }

    public void moveEnemies()
    {

        bool allEnemiesDoneMoving = true;

        for (int i = 0; i < AllEnemies.Count; i++)
        {
            if (AllEnemies[i].CompletedMovement == false)
            {
                AllEnemies[i].ApplySteering();
                allEnemiesDoneMoving = false;
            }

        }
        if (allEnemiesDoneMoving == true)
            {
                EnemyMovementStage = 2;
                enemyTimer = TimeToWaitBeforeMoveingEnemies;
            //do not create enemies when the game is over
            if (GameIsOver == false)
            {

                addEnemy(enemiesToSpawn);
            }
        }
    }

    public Vector2 ReturnPostion(Vector2 MoveToPosition, int[,] theArray)
    {
        //check if the position is in use
        //if 0 then the space is not taken
        Vector2 ReturnValue = new Vector2(0,0);



        //The place it was going is fine
        if (theArray[(int)MoveToPosition.x,(int)MoveToPosition.y] == 0)
        {
            theArray[(int)MoveToPosition.x, (int)MoveToPosition.y] = 1;
            ReturnValue = MoveToPosition;
        }
        //Move the piece to a place that has nothing on it
        else
        {

            for (int i = (int)MoveToPosition.y; i < (int)BoardSize.y; i++)
            {
                //if theArray == 0, then the spot is good, and we can exit the loop
                if (theArray[(int)MoveToPosition.x,  i] == 0)
                {
                    ReturnValue = new Vector2((int)MoveToPosition.x, i);
                    break;
                }
            }
        }

        
        return ReturnValue;
    }

    public void RemovedKnockedOutEnemies()
    {
        for (int i = 0; i < AllEnemies.Count; i++)
        {
  
            if (AllEnemies[i].beenRemoved == true)
            {
                Destroy(AllEnemies[i].gameObject);
                AllEnemies.RemoveAt(i);
            }
        }

    }

    public void ClearEnemyPieces()
    {
        for (int i = 0; i < AllEnemies.Count; i++)
        {
            if (AllEnemies[i].EnteredDaughtersRoom == true)
            {                
                Destroy(AllEnemies[i].gameObject);
                AllEnemies.RemoveAt(i);
            }
            if (AllEnemies[i].beenRemoved == true)
            {
                Destroy(AllEnemies[i].gameObject);
                AllEnemies.RemoveAt(i);
            }
        }

        if (GameIsOver == true)
        {
            for (int i = 0; i < AllEnemies.Count; i++)
            {
                AllEnemies[i].objectToMove.useGravity = true;
            }
        }
    }

    public void cleanTheBoard()
    {
        for (int i = 0; i < AllEnemies.Count; i++)
        {
            Destroy(AllEnemies[i].gameObject);
            AllEnemies.RemoveAt(i);
        }

    }
}
