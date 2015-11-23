﻿using UnityEngine;
using System.Collections;

public class CubeAntiGravity : Cube {

    public bool gravityReversed; // true => gravity is reversed; false => gravity is normal

    public bool _______________;

	OutlinePulser outline;
    public Rigidbody rigidBody;

    public override void AwakeChild() {
        rigidBody = GetComponent<Rigidbody>();
		outline = gameObject.GetComponentInChildren<OutlinePulser> ();
        effect = CubeEffect_e.ANTI_GRAVITY;
    }

    public override void setActiveChild(bool active_) {
        rigidBody.isKinematic = !active_;
        //gravityReversed = !active_;
    }

    void FixedUpdate() {
        if (active  && gravityReversed) {
            rigidBody.AddForce(-Physics.gravity);
        }
    }

    public override void doEffectChild(Collider other) {
        gravityReversed = !gravityReversed;
        rigidBody.useGravity = !gravityReversed;
        outline.OutlinePulseOn = true;
		
    }

}
