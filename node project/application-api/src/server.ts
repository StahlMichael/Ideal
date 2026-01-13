import express, { Express } from 'express';
import http from 'http';
import { SocketService } from './services/SocketService';
import { OrderController } from './controllers/OrderController';

const app: Express = express();
const server = http.createServer(app);
const PORT = process.env.PORT || 3000;

// Initialize Socket.IO service
const socketService = new SocketService(server);

// Initialize controllers
const orderController = new OrderController(socketService);

// Middleware
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Routes
app.get('/save-order', (req, res) => orderController.saveOrder(req, res));

app.get('/', (_req, res) => {
  res.send('Server is running');
});

// Start server
server.listen(PORT, () => {
  console.log(`Server is running on port ${PORT}`);
  console.log(`Socket.IO is ready for connections`);
});

// Handle unhandled promise rejections
process.on('unhandledRejection', (reason, promise) => {
  console.error('Unhandled Rejection at:', promise, 'reason:', reason);
  // Don't exit the process - keep server running
});

// Handle uncaught exceptions
process.on('uncaughtException', (error) => {
  console.error('Uncaught Exception:', error);
  // Don't exit the process - keep server running
});

export { app, server, socketService };
