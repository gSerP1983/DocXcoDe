SELECT 
	c.name AS [Наименование], 
	ep.value AS [Описание], 
	CASE 
		WHEN t.name like '%char' THEN t.name + '(' + IIF(c.max_length = -1, 'MAX', CAST(c.max_length AS varchar(10)))+ ')'
		WHEN t.name = 'decimal' THEN t.name + '(' + CAST(c.precision AS varchar(10)) + ',' +CAST(c.scale AS varchar(10))+ ')'
		ELSE t.name
	END	AS [Тип],
	IIF(c.is_nullable=0, 'Да', 'Нет') AS [Обязательность],
	object_definition(c.default_object_id) [Умолчание]
FROM sys.columns  c
LEFT JOIN sys.types t 
	ON t.system_type_id = c.system_type_id
LEFT JOIN sys.extended_properties ep 
		ON ep.major_id = c.object_id 
		AND ep.minor_id = c.column_id 
		AND ep.name = 'MS_Description' 
WHERE object_id = {table._object_id}
ORDER BY 1
