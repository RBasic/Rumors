using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomMoving : MonoBehaviour
{
    Collider[] around;
    float tempsDeplacement = 5;
    public float rayonDetection = 2;

    public float rotationSpeed=0.5f;
    public float movementSpeed = 0.5f;
    public float rotationTime = 0.5f;

    private List<GameObject> avoidAgents = new List<GameObject>();
    private GameObject bla;


    void Start()
    {
        bla = Instantiate(GameManager.instance.getBlabla());
        bla.SetActive(false);

        float randomRotation = Random.Range(0, 360);
        this.transform.Rotate(0,randomRotation,0); 
        Invoke("ChangeRotation", rotationTime);
    }

    void ChangeRotation()
    {
        if (Random.value > 0.5f)
        {
            rotationSpeed = -rotationSpeed;
        }
        Invoke("ChangeRotation", rotationTime);
    }


    void Update()
    {
        if (!this.gameObject.GetComponent<Agent>().getIsTarget())
        {
            transform.Rotate(new Vector3(0, rotationSpeed*Time.deltaTime, 0));
            transform.position += transform.forward*movementSpeed*Time.deltaTime;

            checkAround();
        }


    }

    void checkAround()
    {
        around = Physics.OverlapSphere(gameObject.transform.position, rayonDetection);
        string s = "";
     
        foreach (Collider c in around)
        {
            if (c.gameObject!= this.gameObject && c.gameObject.tag=="Agent" && !avoidAgents.Contains(c.gameObject) && !c.gameObject.GetComponent<Agent>().getIsTarget())
            {
                c.gameObject.GetComponent<Agent>().setIsTarget(true);
                this.gameObject.GetComponent<Agent>().setIsTarget(true);

                avoidAgents.Add(c.gameObject);
                c.GetComponent<RandomMoving>().addAvoidAgents(this.gameObject);


                float x =(c.gameObject.transform.position.x+this.gameObject.transform.position.x)/2;
                float y = bla.transform.position.y;
                float z = (c.gameObject.transform.position.z + this.gameObject.transform.position.z) / 2;

                bla.SetActive(true);
                bla.GetComponent<Transform>().position = new Vector3(x,y,z);

                StartCoroutine(deplacement(c.transform.position, tempsDeplacement, c.gameObject.GetComponent<Agent>()));
                break;
            }
        }
    }

    IEnumerator deplacement(Vector3 pos, float time, Agent a)
    {
        float elapsedtime = 0;
        Vector3 startingpos = transform.position;
        while (elapsedtime < time)
        {
            transform.position = Vector3.Lerp(startingpos, pos, (elapsedtime / time));
            elapsedtime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.GetComponent<Agent>().interaction(a);       // interaction
        a.gameObject.GetComponent<Agent>().setIsTarget(false);       // re-move
        this.gameObject.GetComponent<Agent>().setIsTarget(false);    // re-move
        bla.SetActive(false);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, rayonDetection/2);
    }

    public void addAvoidAgents(GameObject go)
    {
        avoidAgents.Add(go);
    }
}

