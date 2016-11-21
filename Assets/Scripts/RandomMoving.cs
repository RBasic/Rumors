using UnityEngine;
using System.Collections;

//https://unity3d.com/learn/tutorials/projects/survival-shooter/more-enemies
public class RandomMoving : MonoBehaviour
{

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

        transform.Rotate(new Vector3( 0,rotationSpeed * Time.deltaTime,0));
        transform.position += transform.forward * movementSpeed * Time.deltaTime;

    }
}



/*
public class RandomMoving : MonoBehaviour
{

    [SerializeField] private Transform stuff;// Needs rigidbody attached with a collider
    Vector3 vel; // Holds the random velocity
    float switchDirection = 3;
    float curTime = 0;
 
    void Start()
    {
        SetVel();
    }

    void SetVel()
    {
        if (Random.value > .5)
        {
            vel.x = 10 * 10 * Random.value;
        }
        else {
            vel.x = -10 * 10 * Random.value;
        }
        if (Random.value > .5)
        {
            vel.z = 10 * 10 * Random.value;
        }
        else {
            vel.z = -10 * 10 * Random.value;
        }
    }

    void Update()
    {
        if (curTime < switchDirection)
        {
            curTime += 1 * Time.deltaTime;
        }
        else {
            SetVel();
            if (Random.value > .5)
            {
                switchDirection += Random.value;
            }
            else
            {
                switchDirection -= Random.value;
            }
            if (switchDirection < 1)
                {
                    switchDirection = 1 + Random.value;
                }
                curTime = 0;
            }
        }

        void FixedUpdate()
    {
            stuff.GetComponent<Rigidbody>().velocity = vel;
        }
    }
    */
