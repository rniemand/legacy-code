SELECT
	distinct(a.client),
	count(1) as 'hits',
	round(sum(a.size) / 1048576, 2) as 'sizeMb'
FROM tb_access_raw a
WHERE a.timestamp >= date_sub(now(), interval 5 minute)
GROUP BY a.`client`