import { Request, Response } from 'express';
import { ApiService } from '../services/ApiService';
import { DataParser } from '../services/DataParser';
import { FileService } from '../services/FileService';
import { DatabaseService } from '../services/DatabaseService';
import { SocketService } from '../services/SocketService';
import { ResponseService } from '../services/ResponseService';

export class OrderController {
  private apiService: ApiService;
  private dataParser: DataParser;
  private fileService: FileService;
  private databaseService: DatabaseService;
  private socketService: SocketService;
  private responseService: ResponseService;

  constructor(socketService: SocketService) {
    this.apiService = new ApiService();
    this.dataParser = new DataParser();
    this.fileService = new FileService();
    this.databaseService = new DatabaseService();
    this.socketService = socketService;
    this.responseService = new ResponseService();
  }

  async saveOrder(req: Request, res: Response): Promise<void> {
    try {
      const requestId = req.query.id as string;

      if (!requestId) {
        res.status(400).send(this.responseService.generateErrorHtml('Missing request ID'));
        return;
      }

      console.log(`Processing order for requestId: ${requestId}`);

      // Step 1: Fetch data from external API
      const requestData = await this.apiService.fetchRequestData(requestId);

      // Step 2: Save raw request data to files
      await this.fileService.saveRequestData(requestId, requestData);

      // Step 3: Parse the request data
      const parsedData = this.dataParser.parseRequestData(requestId, requestData);

      // Step 4: Save parsed JSON for reference
      await this.fileService.saveOrderJson(requestId, parsedData);

      // Step 5: Save everything to database in a single transaction
      const order = parsedData.status === 'מאושר' ? {
        requestId,
        vacationDate: parsedData.vacationDate,
        totalRooms: parsedData.totalRooms,
        roomType: parsedData.roomType,
        address: parsedData.address,
        phone: parsedData.phone,
        email: parsedData.email,
        notes: parsedData.notes,
        termsAccepted: parsedData.termsAccepted
      } : null;

      await this.databaseService.saveCompleteOrder(
        requestId,
        JSON.stringify(requestData),
        order,
        parsedData.passengers,
        parsedData.files,
        parsedData.payment || null
      );

      // Step 6: Emit socket event for accepted orders
      if (parsedData.status === 'מאושר') {
        this.socketService.emitNewOrder(requestId);
      }

      // Step 7: Send success response
      res.send(this.responseService.generateSuccessHtml(requestId));

    } catch (error) {
      console.error('Error saving order:', error);
      const errorMessage = error instanceof Error ? error.message : 'Unknown error';
      res.status(500).send(this.responseService.generateErrorHtml(errorMessage));
    }
  }
}
