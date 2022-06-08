SELECT
	*
FROM tb_access_raw a
WHERE a.timestamp >= date_sub(now(), interval 30 minute)
	AND a.content like '%javascript%'
	AND a.fullUrl like '%.js'
	-- AND a.code = 304
ORDER BY a.accessId DESC