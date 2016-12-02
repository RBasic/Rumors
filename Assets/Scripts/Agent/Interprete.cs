using UnityEngine;
using System.Collections;

public class Interprete : Agent
{

    //si interaction avec investigateur ayant doute opposé alors + ou - 0.1

    public override void interaction(Agent a)
    {
        base.interaction(a);
        if (base.getDoute() > 0.5f && a.getDoute() < 0.5f)
        {
            a.setDoute(0.1f);
        }
        else if (base.getDoute() < 0.5 && a.getDoute() > 0.5f)
        {
            a.setDoute(-0.1f);
        }
    }
}
