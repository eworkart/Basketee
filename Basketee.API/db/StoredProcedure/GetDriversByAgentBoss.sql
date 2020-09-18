-------------------------------------------------------------------------------

-- Author       LINSON JOSE

-- Created      16 SEP 2017

-- Purpose      Get Drivers By AgentBoss

-------------------------------------------------------------------------------

-- GetDriversByAgentBoss '12'

CREATE PROC [dbo].[GetDriversByAgentBoss]

@AbosID INT = 0

AS

BEGIN



 ;WITH cteDriverApp AS

 (

   SELECT drv.DrvrID,drv.DriverName

         FROM 

         dbo.[Order] ord

		 JOIN [dbo].[OrderDelivery] orddvr ON ord.OrdrID = orddvr.OrdrID

		 JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

         JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

		 JOIN [dbo].[Driver] drv ON orddvr.[DrvrID]=drv.[DrvrID] AND agtadm.AgenID = drv.[AgenID]

		 WHERE agtboss.AbosID = @AbosID

   )

   ,

   cteDriverTele AS

   (

        SELECT drv.DrvrID,drv.DriverName

        FROM 

         dbo.[TeleOrder] ord

	     JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

         JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

		 JOIN [dbo].[Driver] drv ON ord.[DrvrID]=drv.[DrvrID] AND agtadm.AgenID = drv.[AgenID]

		 WHERE agtboss.AbosID = @AbosID

   )

,cteDriverResult AS

 (

  SELECT DrvrID,DriverName  FROM cteDriverApp

    UNION ALL

  SELECT DrvrID,DriverName  FROM cteDriverTele

 )



SELECT DISTINCT DrvrID,DriverName FROM cteDriverResult



  

END