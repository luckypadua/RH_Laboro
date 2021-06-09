Create View vNombresRepetidos
AS
Select U.Nombre 
From (Select Nombre, Cantidad = count(Nombre) from BL_PERSONAS group by Nombre) U
Where U.Cantidad > 1
GO

Create View vEmailRepetidos
AS
Select U.EmailPersonal
From (Select EmailPersonal, Cantidad = count(EmailPersonal) from BL_PERSONAS group by EmailPersonal having not EmailPersonal is null) U
Where U.Cantidad > 1 And Not U.EmailPersonal is null
GO

print 'Códigos de Personas con Mail repetidos'
select Codigo from BL_PERSONAS where EmailPersonal in (select EmailPersonal from vEmailRepetidos)
print 'Códigos de Personas con Nombres repetidos'
select Codigo from BL_PERSONAS where Nombre in (select Nombre from vNombresRepetidos)

drop view vEmailRepetidos
drop view vNombresRepetidos
GO

