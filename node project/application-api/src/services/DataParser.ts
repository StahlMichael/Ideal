import { Payment, Passenger, File, ParsedRequestData, RequestData } from '../types';

export class DataParser {
  parseRequestData(requestId: string, data: RequestData): ParsedRequestData {
    const jsonData = JSON.parse(data.data.JSON);
    const filesData = data.data['קבצים'] ? JSON.parse(data.data['קבצים']) : null;

    return {
      status: data.data.status,
      vacationDate: data.data['תאריך חופשה'],
      totalRooms: parseInt(data.data['כמות חדרים']),
      roomType: data.data['סוג חדר'],
      address: data.data['כתובת'],
      phone: data.data['טלפון'],
      email: data.data['אימייל'],
      notes: data.data['הערות'] || '',
      termsAccepted: data.data['אישור תקנון'] === 'כן',
      passengers: this.parsePassengers(requestId, jsonData),
      files: this.parseFiles(requestId, jsonData),
      payment: filesData ? this.parsePayment(requestId, filesData) : undefined
    };
  }

  private parsePassengers(requestId: string, jsonData: any): Passenger[] {
    const passengers: Passenger[] = [];
    
    for (let i = 1; i <= 10; i++) {
      const firstName = jsonData[`שם פרטי נוסע ${i}`];
      const lastName = jsonData[`שם משפחה נוסע ${i}`];
      const idNumber = jsonData[`ת.ז. נוסע ${i}`];
      const birthDate = jsonData[`תאריך לידה נוסע ${i}`];

      if (firstName && lastName) {
        passengers.push({
          requestId,
          firstName,
          lastName,
          idNumber: idNumber || '',
          birthDate: birthDate || ''
        });
      }
    }

    return passengers;
  }

  private parseFiles(requestId: string, jsonData: any): File[] {
    const files: File[] = [];
    
    for (let i = 1; i <= 10; i++) {
      const fileName = jsonData[`filename${i}`];
      const folderUrl = jsonData.folderUrl;

      if (fileName) {
        files.push({
          requestId,
          fileName,
          folderUrl: folderUrl || ''
        });
      }
    }

    return files;
  }

  private parsePayment(requestId: string, paymentData: any): Payment {
    const cardTypeMap: { [key: string]: string } = {
      '1': 'Isracard',
      '2': 'Visa Shekel',
      '3': 'Diners Shekel',
      '4': 'AX Shekel',
      '5': 'Eurocard',
      '6': 'Visa Alpha'
    };

    return {
      requestId,
      token: paymentData.token || '',
      approvalNum: paymentData.approvalNum || '',
      cardName: cardTypeMap[paymentData.cardType] || 'Unknown',
      validDate: paymentData.validDate || '',
      amount: parseFloat(paymentData.amount) || 0
    };
  }
}
