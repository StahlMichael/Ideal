app.post('/order', async (req, res) => {
    const data = req.body;

    try {
        await sql.connect(sqlConfig);

        // Insert Vacation
        const vacationResult = await sql.query`
            INSERT INTO Vacation (VacationDate, SubmissionDate, TermsAccepted)
            OUTPUT INSERTED.VacationId
            VALUES (${data.vacationDate}, ${data.submissionDate}, ${data.termsAccepted})
        `;
        const vacationId = vacationResult.recordset[0].VacationId;

        // Insert ContactInfo
        await sql.query`
            INSERT INTO ContactInfo (VacationId, Address, Phone, Email, Notes)
            VALUES (${vacationId}, ${data.contactInfo.address}, ${data.contactInfo.phone}, ${data.contactInfo.email}, ${data.contactInfo.notes})
        `;

        // Insert Room
        await sql.query`
            INSERT INTO Room (VacationId, TotalRooms, RoomType, DeluxeFor, DeluxeRoomsCount)
            VALUES (${vacationId}, ${data.rooms.totalRooms}, ${data.rooms.roomType}, ${data.rooms.deluxeFor}, ${data.rooms.deluxeRoomsCount})
        `;

        // Insert SkiLessons
        const skiLessonsResult = await sql.query`
            INSERT INTO SkiLessons (VacationId, NotInterested)
            OUTPUT INSERTED.SkiLessonsId
            VALUES (${vacationId}, ${data.skiLessons.notInterested})
        `;
        const skiLessonsId = skiLessonsResult.recordset[0].SkiLessonsId;

        // Insert SkiLessonParticipants (kids, adults, private)
        for (const type of ['kids', 'adults', 'private']) {
            const participants = data.skiLessons[type]?.participants || [];
            for (const participant of participants) {
                await sql.query`
                    INSERT INTO SkiLessonParticipants (SkiLessonsId, Type, Name)
                    VALUES (${skiLessonsId}, ${type}, ${participant.name || null})
                `;
            }
        }

        // Insert Passengers and FrequentFlyer
        for (const pax of data.passengers) {
            const passengerResult = await sql.query`
                INSERT INTO Passenger (VacationId, TempId, LastName, FirstName, BirthDate, PassportNumber, Gender, NoFlight, SkiPass, SkiLevel)
                OUTPUT INSERTED.PassengerId
                VALUES (${vacationId}, ${pax.tempId}, ${pax.lastName}, ${pax.firstName}, ${pax.birthDate}, ${pax.passportNumber}, ${pax.gender}, ${pax.noFlight}, ${pax.skiPass}, ${pax.skiLevel})
            `;
            const passengerId = passengerResult.recordset[0].PassengerId;

            await sql.query`
                INSERT INTO FrequentFlyer (PassengerId, HasNumber, Number)
                VALUES (${passengerId}, ${pax.frequentFlyer.hasNumber}, ${pax.frequentFlyer.number})
            `;
        }

        // Insert Files (if any)
        for (const file of data.files) {
            await sql.query`
                INSERT INTO VacationFile (VacationId, FilePath)
                VALUES (${vacationId}, ${file.filePath || null})
            `;
        }

        res.json({ message: 'Order saved to SQL Server' });
    } catch (err) {
        console.error('Database error:', err);
        res.status(500).json({ error: 'Database error', details: err });
    }
});

app.listen(port, () => {
    console.log(`Server is listening on port ${port}`);
});