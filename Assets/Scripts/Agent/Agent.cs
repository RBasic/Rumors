﻿using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{

    private Color defaultcolor;

    //Croyance de la rumeur 0 = croit pas, 1 = croit, 0.5 = a moitie
    private float doute;
    private float deltaDoute=0.0f;
    //private bool like = false; //aime parler de la rumeur

    private bool aware = false;
    private bool isSpeaking = false; // currently speaking
    private int nbHeard = 0; // time the agent heard the rumor
    private int palierHeardToIncrease = 10;
    private int cptPalierNbHeard = 1;

    private bool isTarget = false;

    public TextMesh t;

    void Start()
    {
        t = gameObject.GetComponentInChildren<TextMesh>();
        defaultcolor = this.gameObject.GetComponent<MeshRenderer>().material.color;
        int randomDoute = Random.Range(0, 11);
        doute = (float) randomDoute/10.0f;
        t.text = doute.ToString();
        changeColor();
    }

    public Color getDefaultColor()
    {
        return defaultcolor;
    }

    public void interaction(Agent a)
    {
        // if the other agent doesn't talk
        if (!a.getIsSpeaking())
        {
            a.setIsSpeaking(true);
            isSpeaking = true;
            a.setNbHeard();
            // when done
            a.setIsSpeaking(false);
            isSpeaking = false;
        }
    }

    public void setDoute(float val)
    {
        if (doute + val >= 0.0f && doute + val <= 1.0f)
        {
            doute += val;
            deltaDoute += val;
            changeColor();
            t.text = doute.ToString();

        }
    }

    public float getDoute()
    {
        return doute;
    }

    public void setNbHeard()
    {
        nbHeard++;
        if (nbHeard/ palierHeardToIncrease > cptPalierNbHeard)
        {
            cptPalierNbHeard++;
            setDoute(0.1f);
        }
    }

    public void setIsSpeaking(bool state)
    {
        isSpeaking = state;
    }

    public bool getIsSpeaking()
    {
        return isSpeaking;
    }

    public float getDeltadoute()
    {
        return deltaDoute;
    }

    public void setDeltaDoute()
    {
        deltaDoute = 0;
    }

    public void setIsTarget(bool state)
    {
        isTarget = state;
    }

    public bool getIsTarget()
    {
        return isTarget;
    }

    void changeColor()
    {
        Color c = Color.Lerp(GameManager.instance.getColorDoubt0(), GameManager.instance.getColorDoubt1(), doute);  // Color C is doute% from A and 90% from B. 
        gameObject.GetComponent<MeshRenderer>().material.color = c;
    }

}
