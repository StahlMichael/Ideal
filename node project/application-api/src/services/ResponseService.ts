export class ResponseService {
  generateSuccessHtml(requestId: string): string {
    return `
      <!DOCTYPE html>
      <html dir="rtl" lang="he">
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <title>הבקשה נשמרה בהצלחה</title>
          <style>
            body {
              font-family: Arial, sans-serif;
              background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
              display: flex;
              justify-content: center;
              align-items: center;
              height: 100vh;
              margin: 0;
            }
            .container {
              background: white;
              padding: 40px;
              border-radius: 10px;
              box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
              text-align: center;
              max-width: 500px;
            }
            h1 {
              color: #667eea;
              margin-bottom: 20px;
            }
            p {
              font-size: 18px;
              color: #333;
              margin: 10px 0;
            }
            .request-id {
              font-size: 24px;
              font-weight: bold;
              color: #764ba2;
              margin: 20px 0;
            }
          </style>
        </head>
        <body>
          <div class="container">
            <h1>✅ הבקשה נשמרה בהצלחה!</h1>
            <p>מספר בקשה:</p>
            <div class="request-id">${requestId}</div>
            <p>הבקשה שלך נשמרה במערכת ותטופל בהקדם.</p>
          </div>
        </body>
      </html>
    `;
  }

  generateErrorHtml(error: string): string {
    return `
      <!DOCTYPE html>
      <html dir="rtl" lang="he">
        <head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <title>שגיאה</title>
          <style>
            body {
              font-family: Arial, sans-serif;
              background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%);
              display: flex;
              justify-content: center;
              align-items: center;
              height: 100vh;
              margin: 0;
            }
            .container {
              background: white;
              padding: 40px;
              border-radius: 10px;
              box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
              text-align: center;
              max-width: 500px;
            }
            h1 {
              color: #f5576c;
              margin-bottom: 20px;
            }
            p {
              font-size: 18px;
              color: #333;
              margin: 10px 0;
            }
            .error-message {
              font-size: 16px;
              color: #666;
              margin: 20px 0;
              padding: 15px;
              background: #ffe6e6;
              border-radius: 5px;
            }
          </style>
        </head>
        <body>
          <div class="container">
            <h1>❌ שגיאה</h1>
            <p>אירעה שגיאה בעת שמירת הבקשה.</p>
            <div class="error-message">${error}</div>
            <p>אנא נסה שוב או פנה לתמיכה.</p>
          </div>
        </body>
      </html>
    `;
  }
}
