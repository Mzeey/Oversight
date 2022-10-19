Create database OversightDb;

USE OversightDb

CREATE TABLE ResidentialStatuses(
	Id INT IDENTITY(1,1) NOT NULL,
	Title VARCHAR(MAX),
	PRIMARY KEY (Id),
);
GO

CREATE TABLE Users(
	Id INT NOT NULL IDENTITY(1,1),
	ResidentialStatusId INT,
	FirstName VARCHAR(50),
	MiddleName VARCHAR(50),
	LastName VARCHAR(50),
	ProfileImageUrl VARCHAR(MAX),
	HashedPassword VARCHAR(MAX),
	Username VARCHAR(50),
	Email VARCHAR(MAX),
	FamilyStatus VARCHAR(25),
	PhoneNumber VARCHAR(20),
	HouseAddress VARCHAR(MAX),
	IsActive BIT,
	StatusControl SMALLINT,
	DateCreated DATETIME,
	DateModified DATETIME,
	
	PRIMARY KEY (Id),
	FOREIGN KEY (ResidentialStatusId) REFERENCES ResidentialStatuses(Id)
);
GO

CREATE TABLE Settings (
	Id INT NOT NULL IDENTITY(1,1),
	UseTwoFA BIT,
	DateCreated DATETIME,
	DateModified DATETIME,

	PRIMARY KEY (Id)
);
GO

CREATE TABLE UsersSettings (
	Id INT NOT NULL IDENTITY(1,1),
	UserId INT,
	SettingId INT,

	PRIMARY KEY (Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (SettingId) REFERENCES Settings(Id)
);
GO

CREATE TABLE TwoFAs (
	Id INT NOT NULL IDENTITY(1,1),
	UserId INT,
	AuthReceiver VARCHAR(10),
	DateCreated DATETIME,
	DateModified DATETIME, 

	PRIMARY KEY(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO

CREATE TABLE Fees(
	Id INT NOT NULL IDENTITY(1,1),
	Title VARCHAR(max),
	IsActive BIT,
	DateCreated DATETIME,
	DateModified DATETIME,

	PRIMARY KEY (Id)
);
GO

CREATE TABLE Subscriptions(
	Id INT NOT NULL IDENTITY(1,1),
	UserId INT NOT NULL, 
	FeeId INT NOT NULL,
	StartDate DATETIME,
	EndDate DATETIME,

	PRIMARY KEY (Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (FeeId) REFERENCES Fees (Id)
);
GO

CREATE TABLE ResidentialStatusFees(
	Id INT NOT NULL IDENTITY(1,1),
	FeeId INT NOT NULL, 
	ResidentialStatusId INT NOT NULL,
	Amount DECIMAL(19,4),
	DateCreated DATETIME,
	DateModified DATETIME,

	PRIMARY KEY (Id),
	FOREIGN KEY (FeeId) REFERENCES Fees(Id),
	FOREIGN KEY (ResidentialStatusId) REFERENCES ResidentialStatuses(Id)
); 
GO

CREATE TABLE PaymentMethods(
	Id INT NOT NULL IDENTITY(1,1),
	UserId INT NOT NULL, 
	Title VARCHAR(MAX),
	CardHolderName VARCHAR(MAX),
	CardNumber VARCHAR(25),
	CardType VARCHAR(25),
	DateCreated DATETIME,
	DateModified DATETIME,

	PRIMARY KEY (Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id)

);
GO

CREATE TABLE OtpCodes (
	Id INT NOT NULL IDENTITY (1,1),
	UserId	INT NOT NULL,
	Code SMALLINT,
	IsValid BIT,
	DateCreated DATETIME,
	ExpiryDate DATETIME,

	PRIMARY KEY(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO

CREATE TABLE Invoices(
	Id INT NOT NULL IDENTITY(1,1),
	UserId INT NOT NULL,
	Amount DECIMAL(19,4),
	DurationInMonths INT,
	DatePaid DATETIME,
	ModeOfPayment VARCHAR(MAX)

	PRIMARY KEY(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO

CREATE TABLE EstateAddresses(
	Id INT NOT NULL IDENTITY(1,1),
	Title VARCHAR(MAX),
	Area VARCHAR(MAX),
	City VARCHAR(MAX),
	EState VARCHAR (MAX), 
	Country VARCHAR(MAX),

	PRIMARY KEY (Id)
);
GO