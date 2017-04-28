SELECT 
	SCHEMA_NAME(o.schema_id) + '.' + o.name AS [Наименование], 
	o.object_id AS _object_id, 
	ep.value AS [Описание]
FROM sys.objects o 
	LEFT JOIN sys.extended_properties ep 
		ON ep.major_id = o.object_id 
		AND ep.minor_id = 0 
		AND ep.name = 'MS_Description' 
WHERE o.type_desc = 'USER_TABLE'
ORDER BY [Наименование] 
