import uServer from './uWebSocket';

const uwss = new uServer({ port: 3000 });

uwss.on('connection', (socket) => {

  socket.on('login', (data) => {
    console.log(data, " is connected!");
  });

});