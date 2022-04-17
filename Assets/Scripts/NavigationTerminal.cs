using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationTerminal : MonoBehaviour
{
    private bool active = true;
    public int ID = 1;
    private float neededpower = 5;
    public int TerminalID = 1;
    public Slider setxSlider;
    public Slider setySlider;

    public List<Engine> EngineswithID;

    public void Start()
    {
        Engine[] AllEngines = transform.parent.GetComponentsInChildren<Engine>();
        for (int i = 0; i < AllEngines.Length; i++)
        {
            if (AllEngines[i].TerminalID == TerminalID)
            {
                EngineswithID.Add(AllEngines[i]);
            }
        }
    }
    void Update()
    {
        if(active)
        {
            for (int i = 0; i < EngineswithID.Count; i++)
            {
                EngineswithID[i].targetthrottle = setxSlider.value;
            }
        }
    }
}
