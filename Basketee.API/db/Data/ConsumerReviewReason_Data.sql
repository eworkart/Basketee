INSERT INTO dbo.[ConsumerReviewReason](ReasonText)
SELECT 'Produk tidak sesuai'
UNION
SELECT  'Kualitas Produk'
UNION
SELECT 	'Tidak Sopan'
UNION
SELECT 	'Terlambat'