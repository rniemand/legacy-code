SELECT
	*
FROM tb_access_raw a
WHERE a.timestamp >= date_sub(now(), interval 5 minute)
	AND a.code = 404
ORDER BY a.accessId DESC