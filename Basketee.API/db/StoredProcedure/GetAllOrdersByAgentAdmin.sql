
-------------------------------------------------------------------------------

-- Author       LINSON JOSE

-- Created      16 SEP 2017

-- Purpose      Get All Orders By AgentAdmin

-------------------------------------------------------------------------------
-- GetAllOrdersByAgentAdmin '12'

CREATE PROC [dbo].[GetAllOrdersByAgentAdmin]

@AgadmID INT = 0

AS

BEGIN



DECLARE @ORDER_TYPE_APP VARCHAR(25)='OrderApp'

DECLARE @ORDER_TYPE_TELP VARCHAR(25)='OrderTelp'

DECLARE @ID_ORDER_ACCEPTED INT= 2, @ID_ORDER_OUT_FOR_DELIVERY INT= 3, @ID_ORDER_CLOSED INT= 4



 ;WITH cteOrdersApp AS

 (

   SELECT DISTINCT ord.OrdrID,

        @ORDER_TYPE_APP AS OrderType,

		ord.InvoiceNumber,

		ord.OrderDate,

        ord.OrderTime,

		ord.StatusID,

		cons.Name AS ConsumerName,

		cons.PhoneNumber AS ConsumerMobile,

		addr.[Address] AS ConsumerAddress,

		ord.GrandTotal



         FROM 

         dbo.[Order] ord

		 JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

         JOIN dbo.Consumer cons ON ord.ConsID = cons.ConsID

		 JOIN dbo.ConsumerAddress addr ON ord.AddrID = addr.AddrID

		 WHERE agtadm.AgadmID = @AgadmID

   )

   ,

   cteOrdersTele AS

   (

         SELECT DISTINCT ord.TeleOrdID AS OrdrID,

        @ORDER_TYPE_TELP AS OrderType,

		ord.InvoiceNumber,

		ord.OrderDate,

        ord.OrderTime,

		ord.StatusID,

		cons.CustomerName AS ConsumerName,

		cons.MobileNumber AS ConsumerMobile,

		cons.[Address] AS ConsumerAddress,

		ord.GrantTotal AS GrandTotal



         FROM 

         dbo.[TeleOrder] ord

		 JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

       	 JOIN [dbo].[TeleCustomer] cons ON ord.TeleOrdID = cons.TeleOrdID

	     WHERE agtadm.AgadmID = @AgadmID

   )

,cteOrdersResult AS

 (

  SELECT OrdrID,OrderType,InvoiceNumber,OrderDate,OrderTime,StatusID,ConsumerName,ConsumerMobile,ConsumerAddress,GrandTotal

   FROM cteOrdersApp

    UNION ALL

  SELECT OrdrID,OrderType,InvoiceNumber,OrderDate,OrderTime,StatusID,ConsumerName,ConsumerMobile,ConsumerAddress,GrandTotal

   FROM cteOrdersTele

 )



SELECT OrdrID,OrderType,InvoiceNumber,OrderDate,OrderTime,StatusID,ConsumerName,ConsumerMobile,ConsumerAddress,GrandTotal

from cteOrdersResult

WHERE StatusID IN (@ID_ORDER_ACCEPTED,@ID_ORDER_OUT_FOR_DELIVERY,@ID_ORDER_CLOSED)

  

END



  
