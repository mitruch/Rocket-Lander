﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : MonoBehaviour {

    ParticleSystem flame;

    private void Start()
    {
        flame = GetComponent<ParticleSystem>();
        flame.Play();
    }

    void Update()
    {
        if (Rocket.isLanding || Rocket.isWasted)
        {
            flame.Stop();
        }
    }
}
