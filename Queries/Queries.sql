--1) Вывести сотрудника с максимальной заработной платой.
SELECT Employee.Name,
       Employee.Salary
FROM dbo.Employee
WHERE Employee.Salary =
    (SELECT max(Employee.Salary)
     FROM dbo.Employee)

--2) Вывести отдел с самой высокой заработной платой между сотрудниками.
SELECT Department.Name
FROM dbo.Department
JOIN dbo.Employee ON Department.Id = Employee.DepartmentId
WHERE Employee.Salary =
    (SELECT top(1) max(Employee.Salary)
     FROM dbo.Employee
     ORDER BY avg(Employee.Salary))

--3) Вывести отдел с максимальной суммарной зарплатой сотрудников.
SELECT Department.Name
FROM dbo.Department
JOIN dbo.Employee ON Department.Id = Employee.DepartmentId
WHERE Employee.Salary =
    (SELECT top(1) max(Employee.Salary)
     FROM dbo.Employee
     ORDER BY sum(Employee.Salary))

--4) Вывести сотрудника, чье имя начинается на «Р» и заканчивается на «н».
SELECT Employee.Name
FROM dbo.Employee
WHERE Employee.Name LIKE 'Р%'
  AND Employee.Name LIKE '%н'
