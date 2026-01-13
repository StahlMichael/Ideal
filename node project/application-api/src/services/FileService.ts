import fs from 'fs';
import path from 'path';

export class FileService {
  private readonly dataDir: string;
  private readonly jsonDir: string;

  constructor() {
    this.dataDir = path.join(__dirname, '../../data');
    this.jsonDir = path.join(__dirname, '../../json');
    this.ensureDirectoriesExist();
  }

  private ensureDirectoriesExist(): void {
    if (!fs.existsSync(this.dataDir)) {
      fs.mkdirSync(this.dataDir, { recursive: true });
    }
    if (!fs.existsSync(this.jsonDir)) {
      fs.mkdirSync(this.jsonDir, { recursive: true });
    }
  }

  async saveRequestData(requestId: string, data: any): Promise<void> {
    const dataFilePath = path.join(this.dataDir, `${requestId}.json`);
    await fs.promises.writeFile(dataFilePath, JSON.stringify(data, null, 2));
  }

  async saveOrderJson(requestId: string, orderData: any): Promise<void> {
    const jsonFilePath = path.join(this.jsonDir, `${requestId}.json`);
    await fs.promises.writeFile(jsonFilePath, JSON.stringify(orderData, null, 2));
  }
}
