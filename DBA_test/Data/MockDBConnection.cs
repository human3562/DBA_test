﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DBA_test.Data;

public static class MockDBConnection { //InMemory static DB
    private const string connectionString = "Data Source=InMemoryDB;Mode=Memory;Cache=Shared"; 
    private static SqliteConnection? connection = null;

    private static void InsertTableDefinitions() {
        if (connection == null) return;
        var tableDefCommand = connection.CreateCommand();
        tableDefCommand.CommandText = """                        
            CREATE TABLE PhoneNumber (
                id INTEGER PRIMARY KEY,
                Home TEXT,
                Work TEXT,
                Mobile TEXT
            );
            CREATE TABLE Streets (
                id INTEGER PRIMARY KEY,
                Title TEXT
            );
            CREATE TABLE Address (
                id INTEGER PRIMARY KEY,
                House TEXT,
                Street_id INTEGER,
                FOREIGN KEY (Street_id) REFERENCES Streets(id)

            );
            CREATE TABLE Abonent (
                id INTEGER PRIMARY KEY,
                FullName TEXT,
                Address_id INTEGER,
                PhoneNumber_id INTEGER,
                FOREIGN KEY (Address_id) REFERENCES Address(id),
                FOREIGN KEY (PhoneNumber_id) REFERENCES PhoneNumber(id)
            );
            """;

        tableDefCommand.ExecuteNonQuery();
    }

    private static void InsertMockData() {
        if (connection == null) return;
        var mockDataCommand = connection.CreateCommand();
        mockDataCommand.CommandText = """
                INSERT INTO Streets (Title) VALUES('Некрасова');
                INSERT INTO Streets (Title) VALUES('Данилова');
                INSERT INTO Streets (Title) VALUES('Жебрунова ');
                INSERT INTO Streets (Title) VALUES('Заречная');
                INSERT INTO Streets (Title) VALUES('Фридрихштрассе');
                INSERT INTO Streets (Title) VALUES('Хажайная');
                
                INSERT INTO Address (House, Street_id) VALUES('21', 1);
                INSERT INTO Address (House, Street_id) VALUES('13', 2);
                INSERT INTO Address (House, Street_id) VALUES('23', 2);
                INSERT INTO Address (House, Street_id) VALUES('5', 2);
                INSERT INTO Address (House, Street_id) VALUES('1', 3);
                INSERT INTO Address (House, Street_id) VALUES('5', 4);
                INSERT INTO Address (House, Street_id) VALUES('12', 5);
                INSERT INTO Address (House, Street_id) VALUES('50', 6);
                
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)240-98-36', '7(123)738-10-68', '7(123)144-88-93');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)121-91-96', '7(123)387-29-29', '7(123)338-18-04');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)760-72-73', '7(123)629-35-96', '7(123)136-44-55');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)741-29-83', '7(123)616-49-55', '7(123)141-26-96');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)332-17-92', '7(123)938-00-69', '7(123)406-20-02');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)147-23-41', '7(123)467-48-68', '7(123)304-11-95');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)118-49-20', '7(123)878-46-44', '7(123)327-77-32');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)118-49-20', '7(123)517-56-31', '7(123)741-34-71');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)118-49-20', '7(123)706-86-52', '7(123)870-87-32');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)495-74-68', '7(123)058-61-18', '7(123)373-57-88');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)860-89-15', '7(123)058-61-18', '7(123)058-88-16');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)343-56-30', '7(123)058-61-18', '7(123)580-95-18');
                INSERT INTO PhoneNumber (Home, Work, Mobile) VALUES('7(123)575-67-78', '7(123)058-61-18', '7(123)982-72-48');
                
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Некрасов Илья Муромич', 1, 1);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Алабамыч Никита Андреевич', 2, 2);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Дегтярев Иван Иванович', 3, 3);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Киселёв Ибрагил Платонович', 4, 4);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Доронин Андрей Богуславович', 5, 5);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Иванков Митрофан Васильевич', 6, 6);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Ершов Ефим Юлианович', 7, 7);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Бобылёв Ермак Ростиславович', 8, 8);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Поляков Зиновий Кириллович', 6, 9);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Гуляев Валентин Ярославович', 5, 10);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Буров Нисон Платонович', 4, 11);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Носов Вилли Аркадьевич', 3, 12);
                INSERT INTO Abonent (FullName, Address_id, PhoneNumber_id) VALUES('Агафонов Венедикт Петрович', 2, 13);


            """;
        mockDataCommand.ExecuteNonQuery();
    }

    public static SqliteConnection? GetMockConnection() {
        if(connection == null) return null;
        return connection;
    }

    public static void Start() {
        connection = new SqliteConnection(connectionString);
        connection.Open();
        InsertTableDefinitions();
        InsertMockData();
    }

    public static void Stop() {
        connection?.Close();
    }
}

