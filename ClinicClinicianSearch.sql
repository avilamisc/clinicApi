DECLARE @g geography;   
	SET @g = geography::Point(49.8118805, 24.0096293, 4326)

SELECT
		clinic.Id,
		clinic.Name as ClinicName,
		clinic.City as ClinicCity,
		clinic.Geolocation.STDistance(@g) as Distance,
		uniqCl.ClinicianId,
		uniqCl.ClinicianName,
		uniqCl.ClinicianRate,
		uniqCl.ClinicianSurname
	FROM Clinics as clinic
	LEFT JOIN (SELECT * FROM
				(SELECT ccc.ClinicianId, MIN(DISTANCE) as MinDistance FROM
					(SELECT
							cc.ClinicianId,
							cc.ClinicId,
							c.Distance
						FROM (SELECT Id, Geolocation.STDistance(@g) as Distance FROM [dbo].[Clinics]) AS c
						INNER JOIN (SELECT * FROM [dbo].[ClinicClinicians]) AS cc ON c.Id = cc.ClinicId) AS ccc
						GROUP BY ccc.ClinicianId) as T
				INNER JOIN (SELECT * FROM
					(SELECT
						Geolocation.STDistance(@g) as Distance,
						c.Id as ClinicId,
						cc.ClinicianId as ClId FROM Clinics AS c
						INNER JOIN ClinicClinicians AS cc ON c.Id = cc.ClinicId) as clcc
							INNER JOIN (SELECT
											cl.Rate as ClinicianRate,
											u.Id,
											u.Name as ClinicianName,
											u.Surname as ClinicianSurname FROM Clinicians as cl
											INNER JOIN Users as u ON u.Id = cl.Id) as cu
							ON cu.Id = clcc.ClId) AS T2
				ON T.ClinicianId = T2.ClId AND T.MinDistance = T2.Distance) AS uniqCl
	ON uniqCl.ClinicId = clinic.Id
	ORDER By Distance
