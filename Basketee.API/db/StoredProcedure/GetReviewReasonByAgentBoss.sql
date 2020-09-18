

-------------------------------------------------------------------------------

-- Author       LINSON JOSE

-- Created      16 SEP 2017

-- Purpose      Get Review Reason By AgentBoss

-------------------------------------------------------------------------------

--GetReviewReasonByAgentBoss 12,3,2,8

CREATE PROC [dbo].[GetReviewReasonByAgentBoss]

@AbosID INT = 0,

@DrvrID INT=0,

@PeriodType INT=0,

@PeriodRange INT=0

AS

BEGIN



SET FMTONLY OFF;

CREATE TABLE #tblServiceReason

(

  ReasonText VARCHAR(2000),

  Value DECIMAL(16,2)

)



IF(@AbosID > 0 AND @DrvrID > 0 AND @PeriodType > 0 AND @PeriodRange > 0)

BEGIN





SELECT crr.[ReasonText],crr.[ReasonID],od.DeliveryDate INTO #tmpReasons from [dbo].[ConsumerReview] cr

  JOIN [dbo].[ConsumerReviewReason] crr ON cr.[ReasonID]=crr.[ReasonID]

  JOIN [dbo].[OrderDelivery] od ON cr.DrvrID=od.DrvrID AND cr.OrdrID=od.OrdrID

  JOIN dbo.[Order] ord ON  ord.OrdrID=od.OrdrID

  JOIN dbo.AgentAdmin agtadm ON ord.AgadmID = agtadm.AgadmID

  JOIN dbo.AgentBoss agtboss ON agtadm.AgenID = agtboss.AgenID

 WHERE agtboss.AbosID = @AbosID AND cr.DrvrID=@DrvrID



DECLARE @StartDate Date

DECLARE @CurrentDate Date= GetDate()

IF(@PeriodType =1)

BEGIN

 

SET @StartDate=(SELECT CONVERT(DATE,dateadd(mm,datediff(mm,0,getdate())-(@PeriodRange-1),0)))



END

IF(@PeriodType = 2)

BEGIN



SET @StartDate=(SELECT CONVERT(DATE,dateadd(wk,datediff(wk,0,getdate())-(@PeriodRange-1),0)))



END



;WITH cteReasons AS

 (select r.[ReasonText],r.[ReasonID] from #tmpReasons r

 WHERE (r.DeliveryDate BETWEEN @StartDate AND @CurrentDate)

 )



INSERT INTO #tblServiceReason(ReasonText,Value)

select ReasonText,COUNT(ReasonID) from cteReasons

group by ReasonText



END



SELECT ReasonText,Value FROM #tblServiceReason



END


