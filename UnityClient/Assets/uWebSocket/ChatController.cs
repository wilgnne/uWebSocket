using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

struct ChatMessage {
    public string name;
    public string message;

    public ChatMessage (string _name, string _message) {
        name = _name;
        message = _message;
    }
}

public class ChatController : MonoBehaviour {
    ConnectionController ws;

    string nickname;

    public GameObject loginPanel;
    public InputField nameField;

    public GameObject chatPanel;
    public InputField messageField;

    public Transform content;
    public GameObject messageFrame;

    void Awake () {
        ws = GameObject.FindGameObjectWithTag ("ServerController").
        GetComponent<ConnectionController> ();

        ws.OnConnect ((sender, e) => {
            Debug.Log ("Connected at server");
            ws.Emit ("login", nickname);
            loginPanel.SetActive (false);
            chatPanel.SetActive (true);
        });

        ws.On ("message", (data) => {
            ChatMessage chatMessage = JsonConvert.DeserializeObject<ChatMessage> (data);

            Vector3 framePos = new Vector3 (content.position.x,
                content.position.y,
                content.position.z);

            GameObject goFrame = Instantiate (messageFrame,
                framePos,
                Quaternion.identity)
            as GameObject;

            goFrame.transform.SetParent (content);

            MessageFrame mf = goFrame.GetComponent<MessageFrame> ();
            mf.nameFrame.text = chatMessage.name;
            mf.messageFrame.text = chatMessage.message;

            messageField.text = "";
        });
    }

    void Start () {
        loginPanel.SetActive (true);
        chatPanel.SetActive (false);
    }

    public void EntryButton () {
        nickname = nameField.text;
        ws.Connect ();
    }

    public void SendButton () {
        Debug.Log ("Enviar mensagem: " + messageField.text);
        if (!String.IsNullOrEmpty (messageField.text) &&
            !String.IsNullOrWhiteSpace (messageField.text)) {
            ws.Emit ("message", messageField.text);
        }
    }
}