using UnityEngine;
using System.Collections;

public class Investigator : Agent {

    //si doute > 0.5 alors +0.1 sinon -0.1

    public override void interaction(Agent a)
    {
        base.interaction(a);
        if(a.getDoute()>0.5f)
            a.setDoute(0.1f);
        else 
            a.setDoute(-0.1f);
    }
}
