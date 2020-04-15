# uWebSocket

Sistema de rede multiplayer baseado em WebSocket para Unity.

## Instalação

Neste momento o uWebSocket não esta disponivel em nenhum gerenciador de pacotes, mas pode ser baixado diretamente pelo GitHub.

## Uso

### Echo server

Um Echo server é uma aplicação que permite a conecxão entre um cliente e um servidor, permitindo que o cliente envie mensagems ao servidor, onde este por sua vez ecoa a mensagem de volta ao cliente.

#### Servidor NodeJS
```typescript
import uServer from './uWebSocket';

const uwss = new uServer({ port: 3000 });

uwss.on('connection', (socket) => {

  socket.on('message', (data) => {
      socket.emit('message', data);
  });

});
```

#### Cliente Unity
```csharp
public class EchoClient : MonoBehaviour {
    ConnectionController ws;

    void Awake() {
        ws = GameObject.FindGameObjectWithTag ("ServerController").
                        GetComponent<ConnectionController> ();
        ws.OnConnect ((sender, e) => {
            Debug.Log ("Connected at server");
            ws.Emit ("message", "Unity say hellow");
        });

        ws.On("message", (data) => {
            Debug.Log("Server echo: " + data);
        });
    }
}
```

## Contributing

Solicitações pull são bem-vindas. Para grandes mudanças, abra um problema primeiro para discutir o que você gostaria de mudar.

## License
[MIT](https://choosealicense.com/licenses/mit/)