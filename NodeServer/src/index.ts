import uServer from './uWebSocket';

const uwss = new uServer({ port: 3000 });

interface ChatMessage {
  name: string;
  message: string;
}

uwss.on('connection', (socket) => {

  let userName: string;

  socket.on('login', (data) => {
    console.log("Cliente Logado");
    userName = data;
    const chatMessage: ChatMessage = {
      name: "Server Info",
      message: "O usuario " + userName + " acaba de entrar"
    };

    uwss.broadcast("message", chatMessage);
  });

  socket.on('message', (data) => {
    const chatMessage: ChatMessage = {
      name: userName,
      message: data
    };

    uwss.broadcast('message', chatMessage);
  });

});