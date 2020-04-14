using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatController : MonoBehaviour {
    ConnectionController ws;

    public GameObject loginPanel;
    public InputField nameField;

    public GameObject chatPanel;
    public InputField messageField;

    void Awake () {
        ws = GameObject.FindGameObjectWithTag ("ServerController").GetComponent<ConnectionController> ();

        ws.OnConnect ((sender, e) => {
            Debug.Log ("Connected at server");
            ws.Emit ("login", nameField.text);
            loginPanel.SetActive (false);
            chatPanel.SetActive (true);
        });

        ws.On("message", (data) => {
            Debug.Log(data);
        });
    }

    void Start () {
        loginPanel.SetActive (true);
        chatPanel.SetActive (false);
    }

    public void EntryButton () {
        ws.Connect ();
    }

    public void SendButton () {
        if (!String.IsNullOrEmpty (messageField.text) &&
            !String.IsNullOrWhiteSpace (messageField.text)) {
            ws.Emit("message", messageField.text);
        }
    }
}