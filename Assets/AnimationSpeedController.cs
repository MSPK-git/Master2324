using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.XR;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AnimationSpeedController : MonoBehaviour
{
    Animator bdAnimation;
    SplineAnimate splineAnimate;
    public float speedDecrementTrain = 0.2f; 
    public float speedDecrementBreakDance = 1.0f; 

    void Start()
    {
        bdAnimation = GetComponent<Animator>();
        splineAnimate = GetComponent<SplineAnimate>();
    }

    void Update()
    {
        OVRInput.Update();
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.Touch))
        {
            Debug.Log("controller test");
        }
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            IncreaseAnimationSpeed();
            print("eins");
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            DecreaseAnimationSpeed();
            print("zwei");
        }
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            print("quit");
            Quit();
        }
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {

        }
    }

    void IncreaseAnimationSpeed()
    {
        float speedIncrement = (gameObject.name == "Train") ? speedDecrementTrain : speedDecrementBreakDance;

        if (bdAnimation != null)
        {
            bdAnimation.speed += speedIncrement;
        }

        if (splineAnimate != null)
        {
            splineAnimate.MaxSpeed += speedIncrement;
        }
    }

    void DecreaseAnimationSpeed()
    {
        float speedDecrement = (gameObject.name == "Train") ? speedDecrementTrain : speedDecrementBreakDance;

        if (bdAnimation != null)
        {
            bdAnimation.speed -= speedDecrement;
        }

        if (splineAnimate != null)
        {
            splineAnimate.MaxSpeed -= speedDecrement;
        }
    }

    void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}