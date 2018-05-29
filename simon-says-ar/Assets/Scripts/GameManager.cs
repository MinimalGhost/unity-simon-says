using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // marker left and right halves
    public GameObject left;
    public GameObject right;

    public GameObject red;
    public GameObject blue;
    public GameObject yellow;
    public GameObject green;
    float speed = 20.0f;

    public TapButton[] buttons;
    public List<int> currentSequence;

    public float timeLit = 0.5f; 
    public float intervalTime = 0.5f; 

    public int simonSequence = 2;
    private int playerSequence = 0;

    bool simonTurn = false;
    public bool playerTurn = false;

    private int simonChoice;

    public Button StartButton;
    public Text GameOverText;

	private void Awake()
	{
        GameOverText.text = "";
        StartCoroutine(Intro());
	}


	// Use this for initialization
	void Start () {
        // Loop through buttons to assign click event and associated ids
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i].onClick += ButtonTapped;
            buttons[i].buttonId = i;
        } 

    }

    void PlayAudio(int index)
    {
        float audioLength = timeLit;
        float frequency = 0.0005f * ((float)index + 1f);

        // generate waveform 
        AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, -1f), new Keyframe(audioLength, 0f, -1f, 0f));
        AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0f, frequency, 0f, 0f), new Keyframe(audioLength, frequency, 0f, 0f));

        // set waveform parameters
        LeanAudioOptions audioOptions = LeanAudio.options();
        audioOptions.setWaveSine();
        audioOptions.setFrequency(44100);

        // create clip of our waveform
        AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, audioOptions);

        // play it
        LeanAudio.play(audioClip, 0.5f);
    }

    void ButtonTapped(int _number)
    {
        // is it the players turn?
        if (playerTurn)
        {
            /* if the correct next button in sequence is tapped then
             * allow the player to continue, otherwise end the game */
            if (_number == currentSequence[playerSequence])
            {
                PlayAudio(_number);
                playerSequence += 1;
            } 
            else
            {
                GameOver();
            }

            // player has successfully completed the current sequence
            if (playerSequence == simonSequence)
            {
                simonSequence += 1;
                playerSequence = 0;
                playerTurn = false;
                simonTurn = true;
            }
        }
    }
	
	// Update is called once per frame
	private void Update () {
        if (simonTurn) {
            simonTurn = false;
            StartCoroutine(Simon());
        }
        StartCoroutine(RotateCubes());
	}

    private IEnumerator Intro() 
    {
        StartButton.interactable = false;

        yield return new WaitForSeconds(2f);
        // open marker
        StartCoroutine(MoveObject(left, new Vector3(-0.146f, 0.002f, 0), new Vector3(-0.446f, 0.002f, 0), 1.8f));
        StartCoroutine(MoveObject(right, new Vector3(0.146f, 0.002f, 0), new Vector3(0.446f, 0.002f, 0), 1.8f));

        // bring up cubes
        StartCoroutine(MoveObject(red, new Vector3(0.1f, -0.219f, 0.1f), new Vector3(0.1f, 0.2f, 0.1f), 4f));
        StartCoroutine(MoveObject(blue, new Vector3(0.1f, -0.219f, -0.1f), new Vector3(0.1f, 0.2f, -0.1f), 4f));
        StartCoroutine(MoveObject(yellow, new Vector3(-0.1f, -0.219f, -0.1f), new Vector3(-0.1f, 0.2f, -0.1f), 4f));
        StartCoroutine(MoveObject(green, new Vector3(-0.1f, -0.219f, 0.1f), new Vector3(-0.1f, 0.2f, 0.1f), 4f));

        yield return new WaitForSeconds(4f);
        // close marker
        StartCoroutine(MoveObject(left, new Vector3(-0.446f, 0.002f, 0), new Vector3(-0.14f, 0.002f, 0), 2.5f));
        StartCoroutine(MoveObject(right, new Vector3(0.446f, 0.002f, 0), new Vector3(0.14f, 0.002f, 0), 2.5f));
        // spread out cubes
        StartCoroutine(MoveObject(red, new Vector3(0.1f, 0.2f, 0.1f), new Vector3(0.2f, 0.2f, 0.2f), 2.5f));
        StartCoroutine(MoveObject(blue, new Vector3(0.1f, 0.2f, -0.1f), new Vector3(0.2f, 0.2f, -0.2f), 2.5f));
        StartCoroutine(MoveObject(yellow, new Vector3(-0.1f, 0.2f, -0.1f), new Vector3(-0.2f, 0.2f, -0.2f), 2.5f));
        StartCoroutine(MoveObject(green, new Vector3(-0.1f, 0.2f, 0.1f), new Vector3(-0.2f, 0.2f, 0.2f), 2.5f));
        StartButton.interactable = true;
    }

    private IEnumerator RotateCubes() 
    {
        yield return new WaitForSeconds(6f);
        // rotate red
        red.transform.Rotate(Vector3.up, speed * Time.deltaTime);
        red.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        red.transform.Rotate(Vector3.left, speed * Time.deltaTime);
        // rotate blue
        blue.transform.Rotate(Vector3.down, speed * Time.deltaTime);
        blue.transform.Rotate(Vector3.back, speed * Time.deltaTime);
        blue.transform.Rotate(Vector3.right, speed * Time.deltaTime);
        // rotate yellow
        yellow.transform.Rotate(Vector3.up, speed * Time.deltaTime);
        yellow.transform.Rotate(Vector3.back, speed * Time.deltaTime);
        yellow.transform.Rotate(Vector3.left, speed * Time.deltaTime);
        // rotate green
        green.transform.Rotate(Vector3.down, speed * Time.deltaTime);
        green.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        green.transform.Rotate(Vector3.right, speed * Time.deltaTime);

    }

    private IEnumerator MoveObject(GameObject obj, Vector3 source, Vector3 target, float overTime) 
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            obj.transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }

        //obj.transform.position = target;
    }

    private IEnumerator Simon() {
        // always wait one second before starting next sequence
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < simonSequence; i++) 
        {
            // check if simon needs to add a new button to the sequence
            if (currentSequence.Count < simonSequence)
            {
                // select the next color and add it to sequence
                simonChoice = Random.Range(0, buttons.Length);
                currentSequence.Add(simonChoice);
            }
            // display the sequence as simon goes through it
            buttons[currentSequence[i]].TappedColor();
            PlayAudio(currentSequence[i]);
            yield return new WaitForSeconds(timeLit);
            buttons[currentSequence[i]].DefaultColor();
            yield return new WaitForSeconds(intervalTime);
        }
        // increment the difficulty each time the sequence grows
        timeLit -= 0.05f;
        intervalTime -= 0.05f;
        playerTurn = true;
    }

    public void StartGame()
    {
        currentSequence.Clear();
        simonTurn = true;
        timeLit = 0.5f;
        intervalTime = 0.5f;
        playerSequence = 0;
        simonSequence = 3;
        GameOverText.text = "";
        StartButton.interactable = false;
    }

    private void GameOver() 
    {
        GameOverText.text = "Game Over";
        StartButton.interactable = true;
        playerTurn = false;
    }

}
