﻿using UnityEngine;
using System.Collections;

public class Apotre : Agent {

    // doute+0.1f

    public void interaction(Agent a)
    {
        base.interaction(a);
        a.setDoute(0.1f);
    }

    public new void initDoute()
    {
        setDoute(1);
    }

    public new void setDoute(float val)
    {
    }
}
