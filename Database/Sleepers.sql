with 
origins(ScheduleId, ThreeLetterCode) as
	(select sl.ScheduleId, l.ThreeLetterCode
	from ScheduleLocations sl
	inner join Locations l on l.Id = sl.LocationId
	where sl.Activities like '%TB%'),
destinations(ScheduleId, ThreeLetterCode) as
	(select sl.ScheduleId, l.ThreeLetterCode
	from ScheduleLocations sl
	inner join Locations l on l.Id = sl.LocationId
	where sl.Activities like '%TF%')
select o.ThreeLetterCode, d.ThreeLetterCode, s.RetailServiceId, s.*
from Schedules s
inner join origins o on s.id = o.ScheduleId
inner join destinations d on s.id = d.ScheduleId
where s.Toc = 'CS'
and d.ThreeLetterCode in ('EDB', 'GLC') -- ('ABD', 'FTW', 'INV')
union select null, null, s.RetailServiceId, s.*
from Schedules s
where s.StpIndicator = 'C' 
and s.Toc = 'CS'
and s.RetailServiceId like 'CS%003%'
order by s.RunsFrom, s.DayMask, s.TimetableUid;

with 
origins(ScheduleId, ThreeLetterCode) as
	(select sl.ScheduleId, l.ThreeLetterCode
	from ScheduleLocations sl
	inner join Locations l on l.Id = sl.LocationId
	where sl.Activities like '%TB%'),
destinations(ScheduleId, ThreeLetterCode) as
	(select sl.ScheduleId, l.ThreeLetterCode
	from ScheduleLocations sl
	inner join Locations l on l.Id = sl.LocationId
	where sl.Activities like '%TF%')
select o.ThreeLetterCode, d.ThreeLetterCode, s.RetailServiceId, s.*
from Schedules s
inner join origins o on s.id = o.ScheduleId
inner join destinations d on s.id = d.ScheduleId
where s.Toc = 'CS'
and o.ThreeLetterCode in ('EDB', 'GLC') -- ('ABD', 'FTW', 'INV')
union select null, null, s.RetailServiceId, s.*
from Schedules s
where s.StpIndicator = 'C' 
and s.Toc = 'CS'
and s.RetailServiceId like 'CS%004%'
order by s.RunsFrom, s.DayMask, s.TimetableUid;

select null, null, s.RetailServiceId, s.*
from Schedules s
where s.Toc = 'CS'
and s.RetailServiceId like 'CS1001%'
order by s.RunsFrom, s.DayMask, s.TimetableUid;

select *
from Associations a
where a.MainUid in (select s.TimetableUid
	from Schedules s
	where s.Toc = 'CS' and SUBSTRING(s.RetailServiceId, 1, 6) IN ('CS0001', 'CS1001', 'CS2001', 'CS3001', 'CS7001'))
or a.AssociatedUid in (select s.TimetableUid
	from Schedules s
	where s.Toc = 'CS' and SUBSTRING(s.RetailServiceId, 1, 6) IN ('CS0001', 'CS1001', 'CS2001', 'CS3001', 'CS7001'))
order by a.RunsFrom, a.MainUid;

select *
from Associations a
where a.MainUid in (select s.TimetableUid
	from Schedules s
	where s.Toc = 'CS' and SUBSTRING(s.RetailServiceId, 1, 6) IN ('CS0002', 'CS1002', 'CS2002', 'CS3002', 'CS7002', 'CS9002'))
or a.AssociatedUid in (select s.TimetableUid
	from Schedules s
	where s.Toc = 'CS' and SUBSTRING(s.RetailServiceId, 1, 6) IN ('CS0002', 'CS1002', 'CS2002', 'CS3002', 'CS7002', 'CS9002'))
order by a.RunsFrom, a.MainUid;

select distinct m.RetailServiceId, o.RetailServiceId
from Associations a
inner join Schedules m on m.TimetableUid = a.MainUid
inner join Schedules o on o.TimetableUid = a.AssociatedUid
where a.DateIndicator = 'N';

with 
origins(ScheduleId, ThreeLetterCode, PublicDeparture) as
	(select sl.ScheduleId, l.ThreeLetterCode, sl.PublicDeparture
	from ScheduleLocations sl
	inner join Locations l on l.Id = sl.LocationId
	where sl.Activities like '%TB%'),
destinations(ScheduleId, ThreeLetterCode, PublicArrival) as
	(select sl.ScheduleId, l.ThreeLetterCode, sl.PublicArrival
	from ScheduleLocations sl
	inner join Locations l on l.Id = sl.LocationId
	where sl.Activities like '%TF%')
select o.ThreeLetterCode, o.PublicDeparture, d.ThreeLetterCode, d.PublicArrival, s.RetailServiceId, s.*
from Schedules s
inner join origins o on s.id = o.ScheduleId
inner join destinations d on s.id = d.ScheduleId
where s.RetailServiceId IN ('SN809900', 'SN809901');


select *
from ScheduleLocations sl
inner join Locations l on sl.LocationId = l.id
where sl.ScheduleId = 363760
and l.ThreeLetterCode = 'EDB';



select l.ThreeLetterCode, sl.*
from Schedules s
inner join ScheduleLocations sl on s.id = sl.ScheduleId
inner join Locations l on sl.LocationId = l.id
where s.TimetableUid = 'G65053'; -- sl.ScheduleId = 363760
and l.ThreeLetterCode IN ('EDB', 'HYM');

select *
from Schedules s
where s.TimetableUid in ('Q03385', 'W32185');