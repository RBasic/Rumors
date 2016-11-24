using UnityEngine;
using System.Collections;

public class RelaisPassif : Agent
{

    //pas de doute, si doute personne > 0.5 alors +0.1 
    //si doute personne < 0.5 alors -0.1 
    //si doute personne ==0.5 alors +0.0

    public void interaction(Agent a)
    {
        base.interaction(a);
        if (a.getDoute() > 0.5f)
        {
            a.setDoute(0.1f);
        }
        else if (a.getDoute() < 0.5f)
        {
            a.setDoute(-0.1f);
        }
    }
}
