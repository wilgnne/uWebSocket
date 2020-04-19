import WebSocket from 'ws';

interface EventHandler {
    event: string;
    cb(param: WebSocket | any): any;
}

interface MessageHandler {
    e: string;
    data: any;
}

class Socket {
    private socket: WebSocket;
    private events: EventHandler[];

    constructor(socket: WebSocket) {
        this.socket = socket;
        console.log("uWebSocket Server: New Client");
        this.events = [];

        this.socket.on("message", (data) => {
            const message: MessageHandler = JSON.parse(data.toString());
            console.log("uWebSocket Server: Message => ", message);

            console.log("Recorded Events: ", this.events);
            console.log();
            this.events.filter(value => value.event === message.e).map(event => event.cb(message.data));

        });

        this.socket.on("close", (code, reason) => {
            console.log("uWebSocket Server: Connection Closed => ", code, reason);
        });

        this.socket.on("error", (err) => {
            console.log("uWebSocket Server: Connection Error => ", err);
        });

        console.log();
    }

    on(event: string, cb: (data: any) => void): void {
        this.events = [...this.events, { event, cb }];
        console.log("New Socket Event Registered: ", this.events);
    };

    emit(event: string): void;
    emit(event: string, data: any): void;
    emit(event: string, data?: any) {
        if (data) {
            console.log("Há data");
        }
        else {

        }
    }
}

class uServer {
    private wss: WebSocket.Server;
    private sockets: Socket[];
    private events: EventHandler[];

    constructor(options: WebSocket.ServerOptions) {
        this.wss = new WebSocket.Server(options);
        console.log("uWebSocket Server Created: ", this.wss.options);
        this.sockets = [];
        this.events = [];

        this.wss.on('connection', (webSocket) => {
            const socket = new Socket(webSocket);

            socket.onError((err) => {
                if (socket.socket.readyState !== WebSocket.OPEN) {
                    this.sockets.slice(this.sockets.indexOf(socket), 1);
                }
            });

            this.sockets = [...this.sockets, socket];
            this.events.filter(value => value.event === 'connection').map(event => event.cb(socket));
        });
    }

    on(event: string, cb: (socket: Socket) => void) {
        this.events = [...this.events, { event, cb }];
    }

    broadcast(event: string, data?: any) {
        this.sockets.forEach((socket) => {
            const client = socket.socket;
            if (client.readyState === WebSocket.OPEN) {
                socket.emit(event, data);
            }
        });
    }
}

export default uServer;