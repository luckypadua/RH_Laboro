-- Altas de columnas
exec ADDCOLUMN  'bl_personas','habilitadoPortal','Bit'           ,0,1
exec ADDCOLUMN  'BL_RECIBOS' ,'PDF_Nombre'      ,'varchar(100)'  ,1
exec ADDCOLUMN  'BL_RECIBOS' ,'PDF_RutaLOC'     ,'varchar(3000)' ,1
exec ADDCOLUMN  'BL_RECIBOS' ,'PDF_RutaFTP'     ,'varchar(3000)' ,1
exec ADDCOLUMN  'BL_RECIBOS' ,'MD5'             ,'varchar(100)'  ,1
exec ADDCOLUMN  'BL_RECIBOS' ,'FTPUpLoad'       ,'datetime'      ,1
exec ADDCOLUMN  'BL_RECIBOS' ,'Visualizado'	    ,'datetime'		 ,1
exec ADDCOLUMN  'BL_RECIBOS' ,'Firmado'	        ,'tinyint'	     ,0,0
exec ADDCOLUMN  'BL_RECIBOS' ,'Firmado_Fecha'   ,'datetime'		 ,1
exec ADDCOLUMN  'BL_RECIBOS' ,'Observacion'	    ,'varchar(8000)' ,1
-- Fin Alta de columnas

IF EXISTS (SELECT Name FROM sysindexes WHERE Name = 'IX_BL_RECIBOS_PDF_Nombre')
	DROP INDEX [IX_BL_RECIBOS_PDF_Nombre] ON [dbo].[BL_RECIBOS]
GO
CREATE NONCLUSTERED INDEX [IX_BL_RECIBOS_PDF_Nombre] ON [dbo].[BL_RECIBOS]
	([PDF_Nombre] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

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
		   FecNacimiento		= CONVERT(VARCHAR, p.FecNacimiento, 103),
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
				[Empresa]			= ltrim(E.CODEMP) + ' - ' +  E.NOMBRE, 
				[Categor­a]			= Cate.Nombre, 
				[Convenio]			= c.Denominacion, 
				[Estado]			= (CASE WHEN 
										/*Egreso*/
										(l.FecEgreso is Null OR l.FecEgreso = lq.MesLiq)
										/*Ingeso*/
										AND ((NOT l.fecIngreso is NULL AND datepart(year,l.FecIngreso)*100+datepart(month,l.FecIngreso) <= datepart(year,lq.Mesliq)*100+datepart(month,lq.MesLiq)) 
										OR	(NOT l.fecingreso is NULL AND lq.MesLiq IS NULL))
									  THEN 'Activo' ELSE 'Inactivo' End), 
				[FechaAntiguedad]		= CONVERT(VARCHAR, l.FecBaseAnt, 103),
				[FechaIngreso]			= CONVERT(VARCHAR, l.FecIngreso, 103),
				[FechaIndemnizacion]	= CONVERT(VARCHAR, l.FecBaseIndem, 103),
				[FechaEgreso]			= CONVERT(VARCHAR, l.FecEgreso, 103),
				[FechaJubilacion]		= CONVERT(VARCHAR, l.FecJubilacion, 103),
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
LEFT JOIN EMPRESAS E				(nolock) ON E.CODEMP = l.CODEMP

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
        [FecNacimiento] = CONVERT(VARCHAR, FAM.FecNacimiento, 103),
        [Parentesco]    = PAR.Descripcion,
        [FecACargo]     = CONVERT(VARCHAR, FAM.FecACargo, 103)
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

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vAutogestion_Recibos]'))
	DROP VIEW [dbo].[vAutogestion_Recibos]
GO

Create View [dbo].[vAutogestion_Recibos]
AS
SELECT Clave = Ltrim(R.Idliquidacion) + '-' + LTrim(R.IdLegajo)
      ,R.[IdLiquidacion]
      ,R.[IdLegajo]
      ,P.IdPersona   
	  ,L.LegajoCodigo 
	  ,R.[PDF_Nombre]
      ,R.[PDF_RutaFTP]
      ,R.[FTPUpLoad]
      ,R.[Firmado]
	  ,R.[Firmado_Fecha]
      ,R.[Observacion]
      ,R.[Visualizado]
	  ,Empresa = L.Empresa 
	  ,Liquidacion_Mes = LIQ.MesLiq
	  ,Liquidacion_Codigo = LT.CodLiq 
	  ,Liquidacion_Nombre = LT.Descripcion  
  FROM [dbo].[BL_RECIBOS] R
  JOIN vAutogestion_Legajos   L ON L.IdLegajo = R.IdLegajo 
  JOIN vAutogestion_Personas  P ON P.IdPersona = L.IdPersona 
  JOIN BL_LIQUIDACIONES     LIQ ON LIQ.IdLiquidacion = R.IdLiquidacion   
  JOIN BL_LIQUIDACIONESTIPOS LT ON LT.IdLiqTipo = LIQ.IdLiqTipo 
  WHERE Not R.[PDF_Nombre] is null
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vAutogestion_RecibosDetalle]'))
	DROP VIEW [dbo].[vAutogestion_RecibosDetalle]
GO

Create View [dbo].[vAutogestion_RecibosDetalle]
AS
Select 
Clave = Ltrim(R.Idliquidacion) + '-' + LTrim(R.IdLegajo),
R.IdLiquidacion, R.IdLegajo, P.IdPersona, 
[Apellido y Nombres] = P.nombre,
[Firmado] = case when R.Firmado = 0 then 'No'
                 when R.Firmado = 1 then 'Conforme (' + Convert(varchar(20),R.Firmado_Fecha,103) + ')'
			     when R.Firmado = 2 then 'Disconforme (' + Convert(varchar(20),R.Firmado_Fecha,103) + ')' 
             end,
[Visualizado] = Convert(varchar(20),R.Visualizado,103),
[Observación] = case when R.Observacion is null 
                     then 'No tiene'		  
                     else R.Observacion
                end,    
[Liquidación] = Ltrim(Month(R.Liquidacion_Mes))+'/'+ Rtrim(year(R.Liquidacion_Mes)) + ' - ' + R.Liquidacion_codigo + ' - ' +  R.Liquidacion_Nombre,
[Fecha Ingreso] = AL.FecIngreso, 
[Banco] = AL.BancoPagoDesc,
[Obra Social] = AL.ObraSocialDesc,
[Estado Civil] = EstadoCivil,
[Modalidad Contratación] = ModContratacionDesc,
[Situación de Revista] = DescSituacion,
[Zona] = ZonaDescripcion,
[Categoria] = case when C.Nombre is null 
                   then 'No tiene'		  
                   else C.Nombre
              end,
[Convenio] = case when V.Denominacion is null 
                   then 'No tiene'		  
                   else V.Denominacion
              end,
[Empresa] = R.empresa 
from [vAutogestion_Recibos] R
Join BL_Personas P				(nolock) ON R.idpersona = P.IdPersona 
Join BL_ANEXO_LIQUIDACIONES AL	(nolock) ON AL.IdLiquidacion = R.IdLiquidacion And AL.IdLegajo = R.IdLegajo 
Left Join CATEGORIAS C			(nolock) ON C.IdCategoria = AL.IdCategoria
left Join BL_CONVENIOS V		(nolock) ON V.IdConvenio = AL.IdConvenio 
GO

