-- Altas de columnas
exec ADDCOLUMN  'bl_personas','habilitadoPortal','Bit'         ,0,1
exec ADDCOLUMN  'BL_RECIBOS' ,'PDF'             ,'varchar(100)',1
exec ADDCOLUMN  'BL_RECIBOS' ,'MD5'             ,'varchar(100)',1
exec ADDCOLUMN  'BL_RECIBOS' ,'FTPUpLoad'       ,'datetime'    ,1
-- Fin Alta de columnas

SELECT 0

If Exists (Select * From SysObjects Where id = object_id(N'[vAutogestion_Puestos]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[vAutogestion_Puestos]
GO

Create View [dbo].[vAutogestion_Puestos]
AS
	SELECT	Empresa			= op.CodEmp,
			NombrePuesto	= pu.Nombre,
			IdPersona		= opp.IdPersonaHijo,
			IdLegajo		= l.IdLegajo
	FROM ORG_PuestoPersona opp		(nolock)
	LEFT JOIN ORG_Puestos pu		(nolock) ON opp.IdPuesto = pu.IdPuesto
	LEFT JOIN ORG_Estructuras oe	(nolock) ON oe.IdEstructura = opp.IdEstructura
	LEFT JOIN ORG_Principal op		(nolock) ON op.IdOrganigrama = oe.IdOrganigrama
	LEFT JOIN BL_PERSONAS p			(nolock) ON p.IdPersona = opp.IdPersonaHijo
	LEFT JOIN BL_LEGAJOS l			(nolock) ON l.IdPersona = opp.IdPersonaHijo AND l.CodEmp = op.CodEmp
	WHERE oe.EsOrganigrama = 1  
    
GO

If Exists (Select * From SysObjects Where id = object_id(N'[vAutogestion_Personas]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[vAutogestion_Personas]
GO

Create View [dbo].[vAutogestion_Personas]
AS
	Select p.IdPersona,
		   PersonaCodigo        = p.Codigo,
	       NombreCompleto		= p.Nombre,
		   Nombres				= p.Nombres,
		   Apellido				= p.Apellido,
		   ApellidoCasada		= p.ApellidoCasada,
		   DomicilioCalle		= p.DomicilioCalle,
		   DomicilioNro			= p.DomicilioNro,
		   DomicilioPiso		= p.DomicilioPiso,
		   DomicilioDepto		= p.DomicilioDepto,
		   DomicilioLoc			= p.DomicilioLoc,
           DomicilioCodPos		= p.DomicilioCodPos,
		   DomicilioProv		= V.Nombre,
		   DNI					= p.DocNro,
		   CUIL					= p.CUIL,
		   Nacionalidad			= N.Denominacion,
		   EstadoCivil			= E.Denominacion,
		   Sexo					= p.Sexo,
		   FecNacimiento		= p.FecNacimiento,
		   Telefono				= p.Telefono,
		   EmailPersonal		= p.EmailPersonal
	From bl_personas P			  (nolock) 
	Left Join BL_NACIONALIDADES N (nolock)	On N.IdNacionalidad = p.IdNacionalidad
	Left Join BL_ESTADOSCIVILES E (nolock)	On E.IdEstado = p.IdEstado
	Left Join BL_PROVINCIAS V	  (nolock)	On V.IdProvincia = p.IdProvincia

GO

If Exists (Select * From SysObjects Where id = object_id(N'[vAutogestion_Legajos]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[vAutogestion_Legajos]
GO

Create View [dbo].[vAutogestion_Legajos]
AS
SELECT			l.IdLegajo, 
                l.IdPersona, 
                [LegajoCodigo]		= l.Legajo, 
				[Empresa]			= l.CodEmp, 
				[Categor­a]			= Cate.Nombre, 
				[Convenio]			= c.Denominacion, 
				[Estado]			= (CASE WHEN 
										/*Egreso*/
										(l.FecEgreso is Null OR l.FecEgreso = lq.MesLiq)
										/*Ingeso*/
										AND ((NOT l.fecIngreso is NULL AND datepart(year,l.FecIngreso)*100+datepart(month,l.FecIngreso) <= datepart(year,lq.Mesliq)*100+datepart(month,lq.MesLiq)) 
										OR	(NOT l.fecingreso is NULL AND lq.MesLiq IS NULL))
									  THEN 'Activo' ELSE 'Inactivo' End), 
				[FechaAntiguedad]		= l.FecBaseAnt,
				[FechaIngreso]			= l.FecIngreso, 
				[FechaIndemnizacion]	= l.FecBaseIndem, 
				[FechaEgreso]			= l.FecEgreso, 
				[FechaJubilacion]		= l.FecJubilacion, 
				[ModContratacion]		= mc.Descripcion , 
				[Regimen]				= CASE l.Regimen 
											WHEN 0 THEN 'Capitalizaci½n' 
											WHEN 1 THEN 'Reparto' 
										  END, 
				[ZonaGeografica]		= z.Descripcion, 
				[Telefono-Directo]		= l.TelDirecto, 
				[Telefono-Interno]		= l.TelInterno, 
				[LugarDeTrabajo]		= lt.Descripcion, 
				[Banco]					= R.BancoPagoDesc,
				[BancoCBU]				= cb.NroInscrLegajo, 
				[ObraSocial]			= R.ObraSocialDesc
FROM BL_LEGAJOS l					(nolock)
LEFT JOIN BL_LIQUIDACIONES lq		(nolock) ON lq.idLiquidacion = (SELECT MAX(idLiquidacion) FROM BL_LIQUIDACIONES) 
LEFT JOIN BL_CONVENIOS c			(nolock) ON l.IdConvenio = c.IdConvenio 
LEFT JOIN CATEGORIAS   Cate			(nolock) ON l.IdCategoria = Cate.IdCategoria AND l.IdConvenio = Cate.IdConvenio 
LEFT JOIN BL_LUGARESTRABAJO lt		(nolock) ON lt.IdLugar = l.IdLugar 
LEFT JOIN BL_MODCONTRATACIONES mc	(nolock) ON mc.ModContratacion = l.modContratacion
LEFT JOIN vReparticionesLegajos R	(nolock) ON L.IDLEGAJO = r.IdLegajo 
LEFT JOIN vCtaBancaria			cb	(nolock) ON L.IDLEGAJO = cb.IdLegajo 	
LEFT JOIN BL_ZONAS z				(nolock) ON l.zona = z.Zona  

GO

If Exists (Select * From SysObjects Where id = object_id(N'[vAutogestion_LoguinsIni]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[vAutogestion_LoguinsIni]
GO

Create View [dbo].[vAutogestion_LoguinsIni]
AS
		SELECT p.IdPersona,
			   Usuario = p.CUIL,
			   Contrasenia = p.CUIL
		FROM Bl_Personas P (nolock)
		WHERE P.habilitadoPortal = 1 
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vAutogestion_Familiares]'))
	DROP VIEW [dbo].[vAutogestion_Familiares]
GO

CREATE ViEW [dbo].[vAutogestion_Familiares]
AS
    SELECT
        FAM.IdPersona,
        FAM.IdFamiliar,
        FAM.Secuencia,
        FAM.Nombres,
        FAM.Apellido,
        FAM.FecNacimiento,
        [Parentesco] = PAR.Descripcion,
        FAM.FecACargo
    FROM BL_FAMILIARES FAM              (NOLOCK) 
        INNER JOIN BL_PARENTESCOS PAR   (NOLOCK) ON (PAR.IdParentesco = FAM.IdParentesco)
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vAutogestion_ManagersYEmpleados]'))
	DROP VIEW [dbo].[vAutogestion_ManagersYEmpleados]
GO

CREATE ViEW [dbo].[vAutogestion_ManagersYEmpleados]
AS
    SELECT IdManager,
	   IdLegajo AS IdEmpleado
    FROM BL_EMPLEADOSACARGO
GO