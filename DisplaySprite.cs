using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif 
using UnityEngine;
using UnityEngine.UI;

public class DisplaySprite : MonoBehaviour {
    public GameObject displaySprite;
    // Use this for initialization
    void Start () {
        #if UNITY_EDITOR
        AssetDatabase.Refresh();
        #endif
        displaySprite = GameObject.Find("Sprite2Crop");
        Texture2D tex = Resources.Load("pic", typeof(Texture2D)) as Texture2D;
        Sprite sprite = new Sprite();
        sprite = Sprite.Create(tex, new Rect(0, 0, 686, 342), new Vector2(0.5f, 0.5f));
        SpriteRenderer SR = displaySprite.GetComponent<SpriteRenderer>();
        SR.sprite = sprite;


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
