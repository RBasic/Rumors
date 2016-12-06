using UnityEngine;
using System.Collections;

public class Resistant : Agent {

    // doute -0.1f

    public override void interaction(Agent a)
    {
        base.interaction(a);
        a.setDoute(-0.1f);
    }

    public override void initDoute()
    {
        base.setDoute(0);
    }

    public override void setDoute(float val)
    {
    }
}
