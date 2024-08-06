Create Database Attendance;

Create Table Employees(
Emp_Id INT IDENTITY(1,1) PRIMARY KEY,
Emp_Name varchar (max),
Emp_Fmd varchar (max),
Emp_Joining varchar(max)
);

Create Table Attendances (
Id Int ,
_Date varchar(255),
CheckIN varchar(255),
CheckOUT varchar (255),
Task varchar (255),
Foreign Key (Id) References Employees (Emp_Id)
);
insert into Employees (Emp_Name,Emp_Fmd,Emp_Joining)
Values ('Saim Masood', 'Finger Data' , '11/12/21');

insert into Attendances
Values (2,'11/12/24','in', 'out' ,'Digital Persona');

select * from Employees;
select * from Attendances;
delete from Attendances;
delete from Employees;

--Select * from Attendances as a ,Employees as e 
--where a.Id = e.Emp_Id and a.id=1;
Select * from Attendances as a Join Employees as e 
on a.Id = e.Emp_Id
where  a.id=1;
drop table Attendances
drop table Employees