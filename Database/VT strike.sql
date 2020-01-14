select s.id, s.TimetableUid, s.RetailServiceId, s.StpIndicator, s.RunsFrom, s.RunsTo,  sl.*
from Schedules s
inner join ScheduleLocations sl on s.id = sl.ScheduleId
inner join Locations l on sl.LocationId = l.Id
inner join Stations st on st.TipLoc = l.TipLoc
where st.ThreeLetterCode = 'GLC'
and s.Toc = 'VT'
and sl.PublicDeparture = '09:40:00'
order by s.RunsFrom, s.StpIndicator;

select *
from Schedules s
where s.TimetableUid IN ('P58534',
'P58535',
'S11432',
'C72978',
'C72979',
'C72979',
'C72979')
order by s.RunsFrom, s.StpIndicator;