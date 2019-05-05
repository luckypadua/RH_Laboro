If Exists (Select * From SysObjects Where id = object_id(N'[Portal_Puestos]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[Portal_Puestos]
GO

Create View [dbo].[Portal_Puestos]
AS
	SELECT	CodEmp			= op.CodEmp,
			NombrePuesto	= pu.Nombre,
			IdPersona		= opp.IdPersonaHijo,
			IdLegajo		= l.IdLegajo
	FROM ORG_PuestoPersona opp  (nolock)
		JOIN ORG_Puestos pu		(nolock) ON opp.IdPuesto = pu.IdPuesto
		JOIN ORG_Estructuras oe (nolock) ON oe.IdEstructura = opp.IdEstructura
		JOIN ORG_Principal op	(nolock) ON op.IdOrganigrama = oe.IdOrganigrama
		JOIN BL_PERSONAS p		(nolock) ON p.IdPersona = opp.IdPersonaHijo
		LEFT JOIN BL_LEGAJOS l	(nolock) ON l.IdPersona = opp.IdPersonaHijo AND l.CodEmp = op.CodEmp
	WHERE oe.EsOrganigrama = 1  
    
GO

If Exists (Select * From SysObjects Where id = object_id(N'[Portal_Personas]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[Portal_Personas]
GO

Create View [dbo].[Portal_Personas]
AS
	Select p.IdPersona,
		   p.Codigo,
	       NombreCompleto		=IsNull(MAX(p.Nombre),''),
		   Nombres				=IsNull(MAX(p.Nombres),''),
		   Apellido				=IsNull(MAX(p.Apellido),''),
		   ApellidoCasada		=IsNull(MAX(p.ApellidoCasada),''),
		   DomicilioCalle		=IsNull(MAX(p.DomicilioCalle),''),
		   DomicilioNro			=IsNull(MAX(p.DomicilioNro),''),
		   DomicilioPiso		=IsNull(MAX(p.DomicilioPiso),''),
		   DomicilioDepto		=IsNull(MAX(p.DomicilioDepto),''),
		   DomicilioLoc			=IsNull(MAX(p.DomicilioLoc),''),
           DomicilioCodPos		=IsNull(MAX(p.DomicilioCodPos),''),
		   DomicilioProv		=IsNull(MAX(V.Nombre),''),
		   DNI					=IsNull(MAX(p.DocNro),''),
		   CUIL					=IsNull(MAX(p.CUIL),''),
		   Nacionalidad			=IsNull(MAX(N.Denominacion),''),
		   EstadoCivil			=IsNull(MAX(E.Denominacion),''),
		   Sexo					=IsNull(MAX(p.Sexo),''),
		   FecNacimiento		=IsNull(MAX(p.FecNacimiento),''),
		   Telefono				=IsNull(MAX(p.Telefono),''),
		   EmailPersonal		=IsNull(MAX(p.EmailPersonal),'')
	From bl_personas P			  (nolock) 
	Left Join BL_NACIONALIDADES N (nolock)	On N.IdNacionalidad = p.IdNacionalidad
	Left Join BL_ESTADOSCIVILES E (nolock)	On E.IdEstado = p.IdEstado
	Left Join BL_PROVINCIAS V	  (nolock)	On V.IdProvincia = p.IdProvincia
	Group by p.IdPersona, p.Codigo

GO

If Exists (Select * From SysObjects Where id = object_id(N'[Portal_Legajos]') And OBJECTPROPERTY(id, N'IsView') = 1) 
   DROP VIEW [dbo].[Portal_Legajos]
GO

Create View [dbo].[Portal_Legajos]
AS
SELECT DISTINCT l.IdLegajo, 
                p.IdPersona, 
                [Legajo]			= l.Legajo, 
                [NombreCompleto]	= p.Nombre, 
			    [C.U.I.L.]			= p.CUIL, 
				[Categoría]			= Cate.Nombre, 
				[Convenio]			= c.Denominacion, 
				[Doc.Nro.]			= p.DocNro, 
				[Empresa]			= l.CodEmp, 
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
				[SIJP-ModContratacion]	= mc.Descripcion , 
				[SIJP-Regimen]			= CASE l.Regimen 
											WHEN 0 THEN 'Capitalización' 
											WHEN 1 THEN 'Reparto' 
										  END, 
				[SIJP-ZonaGeografica]	= z.Descripcion, 
				[Tel.Directo]			= l.TelDirecto, 
				[Tel.Interno]			= l.TelInterno, 
				[LugarDeTrabajo]		= lt.Descripcion, 
				[Banco]					= R.BancoPagoDesc, 
				[ObraSocial]			= R.ObraSocialDesc
 FROM Bl_Legajos l					(nolock)
 LEFT JOIN BL_LIQUIDACIONES lq		(nolock) ON lq.idLiquidacion=(SELECT MAX(idLiquidacion) FROM BL_LIQUIDACIONES) 
 LEFT JOIN BL_PERSONAS p			(nolock) ON l.idPersona=p.IdPersona 
 LEFT JOIN BL_CONVENIOS c			(nolock) ON l.IdConvenio=c.IdConvenio 
 LEFT JOIN CATEGORIAS   Cate		(nolock) ON l.IdCategoria = Cate.IdCategoria AND l.IdConvenio = Cate.IdConvenio 
 LEFT JOIN BL_LUGARESTRABAJO lt		(nolock) ON lt.IdLugar=l.IdLugar 
 LEFT JOIN BL_MODCONTRATACIONES mc  (nolock) ON mc.ModContratacion = l.modContratacion
 LEFT JOIN vReparticionesLegajos R	(nolock) ON L.IDLEGAJO = r.IdLegajo  
 LEFT JOIN BL_ZONAS z				(nolock) ON l.zona = z.Zona  

GO



