

-------------------------------------------------------------------------------

-- Author       LINSON JOSE

-- Created      16 SEP 2017

-- Purpose      Get SellerReport By AgentBoss

-------------------------------------------------------------------------------

--GetSellerReportByAgentBoss 12,1,2,8,'1,2,3'

CREATE PROC [dbo].[GetSellerReportByAgentBoss]

@AbosID INT = 0,

@TotalType INT=0,

@PeriodType INT=0,

@PeriodRange INT=0,

@ProductIds VARCHAR(4000)=''

AS

BEGIN



SET FMTONLY OFF;

CREATE TABLE #tblServiceRating 

(

  Period VARCHAR(25),

  Value DECIMAL(16,2)

)



IF(@ProductIds!='' AND @AbosID > 0 AND @TotalType > 0 AND @PeriodType > 0 AND @PeriodRange > 0)

BEGIN



DECLARE @tblProducts TABLE(ProductId INT NOT NULL)



INSERT INTO @tblProducts(ProductId)

 SELECT CAST(Item AS INTEGER) FROM dbo.SplitString(@ProductIds, ',')



 DECLARE @ID_ORDER_CLOSED INT= 4

 ;WITH cteOrdersApp AS

 (

   SELECT DISTINCT 

        ord.OrdrID,

    	orddet.OrdtID,

		orddet.ProdID,

		orddet.TotamAmount AS TotalAmount,

		od.DeliveryDate

        FROM 

         dbo.[Order] ord

		 JOIN dbo.OrderDetails orddet ON ord.OrdrID = orddet.OrdrID

		 JOIN [dbo].[OrderDelivery] od ON  ord.OrdrID=od.OrdrID

		 JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

         JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

		 WHERE agtboss.AbosID = @AbosID AND

		 ord.StatusID IN (@ID_ORDER_CLOSED)

		 AND orddet.ProdID in(SELECT ProductId FROM @tblProducts)

   )

   ,

   cteOrdersTele AS

   (

        SELECT DISTINCT 

		ord.TeleOrdID AS OrdrID,

		orddet.TeleOrdDetID OrdtID,

		orddet.ProdID,

		orddet.TotalAmount,

		ord.DeliveryDate



         FROM 

         dbo.[TeleOrder] ord

		 JOIN dbo.TeleOrderDetails orddet ON ord.TeleOrdID = orddet.TeleOrdID

		 JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

         JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

		 WHERE agtboss.AbosID = @AbosID

   )



   SELECT * INTO #tmpProductDetails FROM

   (

    SELECT OrdrID,OrdtID,ProdID,TotalAmount,DeliveryDate FROM cteOrdersApp

    UNION ALL

    SELECT OrdrID,OrdtID,ProdID,TotalAmount,DeliveryDate FROM cteOrdersTele

   )res



DECLARE @StartDate Date

DECLARE @CurrentDate Date= GetDate()

IF(@PeriodType =1 )

BEGIN

 

;with cteMonths as

(

select 0 as num

union all

select num+1 from cteMonths where num < (@PeriodRange-1)

)

select dates AS FromDate,EOMONTH(dates) AS ToDate

,FORMAT(dates,'MMM') +' '+ CONVERT(varchar(20),year(dates)) AS Period

INTO #tmpMonths

from 

(

select dateadd(mm,-num,dateadd(dd,1,eomonth(@CurrentDate,-1))) as dates

from cteMonths

) m order by dates



SET @StartDate=(SELECT TOP 1 FromDate FROM #tmpMonths Order by FromDate ASC)



;WITH cteProdDetailsMonth AS

 (

 select DateAdd(day,1,EOMONTH(p.DeliveryDate,-1)) AS StartMonth,EOMONTH(p.DeliveryDate) EndMonth,p.DeliveryDate,p.TotalAmount 

  from #tmpProductDetails p

  WHERE (p.DeliveryDate BETWEEN @StartDate AND @CurrentDate)

  )

,cteGroupMonth AS(

select StartMonth,AVG(TotalAmount) AS Amount,COUNT(TotalAmount) AS Number from cteProdDetailsMonth

group by StartMonth

)



INSERT INTO #tblServiceRating(Period,Value)

Select p.Period,IIF(g.StartMonth IS NULL,0,IIF(@TotalType = 1,g.Amount,g.Number)) Value from #tmpMonths p

LEFT JOIN cteGroupMonth g ON p.FromDate=g.StartMonth



END

IF(@PeriodType = 2)

BEGIN

declare @setting date = GETDATE()

;WITH cteWeeks AS(

        SELECT  DATEADD(D, - DATEPART (WEEKDAY,@setting)+2, @setting) AS StartWeek, @setting  AS EndWeek, 1 AS WeekNo

        UNION ALL

        SELECT  DATEADD(D,-7,StartWeek),DATEADD(D,-1,StartWeek), WEEKNO + 1

        FROM   cteWeeks  WHERE WEEKNO < @PeriodRange

)

select 'Week '+ CONVERT(VARCHAR(20),WeekNo) AS Period,StartWeek,EndWeek INTO #tmpWeeks from cteWeeks



SET @StartDate=(SELECT TOP 1 StartWeek FROM #tmpWeeks Order by StartWeek ASC)



;WITH cteProdDetailsWeek AS

 (select CONVERT(DATE,DATEADD(ww, DATEDIFF(ww,0,p.DeliveryDate), 0)) AS StartWeek,p.DeliveryDate,p.TotalAmount 

  from #tmpProductDetails p

  WHERE (p.DeliveryDate BETWEEN @StartDate AND @CurrentDate)

  )

,cteGroupWeek AS(

select StartWeek,AVG(TotalAmount) AS Amount,COUNT(TotalAmount) AS Number from cteProdDetailsWeek

group by StartWeek

)



INSERT INTO #tblServiceRating(Period,Value)

Select p.Period,IIF(g.StartWeek IS NULL,0,IIF(@TotalType = 1,g.Amount,g.Number)) Value from #tmpWeeks p

LEFT JOIN cteGroupWeek g ON p.StartWeek=g.StartWeek



END



END



SELECT Period,Value FROM #tblServiceRating



END


