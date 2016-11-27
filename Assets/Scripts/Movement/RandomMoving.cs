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

    LineRenderer line;


    void Start()
    {
        bla = Instantiate(GameManager.instance.getBlabla());
        bla.SetActive(false);
        line = this.gameObject.AddComponent<LineRenderer>();
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
        if (GameManager.instance.getMouvement())
        {
            if (!this.gameObject.GetComponent<Agent>().getIsTarget())
            {
                transform.Rotate(new Vector3(0, rotationSpeed*Time.deltaTime, 0));
                transform.position += transform.forward*movementSpeed*Time.deltaTime;
                checkAround();
            }
        }
        else
        {
            if (!this.gameObject.GetComponent<Agent>().getIsTarget())
            {
                contactAgent();
            }
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



    /**********************************************/

    void contactAgent()
    {
        // s'il reste des agents à contacter
        if (this.gameObject.GetComponent<Agent>().getAllAgents().Count != 0)
        {
            int randomIndex = Random.Range(0, this.gameObject.GetComponent<Agent>().getAllAgents().Count);
            GameObject a = this.gameObject.GetComponent<Agent>().getAllAgents()[randomIndex];
            avoidAgents.Add(a);
            a.gameObject.GetComponent<Agent>().setIsTarget(true);
            this.gameObject.GetComponent<Agent>().setIsTarget(true);
            StartCoroutine(lineBlabla(tempsDeplacement, a.transform.position,a.gameObject.GetComponent<Agent>()));


        }
        // sinon retransfere la liste
        else
        {
            this.gameObject.GetComponent<Agent>().setAllAgents(avoidAgents);
            avoidAgents.Clear();
            avoidAgents = new List<GameObject>();
        }

    }
    IEnumerator lineBlabla(float time, Vector3 pos, Agent a)
    {
        float elapsedtime = 0;
        float dist = Vector3.Distance(this.transform.position, pos);
        
        line.SetVertexCount(2);
        line.SetPosition(0, this.transform.position);
        line.SetWidth(1, 0);
        float counter = 0;
        float drawsPeed = (time - 2)*10f;
        line.material = GameManager.instance.getLineMat();
        line.SetColors(this.gameObject.GetComponent<Agent>().getColor(), a.getColor());

        /*
        Material whiteDiffuseMat = new Material(Shader.Find("Particles/Additive"));
        
        this.GetComponent<Renderer>().material.color = Color.white;*/
        while (counter<dist)//(elapsedtime < time-2)
        {
            counter += 1.0f/drawsPeed;
            float x = Mathf.Lerp(0, dist, counter);//(elapsedtime/time));
            Vector3 pointAlongLine = x*Vector3.Normalize(pos - this.transform.position) + this.transform.position;
            line.SetPosition(1, pointAlongLine);

            elapsedtime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        line.SetVertexCount(0);

        this.gameObject.GetComponent<Agent>().interaction(a);       // interaction
        a.gameObject.GetComponent<Agent>().setIsTarget(false);       // re-move
        this.gameObject.GetComponent<Agent>().setIsTarget(false);    // re-move
        bla.SetActive(false);

    }

}

