using UnityEngine;
using System.Collections;

public class Apotre : Agent {

    // doute+0.1f

    public override void interaction(Agent a)
    {
        base.interaction(a);
        a.setDoute(0.1f);
        Debug.Log("ixi");

    }

    public override void initDoute()
    {
        base.setDoute(1.0f);

    }

    public new void setDoute(float val)
    {
    }
}
