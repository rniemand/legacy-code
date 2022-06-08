SELECT
	a.client,
	a.size,
	a.fullUrl
FROM tb_access_raw a
WHERE a.timestamp >= date_sub(now(), interval 5 minute)
	AND a.content like '%image%'
ORDER BY a.size DESC