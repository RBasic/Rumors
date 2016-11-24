using UnityEngine;
using System.Collections;

public class InfiniteBorder : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    [SerializeField] private bool top;
    [SerializeField] private bool bottom;
    [SerializeField] private bool left;
    [SerializeField] private bool right;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Agent")
        {
            if(top)
                collider.gameObject.GetComponent<Transform>().position += new Vector3(0,0,-2*5*floor.transform.lossyScale.z+0.1f);  // 5 because the size of the plane is 10*10 square and the middle is 4
            else if (bottom)
                collider.gameObject.GetComponent<Transform>().position += new Vector3(0, 0, 2*5 * floor.transform.lossyScale.z-0.1f);  // 5 because the size of the plane is 10*10 square and the middle is 4
            else if (left)
                collider.gameObject.GetComponent<Transform>().position += new Vector3(2*5 * floor.transform.lossyScale.x - 0.1f, 0,0);  // 5 because the size of the plane is 10*10 square and the middle is 4
            else if (right)
                collider.gameObject.GetComponent<Transform>().position += new Vector3(-2*5 * floor.transform.lossyScale.x+0.1f,0,0);  // 5 because the size of the plane is 10*10 square and the middle is 4

        }
    }
}
