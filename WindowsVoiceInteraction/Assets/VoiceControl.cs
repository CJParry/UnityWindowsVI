using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceControl : MonoBehaviour
{
    private Dictionary<string, Action> keyActs = new Dictionary<string, Action>();
    private KeywordRecognizer recognizer;
    //for colour manipulation
    private MeshRenderer cubeRend;
    //for sound playback
    private AudioSource soundSource;
    public AudioClip[] sounds;

    //Var needed for spin manipulation
    private bool spinningRight;

    // Start is called before the first frame update
    void Start()
    {
        cubeRend = GetComponent<MeshRenderer>();
        cubeRend.enabled = false;

        soundSource = GetComponent<AudioSource>();

        //Voice commands for changing color
        keyActs.Add("red", Red);
        keyActs.Add("green", Green);
        keyActs.Add("blue", Blue);
        keyActs.Add("white", White);

        //Voice commands to create a box
        keyActs.Add("create a box", CreateBox);

        //Voice commands for spinning
        keyActs.Add("spin right", SpinRight);
        keyActs.Add("spin left", SpinLeft);

        //Voice commands for playing sound
        keyActs.Add("please say something", Talk);

        //Voice command to show how complex it can get.
        keyActs.Add("pizza is a wonderful food that makes the world better", FactAcknowledgement);

        recognizer = new KeywordRecognizer(keyActs.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnKeywordsRecognized;
        recognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Command: " + args.text);
        keyActs[args.text].Invoke();
    }
    void CreateBox()
    {
        cubeRend.enabled = true;
    }
    void Red()
    {
        cubeRend.material.SetColor("_Color", Color.red);
    }
    void Green()
    {
        cubeRend.material.SetColor("_Color", Color.green);
    }
    void Blue()
    {
        cubeRend.material.SetColor("_Color", Color.blue);
    }
    void White()
    {
        cubeRend.material.SetColor("_Color", Color.white);
    }

    void SpinRight()
    {
        spinningRight = true;
        StartCoroutine(RotateObject(1f));
    }
    void SpinLeft()
    {
        spinningRight = false;
        StartCoroutine(RotateObject(1f));
    }
    private IEnumerator RotateObject(float duration)
    {
        float startRot = transform.eulerAngles.x;
        float endRot;
        if (spinningRight)
            endRot = startRot - 360f;
        else
            endRot = startRot + 360f;
        float t = 0f;
        float yRot;
        while (t < duration)
        {
            t += Time.deltaTime;
            yRot = Mathf.Lerp(startRot, endRot, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRot, transform.eulerAngles.z);
            yield return null;
        }
    }
    void Talk()
    {
       // soundSource.clip = sounds[UnityEngine.Random.Range(0, sounds.Length)];
       // soundSource.Play();
    }
    void FactAcknowledgement()
    {
        Debug.Log("How right you are.");
    }
}
