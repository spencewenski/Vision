﻿using UnityEngine;
using System.Collections;

public class CubePush : Cube {

    // NONE: the cube can't be pushed
    // BASIC: projectile's (damped) velocity is added to the cube's velocity
    // AXIS_ALIGNED: the largest component of the projectile's velocity is
    //      projected onto the cube's local coordinate system, and the (damped)
    //      projection is added to the cube's velocity
    // FIXED: the cube is moved in a constant direction and speed
    public enum PushType_e { NONE, BASIC, AXIS_ALIGNED, FIXED };
    public PushType_e pushType;

    public bool useGravity = true;
    public float pushDamp = 1;
    public float axisAlignedDamp = 1;
    public float fixedPushDamp = 1;
    public Vector3 fixedPushDirection;

    public bool _____________;

	//OutlinePulser outline;
    public Rigidbody rigidBody;

    public override void AwakeChild() {
        rigidBody = GetComponent<Rigidbody>();
        // SET CUBE EFFECT IN YOUR AWAKE FUNCTION
        effect = Cube.CubeEffect_e.PUSH;

        if (pushType == PushType_e.FIXED || pushType == PushType_e.AXIS_ALIGNED) {
            rigidBody.freezeRotation = true;
        }
    }

    public override void setActiveChild(bool active_) {
        rigidBody.isKinematic = !active_;
        rigidBody.useGravity = active_ && useGravity;
    }

    // Use this for initialization
    void Start () {
	}

    // OVERWRITE THE Cube.doEffectChild METHOD
    //
    // actually do the effect
    public override void doEffectChild(GameObject other) {

        if (other.tag != "Projectile") {
            return;
        }
        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if (projectile == null) {
            return;
        }
        push(other.transform.position, projectile.getVelocity(), projectile.reverse);
    }

    private void push(Vector3 collisionPosition, Vector3 velocity, bool reverse) {
        if (reverse) {
            velocity = -velocity;
        }
        switch (pushType) {
            case PushType_e.NONE:
                return;
            case PushType_e.BASIC:
                rigidBody.velocity += velocity * pushDamp;
                break;
            case PushType_e.AXIS_ALIGNED:
                pushAxisAligned(collisionPosition, velocity);
                break;
            case PushType_e.FIXED:
                rigidBody.velocity = fixedPushDirection.normalized * velocity.magnitude * fixedPushDamp;
                break;
            default:
                break;
        }
    }

    // only works when rotation is locked
    private void pushAxisAligned(Vector3 collisionPosition, Vector3 velocity) {
        Vector3 velocityAbs = new Vector3(Mathf.Abs(velocity.x), Mathf.Abs(velocity.y), Mathf.Abs(velocity.z));
        Vector3 pushVelocity = Vector3.zero;
        if (velocityAbs.x >= velocityAbs.y && velocityAbs.x >= velocityAbs.z) {
            pushVelocity += Vector3.Project(velocity, transform.right);
        } else if (velocityAbs.y >= velocityAbs.x && velocityAbs.y >= velocityAbs.z) {
            pushVelocity += Vector3.Project(velocity, transform.up);
        } else if (velocityAbs.z >= velocityAbs.x && velocityAbs.z >= velocityAbs.y) {
            pushVelocity += Vector3.Project(velocity, transform.forward);
        }
        rigidBody.velocity = pushVelocity * axisAlignedDamp;
    }
}
