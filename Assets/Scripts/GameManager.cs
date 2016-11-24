using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private GameObject panelInfo;
    private GameObject currentAgent;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _instance = this;

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
    }

    private void updatePanelInfo()
    {
        if (currentAgent != null)
        {
            panelInfo.SetActive(true);
        }
        else
        {
            panelInfo.SetActive(false);
        }
    }

}
