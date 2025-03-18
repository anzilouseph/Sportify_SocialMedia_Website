create database SportifyKerala;
use SportifyKerala;

-- Table1
CREATE TABLE Districts (DistrictId CHAR(36) PRIMARY KEY default(uuid()), DistrictName VARCHAR(50) NOT NULL);

-- Table2
CREATE TABLE Users (UserId CHAR(36) PRIMARY KEY default(uuid()),FullName VARCHAR(100) NOT NULL,Email VARCHAR(150) NOT NULL UNIQUE,Phone VARCHAR(100) NOT NULL UNIQUE,Address VARCHAR(500),DistrictId CHAR(36), CommitieApproval boolean DEFAULT False,CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,Password varchar(500)not null,salt varchar(500),FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId) ON DELETE SET NULL);
Alter table users Add Column ProfileImage varchar(255);
Alter table users Rename Column CommitieApproval to Commitie;
ALTER TABLE users MODIFY Commitie TINYINT(1) DEFAULT 0;
Alter table users Add Column Role enum ('Admin','User','Club') default 'User';

-- Table3
create table TournamentPost(PostId char(36) primary key default(uuid()),ClubId char(36) not null, DistrictId char(36) not null, Description varchar(500),PostDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,ImageName varchar(255),CategoryId char(36),FOREIGN KEY (ClubId) REFERENCES Users(UserId) ON DELETE CASCADE,FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId) ON DELETE CASCADE,FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId) ON DELETE CASCADE);

-- Table4
create table Category(CategoryId CHAR(36) PRIMARY KEY default(uuid()), CategoryName VARCHAR(50) NOT NULL);

-- Table5
Create Table Follow(FollowId char(36) default(uuid()), ClubId char(36) NOT NULL,FollowerId char(36) NOT NULL,IsFollowing tinyint(1) default 1,Accepeted tinyint(1) default 0, primary key (ClubId,FollowerId) , FOREIGN KEY (ClubId) REFERENCES Users(UserId) ON DELETE CASCADE,FOREIGN KEY (FollowerId) REFERENCES Users(UserId) ON DELETE CASCADE);

desc users;

INSERT INTO Districts (DistrictName) VALUES ('Thiruvananthapuram');
INSERT INTO Districts (DistrictName) VALUES ('Kollam');
INSERT INTO Districts (DistrictName) VALUES ('Pathanamthitta');
INSERT INTO Districts (DistrictName) VALUES ('Alappuzha');
INSERT INTO Districts (DistrictName) VALUES ('Kottayam');
INSERT INTO Districts (DistrictName) VALUES ('Idukki');
INSERT INTO Districts (DistrictName) VALUES ('Ernakulam');
INSERT INTO Districts (DistrictName) VALUES ('Thrissur');
INSERT INTO Districts (DistrictName) VALUES ('Palakkad');
INSERT INTO Districts (DistrictName) VALUES ('Malappuram');
INSERT INTO Districts (DistrictName) VALUES ('Kozhikode');
INSERT INTO Districts (DistrictName) VALUES ('Wayanad');
INSERT INTO Districts (DistrictName) VALUES ('Kannur');
INSERT INTO Districts (DistrictName) VALUES ('Kasaragod');


INSERT INTO Category (CategoryName) VALUES ('Cricket');
INSERT INTO Category (CategoryName) VALUES ('Football');
INSERT INTO Category (CategoryName) VALUES ('Basketball');
INSERT INTO Category (CategoryName) VALUES ('Volleyball');
INSERT INTO Category (CategoryName) VALUES ('Hockey');
INSERT INTO Category (CategoryName) VALUES ('Chess');
INSERT INTO Category (CategoryName) VALUES ('Kabaddi');
INSERT INTO Category (CategoryName) VALUES ('Badminton');
INSERT INTO Category (CategoryName) VALUES ('Kho-Kho');
INSERT INTO Category (CategoryName) VALUES ('Table Tennis');



select * from districts;
select * from TournamentPost;
select * from Category;
select * from users;



-- Stored Procedures

-- 1) 
Delimiter //
create procedure UserRegistration(in nameOfUser VARCHAR(100),in emailOfUser  VARCHAR(150),in phoneOfUser VARCHAR(100),in addressOfUser  VARCHAR(500),in  idOfDistrict  CHAR(36),in passwordOfUser  varchar(500),in salt varchar(255))
BEGIN 

insert into Users(FullName,Email,Phone,Address,DistrictId,Password,salt) values (nameOfUser,emailOfUser,phoneOfUser,addressOfUser,idOfDistrict,passwordOfUser,salt);

END //
Delimiter ;

-- 2) 
Delimiter //
create procedure CommitieRegistration(in nameOfUser VARCHAR(100),in emailOfUser  VARCHAR(150),in phoneOfUser VARCHAR(100),in addressOfUser  VARCHAR(500),in  idOfDistrict  CHAR(36),in passwordOfUser  varchar(500),in salt varchar(255))
BEGIN 

insert into Users(FullName,Email,Phone,Address,DistrictId,Password,salt,Commitie) values (nameOfUser,emailOfUser,phoneOfUser,addressOfUser,idOfDistrict,passwordOfUser,salt,1);

END //
Delimiter ;


select FullName,ProfileImage from Users where userid="91dffb85-ff12-11ef-921f-18c04de083c5";
show tables;
desc follow
