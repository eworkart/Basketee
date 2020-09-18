ALTER TABLE [dbo].[ConsumerReview]
ADD CONSTRAINT ConsumerReview_ReasonID_FK FOREIGN KEY (ReasonID) REFERENCES [dbo].[ConsumerReviewReason]([ReasonID])