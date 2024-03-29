/****** Script for SelectTopNRows command from SSMS  ******/


	DECLARE @g geography;
	DECLARE @latitude INT;  /* latitiude is in [-90, 90]  */
	DECLARE @longitude INT; /* longitude is in [-180, 180]*/
	DECLARE @i int = 0
	WHILE @i < 300 
	BEGIN
		SET @i = @i + 1
		SELECT @longitude = ROUND((RAND() * 180) - (RAND() * 180), 0)
		SELECT @latitude = ROUND((RAND() * 90) - (RAND() * 90), 0)
		SET @g = geography::Point(@latitude, @longitude, 4326)
		INSERT INTO [ClinicDb].[dbo].[Clinics] (Name, City, Geolocation)
			VALUES('Name_' + CONVERT(varchar(10), @i),
				   'City_' + CONVERT(varchar(10), (ROUND((RAND() * 180), 0) * 10)),
				   @g)
	END

SELECT TOP (1000) [Id]
      ,[Name]
      ,[City]
      ,[Geolocation]
FROM [ClinicDb].[dbo].[Clinics]