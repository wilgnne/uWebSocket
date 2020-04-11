using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerTest : MonoBehaviour {
    ConnectionController ws;

    void Awake () {
        ws = GetComponent<ConnectionController> ();

        ws.OnConnect ((sender, e) => {
            Debug.Log ("Sending BALUS");
            ws.Emit ("BALUS", "HALUS");
        });
    }

    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
}