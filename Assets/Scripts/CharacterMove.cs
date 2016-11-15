using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour
{

    Collider[] around;
    public float tempsDeplacement = 5;
    public float rayonDetection = 15;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        around = Physics.OverlapSphere(gameObject.transform.position, rayonDetection);
        foreach (Collider c in around)
        {
            if (c.gameObject.name == "pasAuCourant")
            {
                Debug.Log("j'ai " + c.gameObject.name + " a portee");
                StartCoroutine(deplacement(c.transform.position, tempsDeplacement));
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, rayonDetection);
    }
}
