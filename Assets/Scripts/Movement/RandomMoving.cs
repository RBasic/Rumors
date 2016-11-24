using UnityEngine;
using System.Collections;

public class RandomMoving : MonoBehaviour
{
    Collider[] around;
    float tempsDeplacement = 5;
    float rayonDetection = 2;

    public float rotationSpeed=0.5f;
    public float movementSpeed = 0.5f;
    public float rotationTime = 0.5f;

    void Start()
    {
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

        foreach (Collider c in around)
        {
            if (c.gameObject.tag=="Agent" && !c.gameObject.GetComponent<Agent>().getIsTarget())
            {
                Debug.Log("j'ai " + c.gameObject.name + " a portee");

                //c.gameObject.GetComponent<Agent>().setIsTarget(true);
                //this.gameObject.GetComponent<Agent>().setIsTarget(true);

                //StartCoroutine(deplacement(c.transform.position, tempsDeplacement));
                //break;
            }
        }
    }

    IEnumerator deplacement(Vector3 pos, float time)
    {
        float elapsedtime = 0;
        Vector3 startingpos = transform.position;
        while (elapsedtime < time)
        {
            transform.position = Vector3.Lerp(startingpos, pos, (elapsedtime / time));
            elapsedtime += Time.deltaTime;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, rayonDetection);
    }
}

