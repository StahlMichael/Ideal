import { Server as SocketIOServer } from 'socket.io';
import { Server as HttpServer } from 'http';

export class SocketService {
  private io: SocketIOServer;

  constructor(httpServer: HttpServer) {
    this.io = new SocketIOServer(httpServer, {
      cors: {
        origin: '*',
        methods: ['GET', 'POST']
      }
    });

    this.setupConnectionHandlers();
  }

  private setupConnectionHandlers(): void {
    this.io.on('connection', (socket) => {
      console.log('Client connected:', socket.id);

      socket.on('disconnect', () => {
        console.log('Client disconnected:', socket.id);
      });
    });
  }

  emitNewOrder(requestId: string): void {
    this.io.emit('newOrder', { requestId });
    console.log(`Emitted newOrder event for requestId: ${requestId}`);
  }

  getIO(): SocketIOServer {
    return this.io;
  }
}
