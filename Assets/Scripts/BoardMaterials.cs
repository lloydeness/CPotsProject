using UnityEngine;
using System.Collections;

public enum BM_Sprite { BM_Normal, BM_Window, BM_Daughter};
public enum EnemyType {Enemy_Diagonal, Enemy_Straight,Enemy_DiagonalR,Enemy_WindowCreeper, Enemy_Zipline }



public class BoardMaterials : MonoBehaviour {


 
   //Background Materials sprites
    public Sprite[] SpriteArray;
    // Enemy Material Sprites
    public Sprite[] EnemySpriteArray;


  
    void Awake()
    {

    }

	// Use this for initialization
	void Start () {



    }
	
	// Update is called once per frame
	void Update () {
	
	}

    // get the sprite
    public Sprite getMaterial(int sprite)
    {

        return SpriteArray[sprite];
    }
    public Sprite getEnemyMaterials (int sprite)
    {
        return EnemySpriteArray[sprite];
    }

}
