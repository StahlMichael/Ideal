import express from 'express';

const app = express();
app.use(express.json());

app.post('/api/application/response', (req, res) => {
  const { applicationId, action } = req.body;
  res.json({ message: `Application ${applicationId} ${action}ed.` });
});

const PORT = 3000;
app.listen(PORT, () => {
  console.log(`Server running on port ${PORT}`);
});