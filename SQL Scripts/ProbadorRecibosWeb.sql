use "400BLB_Prueba"

declare @Legajo as varchar (10) = '000497'
declare @IdLiquidacion as int = 1488
declare @IdLegajo as int
declare @IdPersona as int 
select @IdLegajo = IdLegajo, @IdPersona = IdPersona from bl_legajos where legajo = @Legajo

select 'IdPersona: ' + Ltrim(@IdPersona)

SELECT  IdLegajo,IdLiquidacion, 
	[PDF_Nombre],
	[PDF_RutaLOC],
	[PDF_RutaFTP],
	[MD5],
	[FTPUpLoad],
	[Visualizado],
	[Firmado],
	[Firmado_Fecha],
	[Observacion]
FROM [BL_RECIBOS] where Idlegajo = @IdLegajo and IdLiquidacion = @IdLiquidacion