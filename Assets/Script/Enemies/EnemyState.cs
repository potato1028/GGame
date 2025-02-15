using UnityEngine;
using System.Collections;

public class EnemyState : MonoBehaviour {
    public enum State {
        Wandering,
        Chasing,
        Attacking
    }

    public State currentState = State.Wandering;
}