using UnityEngine;
using System.Collections;

public class ControlledCharacter : MonoBehaviour {

    //movement
    public const int SPEED = 5;
    private bool MovementCompleted = true;
    private SpriteRenderer sprite;
    private Vector3 offset;
    private bool buttonPress = false;
    private int shotsRemaining = 1;
    private Vector2 sizeRef;

    private Vector3 MoveTo;
    private BoardPiece[] MovementLocations;

    private int currentLocation = 0;
    private MoveDirection movedirection = MoveDirection.none;
    private enum MoveDirection {Left, Right, none };

    public DropProjectile refDrop;

    public bool canShoot = true;




	// Use this for initialization
	void Start () {
        sprite = this.GetComponent<SpriteRenderer>();
    
	}
	
	// Update is called once per frame
	void Update () {
        buttonPress = false;
        movedirection = MoveDirection.none;
        if (Input.GetKeyDown(KeyCode.A))
        {
            buttonPress = true;
            movedirection = MoveDirection.Left;
            int newLocation = currentLocation - 1;
            if (newLocation < 0)
            {
                return;
            }
            MoveTo = MovementLocations[newLocation].transform.position + offset;
            currentLocation--;

        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            buttonPress = true;
            movedirection = MoveDirection.Right;
            int newLocation = currentLocation + 1;
            if (newLocation > MovementLocations.Length - 1)
            {
                return;
            }
     
            MoveTo = MovementLocations[newLocation].transform.position + offset;
            currentLocation++;
        }

        if (buttonPress && MovementCompleted)
        {
            
            MoveToLocation(MoveTo) ;
        }
        else if (!MovementCompleted)
        {
            MoveCharacter();
        }

        if (Input.GetKeyDown(KeyCode.Space) && MovementCompleted && canShoot)
        {
            canShoot = false;
            DropProjectile tempEnemy = (DropProjectile)Instantiate(refDrop);
            tempEnemy.setUpProjectile(this.transform.position + new Vector3 (sprite.bounds.size.x / 2, 0,0), sizeRef);
        }

      



    }

    public void SetSize(Vector2 size)
    {
        sizeRef = size;
        this.transform.localScale = new Vector3(size.x, size.y, 0);
        offset = new Vector3(0, sprite.bounds.size.y, 0);
        this.transform.position = new Vector3(MovementLocations[0].transform.position.x, MovementLocations[0].transform.position.y + offset.y);

    }

    public void MoveToLocation(Vector3 target)
    {
        MoveTo = target;
        MovementCompleted = false;
    }

    public void MoveCharacter()
    {

        if (transform.position == MoveTo)
        {
            MovementCompleted = true;
        }
        else
        {
            float step = SPEED * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, MoveTo, step);
        }
    }

    public void setMovementLocations(BoardPiece[] temp)
    {
        MovementLocations = temp;

    }
}
