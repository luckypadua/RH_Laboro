/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
       [Codigo]
      ,[Nombre]
      ,[EmailPersonal]
      /*   
	  ,[HabilitadoAutogestion]
      ,[habilitadoPortal]
      ,[PublicarRecibos]
	  */
	  ,CUIL 
  FROM [dbo].[BL_PERSONAS]
  WHERE Codigo in ('000148','000214','000215','000261','000183','000529','000431','000491','000497','000506','000509','000527','000516')

SELECT * FROM [dbo].[BL_legajos]
  WHERE Legajo in ('000148','000214','000215','000261','000183','000529','000431','000491','000497','000506','000509','000527','000516')

UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'cscarpa@bas.com.ar' WHERE Codigo = '000148'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'AJara@bas.com.ar' WHERE Codigo = '000214'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'dsalum@bas.com.ar' WHERE Codigo = '000215'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'jruggero@bas.com.ar' WHERE Codigo = '000261'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'ivazquez@bas.com.ar' WHERE Codigo = '000183'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'gzanotti@bas.com.ar' WHERE Codigo = '000529'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'klopez@bas.com.ar' WHERE Codigo = '000431'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'RErcolano@bas.com.ar' WHERE Codigo = '000491'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'perbstein@bas.com.ar' WHERE Codigo = '000497'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'ycanceco@bas.com.ar' WHERE Codigo = '000506'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'aruibal@bas.com.ar' WHERE Codigo = '000509'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'hgabach@bas.com.ar' WHERE Codigo = '000527'
UPDATE [dbo].[BL_PERSONAS] set HabilitadoAutogestion = 1, habilitadoPortal = 1, PublicarRecibos = 1, [EmailPersonal] = 'mviscomi@bas.com.ar' WHERE Codigo = '000516'

/*
Codigo	Nombre						EmailPersonal			CUIL
000148	IBARRA, Hugo Benjamin		cscarpa@bas.com.ar		20235898382
000183	SCHIAVI, Rolando Carlos		ivazquez@bas.com.ar		20231685716
000214	BETCHAKIAN, Leonardo		AJara@bas.com.ar		20217866864
000215	VEIGA, Jose Maria			dsalum@bas.com.ar		20101212441
000261	RIBOLZI, Jorge Daniel		jruggero@bas.com.ar		20106101664
000431	ZACARIAS, Claudio Hugo		klopez@bas.com.ar		20169139610
000491	CHICCO, Julian Antonio		RErcolano@bas.com.ar	23405050919
000497	PAVON, Cristian David		perbstein@bas.com.ar	23395425919
000506	TEVEZ, Carlos Alberto		ycanceco@bas.com.ar		20305772551
000509	MESSIDORO, Alexis Nahuel	aruibal@bas.com.ar		20404979044
000516	FABRA PALACIOS, Frank Yusty	mviscomi@bas.com.ar		20627558857
000527	ROFFO, Manuel				hgabach@bas.com.ar		20423329859
000529	CAPALDO TABOAS, Nicolas		gzanotti@bas.com.ar		20414165800
*/
