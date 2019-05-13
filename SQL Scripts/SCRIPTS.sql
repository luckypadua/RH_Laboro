
exec ADDCOLUMN  'bl_personas','habilitadoPortal','Bit',0,1

If Exists (Select * From SysObjects Where id = object_id(N'[Portal_Puestos]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[Portal_Puestos]
GO

Create View [dbo].[Portal_Puestos]
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

If Exists (Select * From SysObjects Where id = object_id(N'[Portal_Personas]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[Portal_Personas]
GO

Create View [dbo].[Portal_Personas]
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

If Exists (Select * From SysObjects Where id = object_id(N'[Portal_Legajos]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[Portal_Legajos]
GO

Create View [dbo].[Portal_Legajos]
AS
SELECT			l.IdLegajo, 
                l.IdPersona, 
                [LegajoCodigo]		= l.Legajo, 
				[Empresa]			= l.CodEmp, 
				[Categoría]			= Cate.Nombre, 
				[Convenio]			= c.Denominacion, 
				[Estado]			= (CASE WHEN 
										/*Egreso*/
										(l.FecEgreso is Null OR l.FecEgreso=lq.MesLiq)
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
											WHEN 0 THEN 'Capitalización' 
											WHEN 1 THEN 'Reparto' 
										  END, 
				[ZonaGeografica]		= z.Descripcion, 
				[Telefono-Directo]		= l.TelDirecto, 
				[Telefono-Interno]		= l.TelInterno, 
				[LugarDeTrabajo]		= lt.Descripcion, 
				[Banco]					= R.BancoPagoDesc,
				[BancoCBU]				= cb.NroInscrLegajo, 
				[ObraSocial]			= R.ObraSocialDesc
 FROM Bl_Legajos l					(nolock)
 LEFT JOIN BL_LIQUIDACIONES lq		(nolock) ON lq.idLiquidacion=(SELECT MAX(idLiquidacion) FROM BL_LIQUIDACIONES) 
 LEFT JOIN BL_CONVENIOS c			(nolock) ON l.IdConvenio=c.IdConvenio 
 LEFT JOIN CATEGORIAS   Cate		(nolock) ON l.IdCategoria = Cate.IdCategoria AND l.IdConvenio = Cate.IdConvenio 
 LEFT JOIN BL_LUGARESTRABAJO lt		(nolock) ON lt.IdLugar=l.IdLugar 
 LEFT JOIN BL_MODCONTRATACIONES mc  (nolock) ON mc.ModContratacion = l.modContratacion
 LEFT JOIN vReparticionesLegajos R	(nolock) ON L.IDLEGAJO = r.IdLegajo 
 LEFT JOIN vCtaBancaria			cb	(nolock) ON L.IDLEGAJO = cb.IdLegajo 	
 LEFT JOIN BL_ZONAS z				(nolock) ON l.zona = z.Zona  

GO

If Exists (Select * From SysObjects Where id = object_id(N'[Portal_LoguinsIni]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[Portal_LoguinsIni]
GO

Create View [dbo].[Portal_LoguinsIni]
AS

	Select p.IdPersona,
		   Usuario = p.CUIL,
		   Contrasenia=p.CUIL
	From bl_personas P (nolock)
	Where P.habilitadoPortal = 1 

GO

