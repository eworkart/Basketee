
-------------------------------------------------------------------------------

-- Author       LINSON JOSE

-- Created      16 SEP 2017

-- Purpose      Get Review Report By AgentBoss

-------------------------------------------------------------------------------
--GetReviewReportByAgentBoss 12,3,2,8

CREATE PROC [dbo].[GetReviewReportByAgentBoss]

@AbosID INT = 0,

@DrvrID INT=0,

@PeriodType INT=0,

@PeriodRange INT=0

AS

BEGIN



SET FMTONLY OFF;

CREATE TABLE #tblServiceRating 

(

  Period VARCHAR(25),

  Value DECIMAL(16,2)

)



IF(@AbosID > 0 AND @DrvrID > 0 AND @PeriodType > 0 AND @PeriodRange > 0)

BEGIN



DECLARE @StartDate Date

DECLARE @CurrentDate Date= GetDate()

IF(@PeriodType =1)

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



;WITH cteReviewDetailsMonth AS

 (select DateAdd(day,1,EOMONTH(od.DeliveryDate,-1)) AS StartMonth,EOMONTH(od.DeliveryDate) EndMonth,od.DeliveryDate,cr.Rating from [dbo].[ConsumerReview] cr

  JOIN [dbo].[OrderDelivery] od ON cr.DrvrID=od.DrvrID AND cr.OrdrID=od.OrdrID

  JOIN dbo.[Order] ord ON  ord.OrdrID=od.OrdrID

  JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

  JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

 WHERE agtboss.AbosID = @AbosID AND cr.DrvrID=@DrvrID AND (od.DeliveryDate BETWEEN @StartDate AND @CurrentDate))

,cteGroupMonth AS(

select StartMonth,AVG(Rating) AS Rating from cteReviewDetailsMonth

group by StartMonth

)



INSERT INTO #tblServiceRating(Period,Value)

Select p.Period,IIF(g.StartMonth IS NULL,0,g.Rating) Value from #tmpMonths p

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



;WITH cteReviewDetailsWeek AS

 (select CONVERT(DATE,DATEADD(ww, DATEDIFF(ww,0,od.DeliveryDate), 0)) AS StartWeek,od.DeliveryDate,cr.Rating from [dbo].[ConsumerReview] cr

  JOIN [dbo].[OrderDelivery] od ON cr.DrvrID=od.DrvrID AND cr.OrdrID=od.OrdrID

  JOIN dbo.[Order] ord ON  ord.OrdrID=od.OrdrID

  JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

  JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

  WHERE agtboss.AbosID = @AbosID AND cr.DrvrID=@DrvrID  AND (od.DeliveryDate BETWEEN @StartDate AND @CurrentDate))

,cteGroupWeek AS(

select StartWeek,AVG(Rating) AS Rating from cteReviewDetailsWeek

group by StartWeek

)



INSERT INTO #tblServiceRating(Period,Value)

Select p.Period,IIF(g.StartWeek IS NULL,0,g.Rating) Value from #tmpWeeks p

LEFT JOIN cteGroupWeek g ON p.StartWeek=g.StartWeek



END



END



SELECT Period,Value FROM #tblServiceRating



END


