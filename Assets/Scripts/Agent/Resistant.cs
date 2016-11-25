using UnityEngine;
using System.Collections;

public class Resistant : Agent {

    // doute -0.1f

    public void interaction(Agent a)
    {
        base.interaction(a);
        a.setDoute(-0.1f);
    }

    public new void initDoute()
    {
        setDoute(0);
    }

    public new void setDoute(float val)
    {
    }
}
