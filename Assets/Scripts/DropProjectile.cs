using UnityEngine;
using System.Collections;

public class DropProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void setUpProjectile(Vector3 Position, Vector2 scale)
    {
        this.transform.position = Position;
        this.transform.localScale = scale;
        //this.GetComponent<BoxCollider2D>().size = scale / 2;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy temp = other.GetComponentInParent<Enemy>();

        if (temp != null)
        {
            temp.gameObject.GetComponent<Rigidbody>().useGravity = true;
            temp.beenRemoved = true;

        }
    }


}
