﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

// TODO :
// Statique : 
//    faire des zones de departs, puis procéder petit à petit à l'évoluetion de la rumeur
//   tout le mond eparle ? que faire si impair ? laisser du temps avant de reparler?
// Bouge : 
//    voir les vol planés
// Tout à la fois : 
//    rester dans le cadre
//    ajouter un timer
//    ajouter des courbes ? ou alors un tableau qui ressence les doutes
//    bouton reload
//    pb de lumière
//    pb de taille
// Théorie :
//    2 lambdas ensembles ?

public class GameManager : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private GameObject panelInfo;
    [SerializeField] private Text panelInfoType;
    [SerializeField] private Text panelInfoDoubt;
    [SerializeField] private GameObject blabla;
    private GameObject currentAgent;

    [Header("Colors of doubt")]
    [SerializeField]
    private Color colorDoubt0;
    [SerializeField]
    private Color colorDoubt1;

    [Header("Static ou mouvement")]
    [SerializeField]
    private Toggle bougeToogle;

    [SerializeField] private Material lineMat;

    private bool mouvement;

    [Header("Graph")]
    [SerializeField]
    private GameObject graph;

    LineRenderer line;
    [SerializeField]
    private float heightGraph;
    public List<float> tabDoubt; //en public pour regarder les valeurs sur le gameManager

    [Header("Jauge")]
    [SerializeField]
    private GameObject jauge;
    LineRenderer lineJauge;

    // liste de tous les elements
    private List<Agent> allAgents = new List<Agent>();

    public enum enumType
    {
        investigator, interprete, leader, apotre, relaisPassif, resistant, lambda
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;
    }

    void Start()
    {
        line = graph.GetComponent<LineRenderer>();
        tabDoubt = new List<float>();

        lineJauge = jauge.GetComponent<LineRenderer>();
        lineJauge.SetColors(colorDoubt0, colorDoubt1);

    }
    public static GameManager instance
    {
        get
        {
            return _instance;
        }
    }
    private static GameManager _instance;

    
    public void displayPanelInfo(bool state)
    {
        panelInfo.SetActive(state);
    }

    public void setCurrentAgent(GameObject agent, Color defaultColor)
    {
        if(currentAgent!=null)
            currentAgent.GetComponent<MeshRenderer>().material.color = defaultColor;
        currentAgent = agent;
    }

    float t = 0;
    int length = 0;
    float divide = 1;
    void Update()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.gameObject.tag == "Agent")
                {
                    if (currentAgent != null)
                    {
                        currentAgent.GetComponent<MeshRenderer>().material.color =
                            currentAgent.GetComponent<Agent>().getDefaultColor();
                    }
                    currentAgent = hit.transform.gameObject;
                    currentAgent.GetComponent<MeshRenderer>().material.color = selectedColor;
                }
                else
                {
                    if (currentAgent != null)
                    {
                        currentAgent.GetComponent<MeshRenderer>().material.color =
                            currentAgent.GetComponent<Agent>().getDefaultColor();
                        currentAgent = null;
                    }
                }
                updatePanelInfo();
            }
        }
        displayJauge();
        if (allAgents.Count != 0)
        {
            drawCourbe();
        }
        
    }

    private void updatePanelInfo()
    {
        if (currentAgent != null)
        {
            panelInfo.SetActive(true);
            panelInfoDoubt.text = currentAgent.GetComponent<Agent>().getDoute().ToString();
            panelInfoType.text = currentAgent.GetComponent<Agent>().getTypeAgent().ToString();
        }
        else
        {
            panelInfo.SetActive(false);
        }
    }

    public GameObject getBlabla()
    {
        return blabla;
    }

    public Color getColorDoubt0()
    {
        return colorDoubt0;
    }

    public Color getColorDoubt1()
    {
        return colorDoubt1;
    }

    public void initBoolMouvement()
    {
        mouvement = bougeToogle.isOn;
    }

    public bool getMouvement()
    {
        return mouvement;
    }

    public Material getLineMat()
    {
        return lineMat;
    }

    public List<Agent> getAllAgents()
    {
       
        return allAgents;
    }

    public List<GameObject> getAllAgentsInGameObject()
    {
        List<GameObject> listGo = new List<GameObject>();
        foreach (Agent a in allAgents)
        {
            listGo.Add(a.gameObject);
        }
        return listGo;

    }

    public float getDoubtMoy()
    {
        int nbAgent = allAgents.Count;
        float doubt = 0;
        foreach(Agent a in allAgents)
        {
            doubt += a.getDoute();
        }

        doubt /= nbAgent;
        //Debug.Log("doubt = " + doubt);

        return doubt;
    }

    public void save()
    {
        string s = "";
        float[] tab = tabDoubt.ToArray();
        for (int i = 0; i < tab.Length; i++)
        {
            s += tab[i].ToString();
            s += "\r\n";
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + "/sortie.txt", s);
    }

    public void drawCourbe()
    {
        t += Time.deltaTime;
        if (t > 1)
        {
            length++;
            line.SetVertexCount(length);    // on augmente la taille de la courbe
            t = 0;
            divide++;                       // on divise pour avoir toute la courbe
            tabDoubt.Add(getDoubtMoy());    // recuperation du doute moyen
        }
        for (int i = 0; i < length; i++)
        {
            Vector3 pos = new Vector3((float)(((i / divide) * 70) + graph.transform.position.x), 0, (tabDoubt[i]) * 50); //valeur du doute + 
                                                                                                                         //placement dans la scene (pas hyper précis, juste pour que ça rentre pile dans le graphe)
            line.SetPosition(i, pos);   //ecriture dans le line renderer au bon indice
        }
    }

    private void displayJauge()
    {
        int nbAgent = allAgents.Count;

        if (nbAgent != 0)
        {
           
            Dictionary<int, float> indexDoubt = new Dictionary<int, float>();
            int nbPosition = -1;

            float startDoubt = -1.0f;
            float lastDoubt = -1.0f;
            float minHeight = 0;
            float maxHeight = 0;

            int nbAgentAtThisDoubt = 0;
            int nbAgentHeight = 0;

            for (float doubt = 0.0f; doubt <= 1.1f; doubt += 0.1f)
            {
                nbAgentAtThisDoubt = 0;
                foreach (Agent a in allAgents)
                {
                   
                    if ((int)doubt == 1 && a.getDoute()==1.0f)
                    {
                        nbAgentAtThisDoubt++;
                        nbAgentHeight++;
                    }
                    if (doubt == 0.0f && a.getDoute() == 0.0f)
                    {
                        nbAgentAtThisDoubt++;
                        nbAgentHeight++;
                    }
                    else if ( doubt == a.getDoute())
                    {
                        nbAgentAtThisDoubt++;
                        nbAgentHeight++;
                    }
                }

                if (nbAgentAtThisDoubt!=0)
                {
                    if (startDoubt == -1)
                    {
                        startDoubt = doubt;
                        minHeight = getHeight(nbAgentHeight, nbAgent);
                    }
                    nbPosition++;
                    maxHeight = getHeight(nbAgentHeight, nbAgent);
                    indexDoubt.Add(nbPosition, maxHeight);
                    lastDoubt = doubt;
                }
                
            }

            lineJauge.SetVertexCount(indexDoubt.Count);
            for(int i=0; i< (indexDoubt.Count); i++)
            {
                indexDoubt[i] = rescaleHeight(indexDoubt[i],minHeight,maxHeight, 0.0f, heightGraph);
                lineJauge.SetPosition(i, positionOnGraph(new Vector3(0, indexDoubt[i], 0)));
            }
          
            lineJauge.SetWidth(1, 1);
            Color c0 = Color.Lerp(colorDoubt0, colorDoubt1, startDoubt);
            Color c1 = Color.Lerp(colorDoubt0, colorDoubt1, lastDoubt);

            lineJauge.SetColors(c0,c1);
        }

    }

    Vector3 positionOnGraph(Vector3 worldPosition)
    {
        Vector3 offset = new Vector3(-jauge.GetComponent<Renderer>().bounds.size.x, jauge.GetComponent<Renderer>().bounds.size.y,0);
        return (worldPosition + jauge.transform.position+ offset);
    }

    float getHeight(int nbAgentAtThisDoubt, int nbAgent)
    {
        return (nbAgentAtThisDoubt * heightGraph) / nbAgent;
    }

    float rescaleHeight(float x,float min, float max, float toA, float toB)
    {
        return (((toB-toA)*(x-min)) / (max- min)) + toA;
    }
}
