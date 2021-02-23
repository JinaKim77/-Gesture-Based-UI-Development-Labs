using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Windows.Speech;

public class Crosshair : MonoBehaviour
{
    // You need Microphone for this functionalities
    // File -> build settings -> player settings -> player -> window -> publish settings -> capabilities -> Microphone

    [SerializeField] private string[] keywords;
    [SerializeField] private float speed = 5.0f;  // move the crosshair at this speed

    private KeywordRecognizer kr;  // up down left right shoot/fire
    private Rigidbody2D rb;

    // Action is in System, using System or System.Action
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    // Confidence Level
    private ConfidenceLevel confidenceLevel = ConfidenceLevel.Medium;

    private string spokenword = "";

    void Start()
    {
        actions.Add("Up",Up);
        actions.Add("Down",Down);
        actions.Add("Left",Left);
        actions.Add("Right",Right);

        rb = GetComponent<Rigidbody2D>();

        if(keywords != null)
        {
            //kr = new KeywordRecognizer(keywords, confidenceLevel);
            kr = new KeywordRecognizer(actions.Keys.ToArray(), confidenceLevel);
            kr.OnPhraseRecognized += KR_OnPhraseRecognized;
            kr.Start();
        }
        
    }

    void KR_OnPhraseRecognized(PhraseRecognizedEventArgs args) {
        spokenword = args.text;
        Debug.Log("You said  "+ spokenword);
        actions[spokenword].Invoke();
    }

    // == Actions Methods ==
    // void return type and no parameters
    private void Up()
    {
        transform.Translate(0,1,0);
    }

    private void Down()
    {
        transform.Translate(0,-1,0);
    }

    private void Left()
    {
        transform.Translate(-1,0,0);
    }

    private void Right()
    {
        transform.Translate(1,0,0);
    }

/*
    void Update()
    {
        // how to apply that command from the spoken word
        float h_Movement = 0, v_Movement = 0;

        switch (spokenword)
        {
            case "Up":
                v_Movement = 1;
                break;

            case "Down":
                v_Movement = -1;
                break;

            case "Left":
                h_Movement = -1;
                break;

            case "Right":
                h_Movement = 1;
                break;
            
            case "Stop":
                h_Movement = v_Movement = 0;
                break;
                
            default:
                break;
        }

        rb.velocity = new Vector2(h_Movement * speed, v_Movement * speed);
    }
*/

    // failsafe stop mechanism
    private void OnApplicationQuit(){
        if(kr!=null & kr.IsRunning)
        {
            kr.OnPhraseRecognized -= KR_OnPhraseRecognized;
            kr.Stop();
        }
    }
}
