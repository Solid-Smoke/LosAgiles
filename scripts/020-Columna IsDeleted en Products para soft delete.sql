USE LosAgilesDB
GO
alter table Products add IsDeleted bit not null default 0
GO