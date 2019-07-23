select top 10 abs(datediff(MINUTE, o.PublicDeparture, d.PublicArrival)) as duration, *
from Schedules s
inner join ScheduleLocations o on s.id = o.ScheduleId
inner join ScheduleLocations d on s.id = d.ScheduleId
where s.Toc = 'CS'
and (s.RunsFrom < getdate() and s.RunsTo > getdate() and s.DayMask & 4 = 4)
and s.StpIndicator = 'P'
and o.Activities = 'TB'
and d.Activities = 'TF';

select s.toc, s.category, COUNT(*)
from Schedules s
where (s.RunsFrom < getdate() and s.RunsTo > getdate() and s.DayMask & 64 = 64)
and s.StpIndicator = 'P'
and not s.category is null
group by s.toc, s.category
order by s.toc, s.category;

select s.toc, COUNT(*)
from Schedules s
inner join ScheduleLocations o on s.id = o.ScheduleId
inner join ScheduleLocations d on s.id = d.ScheduleId
where (s.RunsFrom < getdate() and s.RunsTo > getdate() and s.DayMask & 32 = 32)
and s.StpIndicator = 'P'
and o.Activities = 'TB'
and d.Activities = 'TF'
and (datediff(MINUTE, o.PublicDeparture, d.PublicArrival) > 120
    or (datediff(MINUTE, o.PublicDeparture, d.PublicArrival) < 0
            and datediff(MINUTE, cast(o.PublicDeparture as datetime), dateadd(hour, 24, cast(d.PublicArrival as datetime))) > 120))
group by s.toc
order by s.toc;