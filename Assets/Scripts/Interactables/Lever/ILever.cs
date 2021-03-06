﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILever { 

    bool Activate(string leverPulled);
    bool Deactivate(string leverPulled);
    void PullDown(string lever);
    void PullUp(string lever);

}
