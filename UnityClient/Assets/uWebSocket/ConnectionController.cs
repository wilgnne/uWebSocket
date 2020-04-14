using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;

struct MessageHandler<T> {
    public string e;
    public T data;

    public MessageHandler (string _e, T _data) {
        e = _e;
        data = _data;
    }
}

public class ConnectionController : MonoBehaviour {
    WebSocket ws;
    public string url = "ws://localhost:3000";
    public bool connectOnStart = true;
    bool connected = false;
    Dictionary<string, List<EventHandler<MessageEventArgs>>> messegesCallback;

    void Awake () {
        messegesCallback = new Dictionary<string, List<EventHandler<MessageEventArgs>>> ();

        ws = new WebSocket (url);

        ws.OnMessage += (sender, e) => {
            Debug.Log ("Laputa says: " + e.Data);
        };
        ws.OnClose += (sender, e) => {
            Debug.LogWarning ("uWebSocket Close: " + e.Reason);
            connected = false;
        };
        ws.OnError += (sender, e) => {
            Debug.LogError ("uWebSocket Error: " + e.Exception + " message:" + e.Message);
        };
        ws.OnOpen += (sender, e) => {
            Debug.Log ("uWebSocket Connection Open");
        };
    }

    void Start () {
        if (connectOnStart)
            Connect();
    }

    public void Connect ()
    {
        if (!connected){
            ws.Connect();
            connected = true;
        }
    }

    public void Emit<T> (string e, T data) {
        MessageHandler<T> emit = new MessageHandler<T> (e, data);
        ws.Send (JsonConvert.SerializeObject (emit));
    }

    public void Emit (string e) {
        MessageHandler<string> emit = new MessageHandler<string> (e, "");
        ws.Send (JsonConvert.SerializeObject (emit));
    }

    public void On (string e, EventHandler<MessageEventArgs> cb) {
        if (messegesCallback.ContainsKey (e)) {
            messegesCallback[e].Add (cb);
            return;
        }
        messegesCallback.Add (e, new List<EventHandler<MessageEventArgs>> {
            cb
        });
    }

    public void OnConnect (EventHandler cb) {
        ws.OnOpen += cb;
    }

    public void OnDiconect (EventHandler<CloseEventArgs> cb) {
        ws.OnClose += cb;
    }
}