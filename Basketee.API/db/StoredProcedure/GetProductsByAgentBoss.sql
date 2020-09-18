-------------------------------------------------------------------------------

-- Author       LINSON JOSE

-- Created      16 SEP 2017

-- Purpose      Get Products By AgentBoss

-------------------------------------------------------------------------------

-- GetProductsByAgentBoss '12'

CREATE PROC [dbo].[GetProductsByAgentBoss]

@AbosID INT = 0

AS

BEGIN



 ;WITH cteProdApp AS

 (

   SELECT prd.ProdID,prd.ProductName

         FROM 

         dbo.[Order] ord

		 JOIN dbo.OrderDetails orddet ON ord.OrdrID = orddet.OrdrID

		 JOIN dbo.Product prd ON orddet.ProdID = prd.ProdID

		 JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

         JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

		 WHERE agtboss.AbosID = @AbosID

   )

   ,

   cteProdTele AS

   (

        SELECT prd.ProdID,prd.ProductName

        FROM 

         dbo.[TeleOrder] ord

		 JOIN dbo.TeleOrderDetails orddet ON ord.TeleOrdID = orddet.TeleOrdID

		 JOIN dbo.Product prd ON orddet.ProdID = prd.ProdID

		 JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

         JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

		 WHERE agtboss.AbosID = @AbosID

   )

,cteProdResult AS

 (

  SELECT ProdID,ProductName

   FROM cteProdApp

    UNION ALL

  SELECT ProdID,ProductName

   FROM cteProdTele

 )



SELECT DISTINCT ProdID,ProductName FROM cteProdResult



  

END