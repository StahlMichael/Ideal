export interface Order {
  requestId: string;
  vacationDate: string;
  totalRooms: number;
  roomType: string;
  address: string;
  phone: string;
  email: string;
  notes: string;
  termsAccepted: boolean;
}

export interface Passenger {
  requestId: string;
  firstName: string;
  lastName: string;
  idNumber: string;
  birthDate: string;
}

export interface File {
  requestId: string;
  fileName: string;
  folderUrl: string;
}

export interface Payment {
  requestId: string;
  token: string;
  approvalNum: string;
  cardName: string;
  validDate: string;
  amount: number;
}

export interface RequestData {
  id: string;
  data: {
    status: string;
    'תאריך חופשה': string;
    'כמות חדרים': string;
    'סוג חדר': string;
    'כתובת': string;
    'טלפון': string;
    'אימייל': string;
    'הערות': string;
    'אישור תקנון': string;
    'קבצים': string;
    JSON: string;
  };
}

export interface ParsedRequestData {
  status: string;
  vacationDate: string;
  totalRooms: number;
  roomType: string;
  address: string;
  phone: string;
  email: string;
  notes: string;
  termsAccepted: boolean;
  passengers: Passenger[];
  files: File[];
  payment?: Payment;
}
