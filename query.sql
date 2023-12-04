use master
go

if exists(select * from sys.databases where name = "GuCube")
    drop database GuCube

create database GuCube
go

use GuCube
go

create table User {
    "ID" int identity primary key,
    "Name" varchar(80) not null,
    "Password" varchar(MAX) not null,
    "Salt" varchar(200) not null
}

create table Store {
    "ID" int identity primary key,
    "Name" varchar(80) not null,
    "Localization" varchar(200) not null,
}

create table StoreManager {
    "ID" int identity primary key,
    "StoreID" int references Store(ID) not null,
    "UserID" int references User(ID) not null
}

create table Product {
    "ID" int identity primary key,
    "Name" varchar(80) not null,
    "Description" varchar(500) not null,
    "Image" varbinary(MAX) null
}

create table Ingredient {
    "ID" int identity primary key,
    "ProductID" int references Product(ID) not null,
    "Name" varchar(80) not null,
    "Amount" int not null,
    "Unit" varchar(10) not null
}

create table Menu {
    "ID" int identity primary key,
    "StoreID" int references Store(ID) not null,
    "ProductID" int references Product(ID) not null,
    "Price" float not null
}

create table Order {
    "ID" int identity primary key,
    "StoreID" int references Store(ID) not null,
    "Name" varchar(80) not null,
    "IsDelivered" bit not null
}

create table OrderProduct {
    "ID" int identity primary key,
    "OrderID" int references Order(ID) not null,
    "ProductID" int references Product(ID) not null
}

create table Coupon {
    "ID" varchar(16) identity primary key,
    "UserID" int references User(ID) not null,
    "MenuID" int references Menu(ID) not null,
    "OrderID" int references Order(ID) null,
    "Discount" float not null
}