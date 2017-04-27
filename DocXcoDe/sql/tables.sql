SELECT 
	SCHEMA_NAME(o.schema_id) AS [�����], 
	o.name AS [������������], 
	o.object_id AS _object_id, 
	ep.value AS [��������]
FROM sys.objects o 
	INNER JOIN sys.extended_properties ep 
		ON ep.major_id = o.object_id 
		AND ep.minor_id = 0 
		AND ep.name = 'MS_Description' 
WHERE o.type_desc = 'USER_TABLE'
ORDER BY [�����], [������������] 
