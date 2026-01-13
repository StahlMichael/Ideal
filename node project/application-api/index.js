const express = require('express');
const fs = require('fs');
const path = require('path');
const axios = require('axios');
const sql = require('mssql');
const http = require('http');
const socketIo = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIo(server, {
  cors: {
    origin: "*",
    methods: ["GET", "POST"]
  }
});

const PORT = 3000;
const BASE_SCRIPT_URL = "https://script.google.com/macros/s/AKfycbwU-kBl2qAxIUVX4tp3sCNvY9cCuZGC-ozdliKR3mEsWRECHcLaK2CkCcgIoM2Nr1M5Rg/exec";

const sqlConfig = {
  user: 'Michael',
  password: '2585',
  server: 'SQL-SERVER',
  database: 'SKI',
  options: {
    encrypt: false,
    trustServerCertificate: true,
    instanceName: 'GILBOA'
  }
};

app.use(express.json());

// Endpoint to receive id and status, call external endpoint, and save response
app.get('/save-order', async (req, res) => {
  const requestTime = new Date().toLocaleString('he-IL', { timeZone: 'Asia/Jerusalem' });
  console.log(`[${requestTime}] Request received: /save-order`);
  
  const { id, status, user } = req.query;
  if (!id || !status || !['accepted', 'denied'].includes(status)) {
    return res.status(400).json({ error: 'Missing or invalid id/status' });
  }

  try {
    // Simulate external API call (replace with real URL if needed)
    console.log("url: ",`${BASE_SCRIPT_URL}?id=${id}`)
    const response = await axios.get(`${BASE_SCRIPT_URL}?id=${id}`);
    const data = response.data;
    // For now, mock the response:
    console.log(data)
    // Save to JSON file in /data
    const saveObj = { id, status, data };
    const filePath = path.join(__dirname, 'data', `${id}.json`);
    fs.writeFileSync(filePath, JSON.stringify(saveObj, null, 2));

    // Parse JSON field if it exists
    let parsedData = null;
    if (data.data && data.data.JSON) {
      try {
        parsedData = JSON.parse(data.data.JSON);
        
        // Save the parsed JSON to /json folder
        const jsonDir = path.join(__dirname, 'json');
        if (!fs.existsSync(jsonDir)) {
          fs.mkdirSync(jsonDir, { recursive: true });
        }
        const jsonFilePath = path.join(jsonDir, `${id}.json`);
        fs.writeFileSync(jsonFilePath, JSON.stringify(parsedData, null, 2));
      } catch (e) {
        // If parsing fails, continue without parsed data
      }
    }

    // Extract key details
    const txnId = data.data?.['פרטים'] || id;
    const creationDate = data.data?.['תאריך יצירה'] ? new Date(data.data['תאריך יצירה']).toLocaleString('he-IL') : '';
    const paymentStatus = data.data?.['סטטוס תשלום'] || '';
    const vacationDate = parsedData?.vacationDate || '';
    const contactEmail = parsedData?.contactInfo?.email || '';
    const contactPhone = parsedData?.contactInfo?.phone || '';
    const passengersCount = parsedData?.passengers?.length || 0;

    // Save to SQL Server
    let pool = null;
    try {
      pool = await sql.connect(sqlConfig);
      
      // Log all incoming requests to tblRequestLog
      const now = new Date();
      const pad = (n) => n.toString().padStart(2, '0');
      const requestDateTime = `${pad(now.getDate())}/${pad(now.getMonth() + 1)}/${now.getFullYear()} ${pad(now.getHours())}:${pad(now.getMinutes())}:${pad(now.getSeconds())}`;
      
      await pool.request()
        .input('RequestId', sql.NVarChar, id)
        .input('Status', sql.NVarChar, status)
        .input('RequestDateTime', sql.NVarChar, requestDateTime)
        .input('User', sql.NVarChar, user || 'Unknown')
        .query(`
          INSERT INTO tblRequestLog (RequestId, Status, RequestDateTime, [User])
          VALUES (@RequestId, @Status, @RequestDateTime, @User)
        `);
      console.log(`[${new Date().toLocaleString('he-IL')}] Request logged to tblRequestLog`);

      if (status === 'accepted' && parsedData) {
        // Delete previous data if this RequestId already exists
        await pool.request()
          .input('RequestId', sql.NVarChar, id)
          .query('DELETE FROM tblOrders WHERE RequestId = @RequestId');
        console.log(`[${new Date().toLocaleString('he-IL')}] Deleted previous data for RequestId: ${id}`);

        // Insert into tblOrders
        const now = new Date();
        const pad = (n) => n.toString().padStart(2, '0');
        const submissionDateTime = `${pad(now.getDate())}/${pad(now.getMonth() + 1)}/${now.getFullYear()} ${pad(now.getHours())}:${pad(now.getMinutes())}:${pad(now.getSeconds())}`;
        console.log(`[${new Date().toLocaleString('he-IL')}] Attempting to save order to tblOrders...`);
        
        await pool.request()
          .input('RequestId', sql.NVarChar, id)
          .input('vacationDate', sql.NVarChar, parsedData.vacationDate)
          .input('totalRooms', sql.Int, parsedData.rooms?.totalRooms)
          .input('roomType', sql.NVarChar, parsedData.rooms?.roomType)
          .input('deluxeFor', sql.NVarChar, parsedData.rooms?.deluxeFor)
          .input('deluxeRoomsCount', sql.Int, parsedData.rooms?.deluxeRoomsCount)
          .input('address', sql.NVarChar, parsedData.contactInfo?.address)
          .input('phone', sql.NVarChar, parsedData.contactInfo?.phone)
          .input('email', sql.NVarChar, parsedData.contactInfo?.email)
          .input('notes', sql.NVarChar, parsedData.contactInfo?.notes)
          .input('termsAccepted', sql.Bit, parsedData.termsAccepted)
          .input('submissionDate', sql.NVarChar, submissionDateTime)
          .query(`
            INSERT INTO tblOrders (
              RequestId, vacationDate, totalRooms, roomType, deluxeFor, deluxeRoomsCount,
              address, phone, email, notes, termsAccepted, submissionDate
            )
            VALUES (
              @RequestId, @vacationDate, @totalRooms, @roomType, @deluxeFor, @deluxeRoomsCount,
              @address, @phone, @email, @notes, @termsAccepted, @submissionDate
            )
          `);

        console.log(`[${new Date().toLocaleString('he-IL')}] Order saved to tblOrders with RequestId: ${id}`);

        // Broadcast new order to connected clients
        io.emit('newOrder', {
          requestId: id,
          email: contactEmail,
          phone: contactPhone,
          vacationDate: vacationDate,
          totalRooms: parsedData.rooms?.totalRooms,
          passengersCount: passengersCount,
          submissionDate: submissionDateTime
        });
        console.log(`[${new Date().toLocaleString('he-IL')}] Order notification sent to connected clients`);

        // Insert passengers into tblPax
        if (parsedData.passengers && parsedData.passengers.length > 0) {
          for (const pax of parsedData.passengers) {
            const paxRequest = pool.request()
              .input('RequestId', sql.NVarChar, id)
              .input('tempId', sql.NVarChar, pax.tempId)
              .input('lastName', sql.NVarChar, pax.lastName)
              .input('firstName', sql.NVarChar, pax.firstName)
              .input('birthDate', sql.Date, pax.birthDate)
              .input('passportNumber', sql.NVarChar, pax.passportNumber)
              .input('gender', sql.NVarChar, pax.gender)
              .input('noFlight', sql.Bit, pax.noFlight)
              .input('skiPass', sql.Bit, pax.skiPass)
              .input('skiLevel', sql.NVarChar, pax.skiLevel)
              .input('hasNumber', sql.Bit, pax.frequentFlyer?.hasNumber)
              .input('frequentFlyerNumber', sql.NVarChar, pax.frequentFlyer?.number);

            await paxRequest.query(`
              INSERT INTO tblPax (
                RequestId, tempId, lastName, firstName, birthDate, passportNumber,
                gender, noFlight, skiPass, skiLevel, hasNumber, frequentFlyerNumber
              )
              VALUES (
                @RequestId, @tempId, @lastName, @firstName, @birthDate, @passportNumber,
                @gender, @noFlight, @skiPass, @skiLevel, @hasNumber, @frequentFlyerNumber
              )
            `);
          }
          console.log(`[${new Date().toLocaleString('he-IL')}] ${parsedData.passengers.length} passengers saved to tblPax`);
        }

        // Insert files into tblFiles
        if (parsedData.files && parsedData.files.length > 0) {
          // Extract the folder URL from data.data[""]
          const folderUrl = data.data?.[''] || null;
          
          for (const file of parsedData.files) {
            const fileRequest = pool.request()
              .input('RequestId', sql.NVarChar, id)
              .input('fileName', sql.NVarChar, file.name)
              .input('fileType', sql.NVarChar, file.type)
              .input('fileSize', sql.Int, file.size)
              .input('folderUrl', sql.NVarChar(500), folderUrl);

            await fileRequest.query(`
              INSERT INTO tblFiles (
                RequestId, fileName, fileType, fileSize, folderUrl
              )
              VALUES (
                @RequestId, @fileName, @fileType, @fileSize, @folderUrl
              )
            `);
          }
          console.log(`[${new Date().toLocaleString('he-IL')}] ${parsedData.files.length} files saved to tblFiles`);
        }

        // Insert credit card payment info into tblCC_Payment
        if (data.data && data.data['קבצים']) {
          try {
            const paymentData = JSON.parse(data.data['קבצים']);
            
            // Only insert if we have a valid token (payment was made)
            if (paymentData.token && paymentData.token !== '') {
              const cardTypeMapping = {
                '1': 'Isracard/Mastercard',
                '2': 'Visa',
                '3': 'American Express',
                '4': 'Diners Club',
                '5': 'JCB',
                '6': 'Discover'
              };
              
              const cardName = cardTypeMapping[paymentData.cardType] || 'Unknown';
              const amount = paymentData.amount ? parseFloat(paymentData.amount) : null;
              const paymentDateTime = paymentData.timestamp ? new Date(paymentData.timestamp) : null;
              
              const ccRequest = pool.request()
                .input('RequestId', sql.NVarChar, id)
                .input('token', sql.VarChar(20), paymentData.token || null)
                .input('approveNum', sql.VarChar(20), paymentData.approveNum || null)
                .input('customerId', sql.VarChar(20), paymentData.customerId || null)
                .input('validDate', sql.VarChar(4), paymentData.validDate || null)
                .input('cardType', sql.Int, parseInt(paymentData.cardType) || null)
                .input('cardName', sql.VarChar(20), cardName)
                .input('amount', sql.Decimal(18, 2), amount)
                .input('paymentDateTime', sql.DateTime, paymentDateTime);

              await ccRequest.query(`
                INSERT INTO tblCC_Payment (
                  RequestId, token, approveNum, customerId, validDate, cardType, cardName, amount, paymentDateTime
                )
                VALUES (
                  @RequestId, @token, @approveNum, @customerId, @validDate, @cardType, @cardName, @amount, @paymentDateTime
                )
              `);
              console.log(`[${new Date().toLocaleString('he-IL')}] Credit card payment saved to tblCC_Payment (Token: ${paymentData.token}, Amount: ${amount}, DateTime: ${paymentDateTime})`);
            }
          } catch (paymentErr) {
            console.error(`[${new Date().toLocaleString('he-IL')}] Error parsing payment data:`, paymentErr.message);
            // Continue even if payment parsing fails
          }
        }
      }
    } catch (sqlErr) {
      console.error(`[${new Date().toLocaleString('he-IL')}] SQL Error:`, sqlErr.message);
      // Continue even if SQL fails - data is still saved to JSON
    } finally {
      if (pool) await pool.close();
    }

    // Emit Socket.IO event for all requests (accepted and denied)
    io.emit('newOrder', {
      requestId: id,
      status: status,
      email: contactEmail,
      phone: contactPhone,
      vacationDate: vacationDate,
      user: user || 'Unknown'
    });
    console.log(`[${new Date().toLocaleString('he-IL')}] Order notification sent to connected clients (Status: ${status})`);

    // Return beautiful HTML response
    const html = `
<!DOCTYPE html>
<html dir="rtl" lang="he">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>אישור פעולה</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }
        .container {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
            max-width: 600px;
            width: 100%;
            padding: 40px;
            animation: slideIn 0.5s ease-out;
        }
        @keyframes slideIn {
            from {
                opacity: 0;
                transform: translateY(-30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
        .success-icon {
            width: 80px;
            height: 80px;
            margin: 0 auto 20px;
            background: ${status === 'accepted' ? '#4CAF50' : '#FF5722'};
            border-radius: 50%;
            display: flex;
            justify-content: center;
            align-items: center;
            font-size: 40px;
            color: white;
            animation: scaleIn 0.6s ease-out;
        }
        @keyframes scaleIn {
            from {
                transform: scale(0);
            }
            to {
                transform: scale(1);
            }
        }
        h1 {
            text-align: center;
            color: #333;
            margin-bottom: 10px;
            font-size: 28px;
        }
        .status-badge {
            display: inline-block;
            padding: 8px 20px;
            border-radius: 20px;
            font-weight: bold;
            font-size: 14px;
            margin: 0 auto 30px;
            display: block;
            width: fit-content;
            background: ${status === 'accepted' ? '#4CAF50' : '#FF5722'};
            color: white;
        }
        .details {
            background: #f8f9fa;
            border-radius: 10px;
            padding: 20px;
            margin-top: 20px;
        }
        .detail-row {
            display: flex;
            justify-content: space-between;
            padding: 12px 0;
            border-bottom: 1px solid #e0e0e0;
        }
        .detail-row:last-child {
            border-bottom: none;
        }
        .detail-label {
            font-weight: 600;
            color: #555;
        }
        .detail-value {
            color: #333;
            text-align: left;
        }
        .footer {
            text-align: center;
            margin-top: 30px;
            color: #777;
            font-size: 14px;
        }
    </style>
</head>
<body>
    <div class="container">
        <div class="success-icon">${status === 'accepted' ? '✓' : '✗'}</div>
        <h1>הפעולה בוצעה בהצלחה!</h1>
        <span class="status-badge">סטטוס: ${status === 'accepted' ? 'אושר' : 'נדחה'}</span>
        
        <div class="details">
            <div class="detail-row">
                <span class="detail-label">מספר עסקה:</span>
                <span class="detail-value">${txnId}</span>
            </div>
            ${creationDate ? `
            <div class="detail-row">
                <span class="detail-label">תאריך יצירה:</span>
                <span class="detail-value">${creationDate}</span>
            </div>` : ''}
            ${vacationDate ? `
            <div class="detail-row">
                <span class="detail-label">תאריכי חופשה:</span>
                <span class="detail-value">${vacationDate}</span>
            </div>` : ''}
            ${passengersCount > 0 ? `
            <div class="detail-row">
                <span class="detail-label">מספר נוסעים:</span>
                <span class="detail-value">${passengersCount}</span>
            </div>` : ''}
            ${paymentStatus ? `
            <div class="detail-row">
                <span class="detail-label">סטטוס תשלום:</span>
                <span class="detail-value">${paymentStatus === 'paid' ? 'שולם' : paymentStatus}</span>
            </div>` : ''}
            ${contactEmail ? `
            <div class="detail-row">
                <span class="detail-label">אימייל:</span>
                <span class="detail-value">${contactEmail}</span>
            </div>` : ''}
            ${contactPhone ? `
            <div class="detail-row">
                <span class="detail-label">טלפון:</span>
                <span class="detail-value">${contactPhone}</span>
            </div>` : ''}
        </div>
        
        <div class="footer">
            הנתונים נשמרו במערכת בהצלחה
        </div>
    </div>
</body>
</html>
    `;

    const completeTime = new Date().toLocaleString('he-IL', { timeZone: 'Asia/Jerusalem' });
    console.log(`[${completeTime}] Request completed successfully: ID=${id}, Status=${status}`);
    res.send(html);
  } catch (err) {
    const errorTime = new Date().toLocaleString('he-IL', { timeZone: 'Asia/Jerusalem' });
    console.error(`[${errorTime}] Error processing request:`, err.message);
    res.status(500).json({ error: 'Failed to process request', details: err.message });
  }
});

app.get('/', (req, res) => {
  res.send('Hello World!');
});

// Socket.IO connection handler
io.on('connection', (socket) => {
  const connectTime = new Date().toLocaleString('he-IL', { timeZone: 'Asia/Jerusalem' });
  console.log(`[${connectTime}] WinForms client connected: ${socket.id}`);
  
  socket.on('disconnect', () => {
    const disconnectTime = new Date().toLocaleString('he-IL', { timeZone: 'Asia/Jerusalem' });
    console.log(`[${disconnectTime}] WinForms client disconnected: ${socket.id}`);
  });
});

server.listen(PORT, () => {
  const startTime = new Date().toLocaleString('he-IL', { timeZone: 'Asia/Jerusalem' });
  console.log(`[${startTime}] Server is running on port ${PORT}`);
});
