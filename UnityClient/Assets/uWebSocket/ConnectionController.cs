using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ConnectionController : MonoBehaviour {
    public static WebSocket ws { get; private set; }
    public string url = "ws://localhost:3000";
    void Awake () {
        ws = new WebSocket (url);

        ws.OnMessage += (sender, e) => {
            Debug.Log ("Laputa says: " + e.Data);
        };
        ws.OnClose += (sender, e) => {
            Debug.LogWarning ("uWebSocket Close: " + e.Reason);
        };
        ws.OnError += (sender, e) => {
            Debug.LogError ("uWebSocket Error: " + e.ToString ());
        };
        ws.OnOpen += (sender, e) => {
            Debug.Log ("uWebSocket Connection Open: " + e.ToString ());
        };
        
        ws.OnOpen += (sender, e) => {
            Debug.Log("Sending BALUS");
            ws.Send("BALUS");
        };

        ws.Connect ();
    }
    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
}