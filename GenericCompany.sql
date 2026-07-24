/* =========================================================
   Final Project
   Theme: Generic Company
   Written by Jason Glili
   ========================================================= */

CREATE DATABASE Generic_Company;
GO
USE Generic_Company;
GO

CREATE SCHEMA company;
GO

-- #1 Create Department Table
CREATE TABLE company.Department(
	dept_id INT IDENTITY (1,1) PRIMARY KEY,
	dept_name VARCHAR(60) NOT NULL UNIQUE,
	budget INT NOT NULL
	)

-- #2 Create Merchandise Table
CREATE TABLE company.Merchandise(
	merch_id INT IDENTITY (100,1) PRIMARY KEY,
	item_name VARCHAR(40) NOT NULL UNIQUE,
	cost FLOAT NOT NULL,
	price FLOAT NOT NULL,
	dept_id INT NOT NULL,
	CONSTRAINT FK_Merch_Department
        FOREIGN KEY (dept_id) REFERENCES company.Department(dept_id)
	)

-- #3 Create Location Table
CREATE TABLE company.Location(
	building_id INT IDENTITY (100,1) PRIMARY KEY,
	building_name VARCHAR(40) NOT NULL UNIQUE,
	country VARCHAR(40) NOT NULL,
	us_state VARCHAR(40) NULL,
	zip_code VARCHAR(5) NOT NULL,
	location_address VARCHAR(40) NOT NULL UNIQUE,
	)

-- #4 Create Employee Table
CREATE TABLE company.Employee(
	employee_id INT IDENTITY(1000, 1) PRIMARY KEY,
	first_name  VARCHAR(40) NOT NULL,
    last_name   VARCHAR(40) NOT NULL,
	job_title VARCHAR(40) NOT NULL,
	salary INT NOT NULL,
	dept_id INT NOT NULL,
	building_id INT NOT NULL,
	CONSTRAINT FK_Employee_Department
        FOREIGN KEY (dept_id) REFERENCES company.Department(dept_id),
	CONSTRAINT FK_Employee_Location
        FOREIGN KEY (building_id) REFERENCES company.Location(building_id)
	)

-- #5 Create Event Table
	CREATE TABLE company.Event(
	event_id INT IDENTITY (1,1) PRIMARY KEY,
	event_name VARCHAR(60) NOT NULL,
	event_date DATE NOT NULL,
	attendees INT NOT NULL,
	cost FLOAT NOT NULL,
	building_id INT NOT NULL,
	CONSTRAINT FK_Events_Location
        FOREIGN KEY (building_id) REFERENCES company.Location(building_id)
	)

-- #1 Fill Departments Table
INSERT INTO company.Department (dept_name, budget) VALUES
('Finance', 500000),
('Customer Service', 300000),
('Research', 750000),
('Production', 1200000);

-- #2 Fill Locations Table
INSERT INTO company.Location (building_name, country, us_state, zip_code, location_address) VALUES
('Company HQ', 'USA', 'NY', '10036', '7 Times Square'),
('Call Center', 'USA', 'NE', '68102', '203 Berry Rd'),
('Company Factory 53', 'USA', 'TN', '37201', '543 Industrial Lot'),
('Company Labs', 'USA', 'CA', '94043', '1600 Amphitheatre Pkwy, Mountain View');

-- #3 Fill Employees Tables
INSERT INTO company.Employee (first_name, last_name, job_title, salary, dept_id, building_id) VALUES
('Charley', 'Duncan', 'Manager', 85000, 1, 100),
('Amie', 'Mcintyre', 'Associate', 55000, 2, 101),
('Jind', 'Matthews', 'Senior', 95000, 3, 103),
('Myles', 'Oneal', 'Intern', 35000, 4, 102),
('Ivan', 'Moss', 'Manager', 88000, 4, 102),
('Matthew', 'Giles', 'Associate', 58000, 1, 100),
('Amna', 'Bright', 'Senior', 92000, 3, 103);

-- #4 Fill Merchandise Tables
INSERT INTO company.Merchandise (item_name, cost, price, dept_id) VALUES
('Paper', 2.50, 5.00, 4),
('Cardboard', 3.00, 6.50, 4),
('Pens', 0.75, 2.00, 4),
('Calculators', 8.00, 15.00, 4),
('Computer Chips', 40.00, 90.00, 3);

-- #5 Fill Events Tables
INSERT INTO company.Event (event_name, event_date, attendees, cost, building_id) VALUES
('Shareholder Meeting', '2026-03-15', 40, 12000.00, 100),
('Pizza Party', '2026-06-20', 25, 400.00, 101),
('125th Company Anniversary', '2026-09-01', 300, 75000.00, 100);

SELECT * FROM company.Department
SELECT * FROM company.Employee
SELECT * FROM company.Event
SELECT * FROM company.Location
SELECT * FROM company.Merchandise