using UnityEngine;
using System.Collections.Generic;

public class Leader : Agent {

	List<Agent> suiveurs = new List<Agent>();

    //meneur du groupe + ou -0.2 -> + ou - 0.1 aux personnes du groupe

    public void setDoute(float val)
    {
        base.setDoute(val);
        if (base.getDeltadoute() == -0.2f || base.getDeltadoute() == 0.2f)
        {
            foreach (Agent ag in suiveurs)
            {
                ag.setDoute(base.getDeltadoute()/2.0f);
            }
            base.setDeltaDoute();
        }
    }

    public void addSuiveur(Agent a)
    {
        suiveurs.Add(a);
    }
}
