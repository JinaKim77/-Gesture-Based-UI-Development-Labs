using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.IO;
using UnityEngine.Windows.Speech;  // grammar recognizer
using System.Text;  // for StringBuilder

public class GrammarController : MonoBehaviour
{
    private GrammarRecognizer gr;

    private void Start()
    {
        gr = new GrammarRecognizer(Application.streamingAssetsPath + "/SimpleGrammar.xml", ConfidenceLevel.Medium);
        Debug.Log("Grammar loaded!");
        gr.OnPhraseRecognized += GR_OnPhraseRecognized;
        gr.Start();

        if(gr.IsRunning)
        {
            Debug.Log("Grammar is running!");
        }else{
            Debug.Log("Grammar is not running!");
        }
    }

    private void GR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder message = new StringBuilder();
        Debug.Log("Recognized a phrase");

        // Read the semantic meanings from the args passed in. 
        SemanticMeaning[] meanings = args.semanticMeanings;

        // Move pawn from C2 to C4 - Piece, Start, Finish
        // semantic meanings are returned as key/value pairs.
        // Piece/"pawn", Start/"C2", Finish/"C4"
        // user foreach to get all the meanings.

        foreach(SemanticMeaning meaning in meanings)
        {
            string keyString = meaning.key.Trim();
            string valueString = meaning.values[0].Trim();

            message.Append("Key: " + keyString + ", Value: " + valueString);
        }
        // use a string builder to create the string and output to the user.
        Debug.Log(message);
    }

    // failsafe stop mechanism
    private void OnApplicationQuit(){
        if(gr!=null & gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }
    }

}
