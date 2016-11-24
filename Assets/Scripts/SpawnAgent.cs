using UnityEngine;
using System.Collections.Generic;


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

    enum enumType
    {
       investigator, interprete, leader, apotre, relaisPassif, resistant, lambda
    }

    void Start()
    {
        initiateValues();
        typeSpawn(10,1,1,1,1,1,1);
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
            spawn(enumType.lambda);
        }
        for (int i = 0; i < investigator; i++)
        {
            spawn(enumType.investigator);
        }
        for (int i = 0; i < interprete; i++)
        {
            spawn(enumType.interprete);
        }
        for (int i = 0; i < apotre; i++)
        {
            spawn(enumType.apotre);
        }
        for (int i = 0; i < relaisPassif; i++)
        {
            spawn(enumType.relaisPassif);
        }
        for (int i = 0; i < leader; i++)
        {
            spawn(enumType.leader);
        }
        for (int i = 0; i < resistant; i++)
        {
            spawn(enumType.resistant);
        }
        for (int i = 0; i < leader; i++)
        {
            spawn(enumType.leader);
        }
    }

    void spawn(enumType t)
    {
        // Find a random index between zero and one less than the number of spawn points.
        float randomX = Random.Range(xMin, xMax);
        float randomZ = Random.Range(zMin, zMax);

        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        
        GameObject go =Instantiate(agent, new Vector3(randomX, 2, randomZ), agent.gameObject.GetComponent<Transform>().rotation) as GameObject;
        go.transform.SetParent(this.gameObject.transform);
        switch (t)
        {
            case enumType.investigator:
                go.AddComponent<Investigator>();
                go.name = "investigator";
                break;

            case enumType.interprete:
                go.AddComponent<Interprete>();
                go.name = "interpretre";
                break;

            case enumType.apotre:
                go.AddComponent<Apotre>();
                go.name = "apotre";
                break;

            case enumType.leader:
                go.AddComponent<Leader>();
                go.name = "leader";
                int nbSuiveurs = Random.Range(2, 5);
                for (int i = 0; i < nbSuiveurs; i++)
                {
                    int indexSuiveur = Random.Range(0, lambdas.Count);
                    go.GetComponent<Leader>().addSuiveur(lambdas[indexSuiveur].GetComponent<Agent>());
                }
                break;

            case enumType.relaisPassif:
                go.AddComponent<RelaisPassif>();
                go.name = "relais passif";
                break;

            case enumType.resistant:
                go.AddComponent<Resistant>();
                go.name = "résistant";
                break;

            case enumType.lambda:
                go.AddComponent<Agent>();
                go.name = "lambda";
                lambdas.Add(go);
                break;

        }
    }

}


