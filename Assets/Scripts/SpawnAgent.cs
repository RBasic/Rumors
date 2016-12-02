using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class SpawnAgent : MonoBehaviour
{
    public GameObject agent;                // The agent prefab to be spawned.
    public int nbSpawn = 10;                // Nb of spawn.
    public GameObject floor;

    private float xMin = 0;
    private float xMax = 0;
    private float zMin = 0;
    private float zMax = 0;

    private List<GameObject> lambdas = new List<GameObject>();

 

    [Header("UI")]
    [SerializeField] private GameObject board;
    [SerializeField] private InputField inputInvestigator;
    [SerializeField] private InputField inputInterprete;
    [SerializeField] private InputField inputLeader;
    [SerializeField] private InputField inputApotre;
    [SerializeField] private InputField inputRelaisPassif;
    [SerializeField] private InputField inputResistant;
    [SerializeField] private InputField inputLambda;


    void Start()
    {
        inputLambda.text = "10";
        inputApotre.text = "1";
        inputInterprete.text = "1";
        inputInvestigator.text = "1";
        inputLeader.text = "1";
        inputRelaisPassif.text = "1";
        inputResistant.text = "1";
    }

    public void init()
    {
        GameManager.instance.initBoolMouvement();
        initiateValues();
        int intLambda = int.Parse(inputLambda.text);
        int intApotre = int.Parse(inputApotre.text);
        int intInterprete = int.Parse(inputInterprete.text);
        int intInvestigator = int.Parse(inputInvestigator.text);
        int intLeader = int.Parse(inputLeader.text);
        int intRelaisPassif = int.Parse(inputRelaisPassif.text);
        int intResistant = int.Parse(inputResistant.text);
        board.SetActive(false);
        typeSpawn(intLambda, intInvestigator, intInterprete, intLeader, intApotre, intRelaisPassif, intResistant);
        if (!GameManager.instance.getMouvement())
        {
            foreach (Agent a in GameManager.instance.getAllAgents())
            {
                a.fillListallOtherAgents(GameManager.instance.getAllAgentsInGameObject());
            }
        }
    }

    void initiateValues()
    {
        xMin = -floor.gameObject.GetComponent<Transform>().lossyScale.x*5;
        xMax = floor.gameObject.GetComponent<Transform>().lossyScale.x * 5;
        zMin = -floor.gameObject.GetComponent<Transform>().lossyScale.z * 5;
        zMax = floor.gameObject.GetComponent<Transform>().lossyScale.z * 5;
    }

    void typeSpawn(int lambda, int investigator, int interprete, int leader, int apotre, int relaisPassif, int resistant)
    {
        for (int i = 0; i < lambda; i++)
        {
            spawn(GameManager.enumType.lambda);
        }
        for (int i = 0; i < investigator; i++)
        {
            spawn(GameManager.enumType.investigator);
        }
        for (int i = 0; i < interprete; i++)
        {
            spawn(GameManager.enumType.interprete);
        }
        for (int i = 0; i < apotre; i++)
        {
            spawn(GameManager.enumType.apotre);
        }
        for (int i = 0; i < relaisPassif; i++)
        {
            spawn(GameManager.enumType.relaisPassif);
        }
        for (int i = 0; i < leader; i++)
        {
            spawn(GameManager.enumType.leader);
        }
        for (int i = 0; i < resistant; i++)
        {
            spawn(GameManager.enumType.resistant);
        }
        for (int i = 0; i < leader; i++)
        {
            spawn(GameManager.enumType.leader);
        }
    }

    void spawn(GameManager.enumType t)
    {
        // Find a random index between zero and one less than the number of spawn points.
        float randomX = Random.Range(xMin, xMax);
        float randomZ = Random.Range(zMin, zMax);

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        
        GameObject go =Instantiate(agent, new Vector3(randomX, 2, randomZ), agent.gameObject.GetComponent<Transform>().rotation) as GameObject;
        go.transform.SetParent(this.gameObject.transform);
        switch (t)
        {
            case GameManager.enumType.investigator:
                go.AddComponent<Investigator>();
                go.name = "investigator";
                break;

            case GameManager.enumType.interprete:
                go.AddComponent<Interprete>();
                go.name = "interpretre";
                break;

            case GameManager.enumType.apotre:
                go.AddComponent<Apotre>();
                go.name = "apotre";
                break;

            case GameManager.enumType.leader:
                go.AddComponent<Leader>();
                go.name = "leader";
                int nbSuiveurs = Random.Range(2, 5);
                for (int i = 0; i < nbSuiveurs; i++)
                {
                    int indexSuiveur = Random.Range(0, lambdas.Count);
                    if (lambdas.Count != 0)
                    {
                        go.GetComponent<Leader>().addSuiveur(lambdas[indexSuiveur].GetComponent<Agent>());
                    }
                }
                break;

            case GameManager.enumType.relaisPassif:
                go.AddComponent<RelaisPassif>();
                go.name = "relais passif";
                break;

            case GameManager.enumType.resistant:
                go.AddComponent<Resistant>();
                go.name = "résistant";
                break;

            case GameManager.enumType.lambda:
                go.AddComponent<Agent>();
                go.name = "lambda";
                lambdas.Add(go);
                break;

        }
       
        GameManager.instance.getAllAgents().Add(go.GetComponent<Agent>());
        
        go.GetComponent<Agent>().init(t);
    }

}


