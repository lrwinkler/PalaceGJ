using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTexture : MonoBehaviour {

    public List<Sprite> sprites;

	// Use this for initialization
	void Start () {
        if (Random.Range(0, 5) == 0)
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(1, sprites.Count)];
        }
    }
	
}
