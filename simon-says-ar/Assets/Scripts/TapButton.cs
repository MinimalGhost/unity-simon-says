using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapButton : MonoBehaviour {

    public Material defaultMat;
    public Material litMat;

    private Renderer rend;
    private Vector3 buttonPos;

    public int buttonId = 32;
    public GameManager SimonLogic; 

    public delegate void ClickEvent(int number);

    public event ClickEvent onClick;

	// Use this for initialization
	private void Awake () {
        // ensure renderer is enabled
        rend = GetComponent<Renderer>();
        rend.enabled = true;

	}
	
	// Update is called once per frame
	void Update () {
        // remember current button position
        buttonPos = transform.position;
	}

	private void OnMouseDown()
	{
        if (SimonLogic.playerTurn)
        {
            // color changes on tap
            TappedColor();

            // get the instance id
            onClick.Invoke(buttonId);
        }

	}

	private void OnMouseUp()
	{
        DefaultColor();

        transform.position = new Vector3(buttonPos.x, buttonPos.y, buttonPos.z);
	}

    public void TappedColor()
    {
        rend.sharedMaterial = litMat;
    }

    public void DefaultColor()
    {
        rend.sharedMaterial = defaultMat;
    }
}
