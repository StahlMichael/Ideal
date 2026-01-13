import sql from 'mssql';
import { Order, Passenger, File, Payment } from '../types';

export class DatabaseService {
  private static pool: sql.ConnectionPool | null = null;
  private config: sql.config = {
    user: 'sa',
    password: 'Welcome1',
    server: 'localhost',
    database: 'SKI',
    options: {
      encrypt: true,
      trustServerCertificate: true
    },
    pool: {
      max: 10,
      min: 0,
      idleTimeoutMillis: 30000
    }
  };

  private async getPool(): Promise<sql.ConnectionPool> {
    if (!DatabaseService.pool) {
      DatabaseService.pool = await sql.connect(this.config);
    }
    return DatabaseService.pool;
  }

  async saveCompleteOrder(
    requestId: string,
    requestData: string,
    order: Order | null,
    passengers: Passenger[],
    files: File[],
    payment: Payment | null
  ): Promise<void> {
    const pool = await this.getPool();
    const transaction = new sql.Transaction(pool);
    
    try {
      await transaction.begin();

      // Insert request log
      await transaction.request()
        .input('RequestId', sql.NVarChar, requestId)
        .input('RequestData', sql.NVarChar, requestData)
        .query(`
          INSERT INTO tblRequestLog (RequestId, RequestData, RequestDateTime)
          VALUES (@RequestId, @RequestData, GETDATE())
        `);

      // If order is accepted, insert all data
      if (order) {
        // Delete existing order data (CASCADE will handle related records)
        await transaction.request()
          .input('RequestId', sql.NVarChar, requestId)
          .query('DELETE FROM tblOrders WHERE RequestId = @RequestId');

        // Insert order
        await transaction.request()
          .input('RequestId', sql.NVarChar, order.requestId)
          .input('VacationDate', sql.Date, order.vacationDate)
          .input('TotalRooms', sql.Int, order.totalRooms)
          .input('RoomType', sql.NVarChar, order.roomType)
          .input('Address', sql.NVarChar, order.address)
          .input('Phone', sql.NVarChar, order.phone)
          .input('Email', sql.NVarChar, order.email)
          .input('Notes', sql.NVarChar, order.notes)
          .input('TermsAccepted', sql.Bit, order.termsAccepted)
          .query(`
            INSERT INTO tblOrders 
            (RequestId, VacationDate, TotalRooms, RoomType, Address, Phone, Email, Notes, TermsAccepted, SubmissionDate)
            VALUES 
            (@RequestId, @VacationDate, @TotalRooms, @RoomType, @Address, @Phone, @Email, @Notes, @TermsAccepted, GETDATE())
          `);

        // Batch insert passengers
        if (passengers.length > 0) {
          const passengerValues = passengers.map((_p, idx) => 
            `(@RequestId, @FirstName${idx}, @LastName${idx}, @IdNumber${idx}, @BirthDate${idx})`
          ).join(',');

          const request = transaction.request();
          request.input('RequestId', sql.NVarChar, requestId);
          
          passengers.forEach((p, idx) => {
            request.input(`FirstName${idx}`, sql.NVarChar, p.firstName);
            request.input(`LastName${idx}`, sql.NVarChar, p.lastName);
            request.input(`IdNumber${idx}`, sql.NVarChar, p.idNumber);
            request.input(`BirthDate${idx}`, sql.Date, p.birthDate);
          });

          await request.query(`
            INSERT INTO tblPax (RequestId, FirstName, LastName, IdNumber, BirthDate)
            VALUES ${passengerValues}
          `);
        }

        // Batch insert files
        if (files.length > 0) {
          const fileValues = files.map((_f, idx) => 
            `(@RequestId, @FileName${idx}, @FolderUrl${idx})`
          ).join(',');

          const request = transaction.request();
          request.input('RequestId', sql.NVarChar, requestId);
          
          files.forEach((f, idx) => {
            request.input(`FileName${idx}`, sql.NVarChar, f.fileName);
            request.input(`FolderUrl${idx}`, sql.NVarChar, f.folderUrl);
          });

          await request.query(`
            INSERT INTO tblFiles (RequestId, FileName, FolderUrl)
            VALUES ${fileValues}
          `);
        }

        // Insert payment
        if (payment) {
          await transaction.request()
            .input('RequestId', sql.NVarChar, payment.requestId)
            .input('Token', sql.NVarChar, payment.token)
            .input('ApprovalNum', sql.NVarChar, payment.approvalNum)
            .input('CardName', sql.NVarChar, payment.cardName)
            .input('ValidDate', sql.NVarChar, payment.validDate)
            .input('Amount', sql.Decimal(10, 2), payment.amount)
            .query(`
              INSERT INTO tblCC_Payment 
              (RequestId, Token, ApprovalNum, CardName, ValidDate, Amount, PaymentDateTime)
              VALUES 
              (@RequestId, @Token, @ApprovalNum, @CardName, @ValidDate, @Amount, GETDATE())
            `);
        }
      }

      await transaction.commit();
    } catch (error) {
      await transaction.rollback();
      throw error;
    }
  }
}
