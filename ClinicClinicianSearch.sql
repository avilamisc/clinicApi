SELECT * FROM
	(SELECT ClinicClinicians.ClinicianId, ClinicClinicians.ClinicId, Clinics.Geolocation, Clinics.City, Name
		  FROM [dbo].[Clinics]
			INNER JOIN [dbo].[ClinicClinicians] ON [dbo].[Clinics].Id = ClinicId) a
	GROUP BY a.ClinicianId