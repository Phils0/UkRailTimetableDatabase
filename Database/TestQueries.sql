use Timetable;

select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, count(*)
from locations l
inner join schedulelocations sl on l.id = sl.locationid
where l.ThreeLetterCode = 'WAT'
or l.nlc like '5598%'
group by l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode
UNION 
select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, 0
from locations l
where l.ThreeLetterCode = 'WAT'
or l.nlc like '5598%';

select distinct l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description
from locations l
left outer join schedulelocations sl on l.id = sl.locationid
where sl.locationid is null and not l.ThreeLetterCode is null
order by l.ThreeLetterCode;


select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, count(*)
from locations l
inner join schedulelocations sl on l.id = sl.locationid
where l.ThreeLetterCode = 'SUR'
or l.nlc like '5571%'
group by l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode;

select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description, count(*)
from locations l
inner join schedulelocations sl on l.id = sl.locationid
where l.ThreeLetterCode = 'STP'
or l.nlc like '1555%' or l.nlc like '1540%' or l.Stanox = '63630' or l.Description like '%PANCRAS%'
group by l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description
UNION 
select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description, 0
from locations l
where l.ThreeLetterCode = 'STP'
or l.nlc like '1555%' or l.nlc like '1540%' or l.Stanox = '63630' or l.Description like '%PANCRAS%';

select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description, count(*)
from locations l
inner join schedulelocations sl on l.id = sl.locationid
where l.ThreeLetterCode = 'VXH'
or l.nlc like '5597%' or l.Stanox = '87214' or l.Description like '%VAUXHALL%'
group by l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description
UNION 
select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description, 0
from locations l
where l.ThreeLetterCode = 'VXH'
or l.nlc like '5597%' or l.Stanox = '87214' or l.Description like '%VAUXHALL%';

select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description, count(*)
from locations l
inner join schedulelocations sl on l.id = sl.locationid
where l.ThreeLetterCode = 'CLJ'
or l.nlc like '5595%' or l.Stanox = '87219' or l.Description like '%CLAPHAM J%'
group by l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description
UNION 
select l.id, l.tiploc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description, 0
from locations l
where l.ThreeLetterCode = 'CLJ'
or l.nlc like '5595%' or l.Stanox = '87219' or l.Description like '%CLAPHAM J%';


select top 100 *
from schedulelocations sl
where sl.locationid in (9419, 9420) ; -- 9405, 

select count(*), l.id, l.TipLoc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description
from schedulelocations sl
inner join locations l on l.id = sl.locationid
group by l.id, l.TipLoc, l.nlc, l.Stanox, l.ThreeLetterCode, l.Description
order by l.nlc;

select s.id, s.TimetableUid, s.StpIndicator, s.runsfrom, s.RunsTo, s.DayMask, s.toc, l.tiploc, l.description, l.nlc, l.threelettercode, sl.*
from schedules s 
inner join schedulelocations sl on sl.scheduleId = s.id
inner join locations l on l.id = sl.locationid
where  s.id IN (281856, 282019, 433574, 433429) --  s.id IN (281856, 282019, 433574, 433429) s.TimetableUid IN ('C10037', 'C10188')
order by sl.id;

select s.toc, s.Status, s.Category, count(*)
from Schedules s
group by s.toc, s.Status, s.Category
order by s.toc, s.Status, s.Category;

select s.Status, s.StpIndicator, count(*)
from Schedules s
group by s.Status, s.StpIndicator
order by s.Status, s.StpIndicator desc;

select top 10 *
from schedules s
where s.status is null;


select top 10 *
from associations;
