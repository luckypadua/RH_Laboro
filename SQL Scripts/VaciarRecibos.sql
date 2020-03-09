/*
UPDATE [BL_RECIBOS] 
   SET [Visualizado] = Null
      ,[Firmado] = 0
      ,[Firmado_Fecha] = Null
      ,[Observacion] = Null
*/

select IdLegajo, 
       PDF_Nombre, 
	   PDF_RutaLOC,
	   PDF_RutaFTP,
	   MD5,
	   FTPUpLoad,
	   Visualizado,
	   Firmado,
	   Firmado_Fecha,
	   Observacion from [BL_RECIBOS] where Not PDF_Nombre is null