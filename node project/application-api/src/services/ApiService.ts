import axios from 'axios';
import { RequestData } from '../types';

export class ApiService {
  private readonly baseUrl = 'https://script.google.com/macros/s/AKfycbx_iP__JJF8SWK-VnUKz7TVgHZU_aZvqwqOsrqXIlvBwkUgBdEH0lwMxxJZ5g5FQYs-zw/exec';

  async fetchRequestData(requestId: string): Promise<RequestData> {
    const response = await axios.get<RequestData>(this.baseUrl, {
      params: { id: requestId }
    });
    return response.data;
  }
}
